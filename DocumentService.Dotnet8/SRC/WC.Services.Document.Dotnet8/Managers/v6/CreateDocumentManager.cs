// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   WC.Services.Document
//   CreateDocumentManager.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.Document.Dotnet8.Managers.v6
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using WC.Services.Document.Dotnet8.Adapters.v6;
	using Hyland.Unity;
	using Hyland.Unity.UnityForm;
	using Microsoft.Extensions.Logging;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using CareSource.WC.Entities.Exceptions;
    using WC.Services.Document.Dotnet8.Adapters.v6.Interfaces;
    using WC.Services.Document.Dotnet8.Managers.v6.Interfaces;
    using WC.Services.Document.Dotnet8.Models.v6;
    using WC.Services.Document.Dotnet8.Models.v6.Requests;

    public class CreateDocumentManager : ICreateDocumentManager<OnBaseDocument>
	{
		private readonly ICreateDocumentAdapter _createDocumentAdapter;
		private readonly IFileAdapter _fileAdapter;
		private readonly IGetDocumentAdapter<OnBaseDocument> _getDocumentAdapter;
		private readonly log4net.ILog _logger;
		private readonly IConfiguration _configuration;
		private readonly IOnBaseAdapter _onBaseAdapter;

		public CreateDocumentManager(
			ICreateDocumentAdapter createDocumentAdapter,
			IGetDocumentAdapter<OnBaseDocument> getDocumentAdapter,
			IConfiguration configuration,
			IFileAdapter fileAdapter,
			IOnBaseAdapter onBaseAdapter,
			log4net.ILog logger
		)
		{
			_configuration = configuration;
			_createDocumentAdapter = createDocumentAdapter;
			_getDocumentAdapter = getDocumentAdapter;
			_fileAdapter = fileAdapter;
			_onBaseAdapter = onBaseAdapter;
			_logger = logger;
		}

		public OnBaseDocument CreateDocument(CreateDocumentFileLinkRequest request)
		{
			string rootFilePath = _configuration["FileServerSettings:OnBase.FileServer.Root"];
			string importFilePath = _configuration["FileServerSettings:Import.FileDrop"];
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
			_logger.Debug(
				"Validating Request"+
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
							if (String.IsNullOrWhiteSpace(value))
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

					if (!String.IsNullOrWhiteSpace(keyword.Value))
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
				when(e is CareSource.WC.Entities.Exceptions.InvalidDocumentTypeException)
			{
				state.AddModelError("Invalid Document Type", e.Message);
			}

			return state.IsValid;
		}

		private string CreateInvalidDataMessage(string key)
			=> $"Invalid data for {key}.";
	}
}