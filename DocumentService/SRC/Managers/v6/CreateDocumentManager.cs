// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   WC.Services.Document
//   CreateDocumentManager.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.Document.Managers.v6
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Web.Http.ModelBinding;
	using CareSource.WC.OnBase.Core.Configuration.Interfaces;
	using CareSource.WC.OnBase.Core.ExtensionMethods;
	using CareSource.WC.Services.Document.Adapters.v6;
	using CareSource.WC.Services.Document.Models.v6;
	using Hyland.Unity;
	using Hyland.Unity.UnityForm;
	using Microsoft.Extensions.Logging;

	public class CreateDocumentManager : ICreateDocumentManager<OnBaseDocument>
	{
		private readonly ICreateDocumentAdapter _createDocumentAdapter;
		private readonly IFileAdapter _fileAdapter;
		private readonly IGetDocumentAdapter<OnBaseDocument> _getDocumentAdapter;
		private readonly ILogger _logger;
		private readonly ISettingsAdapter _settingsAdapter;
		private readonly IOnBaseAdapter _onBaseAdapter;

		public CreateDocumentManager(
			ICreateDocumentAdapter createDocumentAdapter,
			IGetDocumentAdapter<OnBaseDocument> getDocumentAdapter,
			ISettingsAdapter settingsAdapter,
			IFileAdapter fileAdapter,
			IOnBaseAdapter onBaseAdapter,
			ILogger logger
		)
		{
			_settingsAdapter = settingsAdapter;
			_createDocumentAdapter = createDocumentAdapter;
			_getDocumentAdapter = getDocumentAdapter;
			_fileAdapter = fileAdapter;
			_onBaseAdapter = onBaseAdapter;
			_logger = logger;
		}

		public OnBaseDocument CreateDocument(CreateDocumentFileLinkRequest request)
		{
			string rootFilePath = _settingsAdapter.GetSetting("OnBase.FileServer.Root");
			string importFilePath = _settingsAdapter.GetSetting("Import.FileDrop");
			string filePath = $"{rootFilePath}\\{importFilePath}\\{request.RequestData.FileName}";

			if (!_fileAdapter.FileExists(filePath))
			{
				throw new FileNotFoundException(
					$"Unable to find file '{request.RequestData.FileName}'.");
			}

			long? documentId = null;
			using (Stream fileStream = _fileAdapter
						.ReadFileContents(filePath))
			{
				if (fileStream.Length > 0)
				{
					documentId = _createDocumentAdapter.CreateDocument(
						request.RequestData.DocumentType,
						request.RequestData.Keywords,
						request.RequestData.FileName,
						new List<Stream> { fileStream });
				}
				else
				{
					throw new InvalidDataException("Document has no content(0 bytes).");
				}

			}

			if (!documentId.HasValue)
			{
				return null;
			}

			OnBaseDocument document = _getDocumentAdapter.GetDocument(
				documentId.Value,
				new DownloadRequest
				{
					DisplayColumns = request.RequestData.Keywords
						.Select(kvp => kvp.Key)
						.ToList(),
					CorrelationGuid = request.CorrelationGuid,
					SourceApplication = request.SourceApplication,
					UserId = request.UserId
				});

			if (document != null)
			{
				document.FileData = null;
				document.Filename = null;
				_fileAdapter.DeleteFile(filePath);
			}

			return document;

		}

		public OnBaseDocument CreateDocument(CreateDocumentRequest request)
		{
			long? documentId = _createDocumentAdapter.CreateDocument(
				request.RequestData.DocumentType,
				request.RequestData.Keywords);

			if (!documentId.HasValue)
			{
				return null;
			}

			OnBaseDocument document = _getDocumentAdapter.GetDocument(
				documentId.Value,
				new DownloadRequest
				{
					DisplayColumns = request.RequestData.Keywords
						.Select(kvp => kvp.Key)
						.ToList(),
					CorrelationGuid = request.CorrelationGuid,
					SourceApplication = request.SourceApplication,
					UserId = request.UserId
				});

			return document;
		}

		public bool IsValid(
			CreateDocumentRequest request,
			ModelStateDictionary state)
		{
			_logger.LogDebug(
				"Validating Request",
				new Dictionary<string, object>
				{
					{ "request", request },
					{ "Model State", state }
				});
			
			try
			{
				(FormTemplate template, StoreNewUnityFormProperties props) =
					_onBaseAdapter.GetUnityFormTemplate(request.RequestData.DocumentType);

				foreach (KeywordType keyword in template.DocumentType.KeywordTypesRequiredForArchival)
				{
					if (request.RequestData.Keywords.ContainsKey(keyword.Name))
					{
						if (request.RequestData.Keywords.TryGetValue(
							keyword.Name,
							out string value))
						{
							if (value.IsNullOrWhiteSpace())
							{
								state.AddModelError(
									"Missing Required Keyword(s)",
									keyword.Name);
							}
						}
					}
					else
					{
						state.AddModelError(
							"Missing Required Keyword(s)",
							keyword.Name);
					}
				}

				foreach (KeyValuePair<string, string> keyword in request.RequestData.Keywords)
				{
					KeywordType kw =
						template.DocumentType.KeywordRecordTypes.FindKeywordType(keyword.Key);

					if (kw == null)
					{
						state.AddModelError("Invalid Keyword(s)", keyword.Key);
						continue;
					}

					if (!keyword.Value.IsNullOrWhiteSpace())
					{
						switch (kw.DataType)
						{
							case KeywordDataType.Numeric9:
								if (!int.TryParse(
									keyword.Value,
									out _))
								{
									state.AddModelError(
										CreateInvalidDataMessage(keyword.Key),
										keyword.Value);
								}

								break;
							case KeywordDataType.Numeric20:
								if (!long.TryParse(
									keyword.Value,
									out _))
								{
									state.AddModelError(
										CreateInvalidDataMessage(keyword.Key),
										keyword.Value);
								}

								break;
							case KeywordDataType.AlphaNumeric:
								if (((long)keyword.Value.Length > kw.DataLength))
								{
									state.AddModelError(
										$"Value exceeds Keyword [{keyword.Key}] max length of {kw.DataLength}.",
										keyword.Value);
								}

								break;
							case KeywordDataType.Currency:
							case KeywordDataType.SpecificCurrency:
								if (!decimal.TryParse(
									keyword.Value,
									out _))
								{
									state.AddModelError(
										CreateInvalidDataMessage(keyword.Key),
										keyword.Value);
								}

								break;
							case KeywordDataType.Date:
							case KeywordDataType.DateTime:
								if (!DateTime.TryParse(
									keyword.Value,
									out _))
								{
									state.AddModelError(
										CreateInvalidDataMessage(keyword.Key),
										keyword.Value);
								}

								break;
							case KeywordDataType.FloatingPoint:
								if (!double.TryParse(
									keyword.Value,
									out _))
								{
									state.AddModelError(
										CreateInvalidDataMessage(keyword.Key),
										keyword.Value);
								}

								break;
							case KeywordDataType.Undefined:
							default:
								continue;
						}
					}

				}

			}
			catch (Exception e)
				when(e is InvalidDocumentTypeException)
			{
				state.AddModelError("Invalid Document Type", e.Message);
			}

			return state.IsValid;
		}

		private string CreateInvalidDataMessage(string key)
			=> $"Invalid data for {key}.";
	}
}