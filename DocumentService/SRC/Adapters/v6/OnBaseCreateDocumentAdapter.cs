// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   WC.Services.Document
//   OnBaseCreateDocumentAdapter.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.Document.Adapters.v6
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Text.RegularExpressions;
	using CareSource.WC.OnBase.Core.Connection.Interfaces;
	using CareSource.WC.OnBase.Core.ExtensionMethods;
	using CareSource.WC.Services.Document.Models.v6;
	using Hyland.Unity;
	using Hyland.Unity.UnityForm;
	using Microsoft.Extensions.Logging;
	using Document = Hyland.Unity.Document;

	public class OnBaseCreateDocumentAdapter : ICreateDocumentAdapter
	{
		private const string Html = "HTML";

		private const string ImageFileFormat = "Image File Format";
		private const string MsExcelSpreadsheet = "MS Excel Spreadsheet";
		private const string MsPowerPoint = "MS Power Point";
		private const string MsWordDocument = "MS Word Document";
		private const string Pdf = "PDF";
		private const string RichTextFormat = "Rich Text Format";
		private const string TextReportFormat = "Text Report Format";
		private const string WavAudioFile = "WAV Audio File";
		private const string ZipCompressionArchive = "Zip Compression Archive";
		private readonly IApplicationConnectionAdapter<Application> _applicationConnectionAdapter;
		private readonly ICreateKeywordAdapter<Keyword> _createKeywordAdapter;
		private readonly List<string> _invalidKeywords = new List<string>();

		private readonly Dictionary<string, string> _fileTypes = new Dictionary<string, string>
		{
			{ "tif", ImageFileFormat },
			{ "tiff", ImageFileFormat },
			{ "jpg", ImageFileFormat },
			{ "jpe", ImageFileFormat },
			{ "jpeg", ImageFileFormat },
			{ "gif", ImageFileFormat },
			{ "png", ImageFileFormat },
			{ "pdf", Pdf },
			{ "doc", MsWordDocument },
			{ "docx", MsWordDocument },
			{ "xls", MsExcelSpreadsheet },
			{ "xlsx", MsExcelSpreadsheet },
			{ "csv", MsExcelSpreadsheet },
			{ "txt", TextReportFormat },
			{ "wav", WavAudioFile },
			{ "zip", ZipCompressionArchive },
			{ "html", Html },
			{ "htm", Html },
			{ "ppt", MsPowerPoint },
			{ "pptx", MsPowerPoint },
			{ "rtf", RichTextFormat }
		};

		private readonly ILogger _logger;
		private readonly IOnBaseAdapter _onBaseAdapter;

		public OnBaseCreateDocumentAdapter(
			ICreateKeywordAdapter<Keyword> createKeywordAdapter,
			IApplicationConnectionAdapter<Application> applicationConnectionAdapter,
			IOnBaseAdapter onBaseAdapter,
			ILogger logger)
		{
			_createKeywordAdapter = createKeywordAdapter;
			_applicationConnectionAdapter = applicationConnectionAdapter;
			_onBaseAdapter = onBaseAdapter;
			_logger = logger;
		}

		public long? CreateDocument(
			string documentType,
			IDictionary<string, string> keywords,
			string fileName,
			List<Stream> documentPages)
		{
			if (string.IsNullOrWhiteSpace(documentType))
			{
				throw new ArgumentException(
					"Must have Document Type object when adding add a new document to OnBase.");
			}

			DocumentTypeList documentTypeList = _applicationConnectionAdapter
				.Application
				.Core
				.DocumentTypes;

			DocumentType obDocumentType = documentTypeList.Find(documentType);

			if (obDocumentType == null)
			{
				throw new ArgumentException(
					$"Could not find document type '{documentType}' in OnBase.");
			}

			_logger.LogDebug(
				$"Creating document storage properties for '{obDocumentType}' document type name " +
				$"and '{obDocumentType.DefaultFileType.Name}' file type name.");

			string fileTypeExtension = obDocumentType.DefaultFileType.Extension;

			if (!string.IsNullOrEmpty(fileName))
			{
				Match fileExtensionSearch = Regex.Match(
					fileName,
					@"\.([A-z,0-9]+)$");

				if (!fileExtensionSearch.Success &&
					fileExtensionSearch.Groups[1]
						.Success)
				{
					throw new ArgumentException(
						$"Unable to find file extension from file name '{fileName}'.");
				}

				_logger.LogDebug(
					$"Overriding default file extension '{fileTypeExtension}' " +
					$"with found file extension '{fileExtensionSearch.Groups[1].Value}' " +
					$"from file name '{fileName}'.");

				fileTypeExtension = fileExtensionSearch.Groups[1]
					.Value;
			}

			FileType fileType = GetFileTypeByExtension(fileTypeExtension) ??
				obDocumentType.DefaultFileType;

			StoreNewDocumentProperties newDocumentProperties = _applicationConnectionAdapter
				.Application
				.Core
				.Storage
				.CreateStoreNewDocumentProperties(
					obDocumentType,
					fileType);

			AddKeywordRecordsToDocumentProperties(
				obDocumentType,
				keywords,
				newDocumentProperties);

			IEnumerable<PageData> pages = documentPages
				.Select(
					p => _applicationConnectionAdapter
						.Application
						.Core
						.Storage
						.CreatePageData(
							p,
							fileTypeExtension));

			_logger.LogInformation(
				$"Saving document for '{obDocumentType.Name}' " +
				"document type name through Hyland Unity API.");

			Document doc = _applicationConnectionAdapter
				.Application
				.Core
				.Storage
				.StoreNewDocument(
					pages,
					newDocumentProperties);

			return doc.ID;
		}

		public long? CreateDocument(
			string documentType,
			IDictionary<string, string> keywords)
		{
			(FormTemplate template, StoreNewUnityFormProperties props) =
				_onBaseAdapter.GetUnityFormTemplate(documentType);

			foreach (KeyValuePair<string, string> kvp in keywords)
			{
				try
				{
					props.AddKeyword(
						_createKeywordAdapter.CreateKeyword(
							kvp.Value,
							kvp.Key,
							template.DocumentType,
							false));
				}
				catch (InvalidKeywordException e)
				{
					_logger.LogWarning(e.Message);
					_invalidKeywords.Add(kvp.Key);
				}
			}

			if (_invalidKeywords.Any())
			{
				throw new InvalidKeywordException(
					string.Join(
						$",{Environment.NewLine}",
						_invalidKeywords));
			}

			return _onBaseAdapter.CreateUnityForm(props);
		}

		private void AddKeywordRecordsToDocumentProperties(
			DocumentType documentType,
			IDictionary<string, string> keywords,
			AddOnlyKeywordModifier newDocumentProperties)
		{
			List<string> unfoundKeywords = keywords
				.Select(kvp => kvp.Key)
				.ToList();

			foreach (KeywordRecordType keywordRecordType in documentType.KeywordRecordTypes)
			{
				if (keywordRecordType?.RecordType == null)
				{
					continue;
				}

				if (keywordRecordType.RecordType == RecordType.StandAlone ||
					keywordRecordType.RecordType == RecordType.SingleInstance)
				{
					//Add the Single Instance Keywords
					foreach (KeywordType keywordType in keywordRecordType.KeywordTypes)
					{
						string newKeywordValue = null;

						if (!keywords.TryGetValue(
								keywordType.Name,
								out newKeywordValue) &&
							string.IsNullOrWhiteSpace(newKeywordValue))
						{
							continue;
						}

						unfoundKeywords.Remove(keywordType.Name);

						Keyword keyword = _createKeywordAdapter.CreateKeyword(
							newKeywordValue,
							keywordType.Name,
							documentType.Name,
							false);

						newDocumentProperties.AddKeyword(keyword);
					}
				}
				else
				{
					EditableKeywordRecord editableKeywordRecordType =
						keywordRecordType.CreateEditableKeywordRecord();

					//Add Keyword groups to the Document Properties
					foreach (KeywordType keywordType in keywordRecordType.KeywordTypes)
					{
						string newKeywordValue = null;

						if (!keywords.TryGetValue(
								keywordType.Name,
								out newKeywordValue) &&
							string.IsNullOrWhiteSpace(newKeywordValue))
						{
							continue;
						}

						unfoundKeywords.Remove(keywordType.Name);

						Keyword keyword = _createKeywordAdapter.CreateKeyword(
							newKeywordValue,
							keywordType.Name,
							documentType.Name,
							false);

						editableKeywordRecordType.AddKeyword(keyword);
					}

					if (editableKeywordRecordType.Keywords.Count > 0)
					{
						newDocumentProperties.AddKeywordRecord(editableKeywordRecordType);
					}
				}
			}

			if (unfoundKeywords.Count > 0)
			{
				throw new ArgumentException(
					$"Unable to find keyword types '{string.Join(", ", unfoundKeywords)}' in OnBase.");
			}
		}

		private FileType GetFileTypeByExtension(string fileExtension)
		{
			_logger.LogInformation(
				$"Retrieving file type from OnBase API for '{fileExtension}' file type name.");

			FileType fileType = _applicationConnectionAdapter
				.Application
				.Core
				.FileTypes
				.Find(
					filetype => filetype.Name.Equals(
						_fileTypes
							.FirstOrDefault(
								value => value.Key.Equals(
									fileExtension.ToLowerInvariant(),
									StringComparison.InvariantCulture))
							.Value,
						StringComparison.CurrentCulture));

			return fileType;
		}
	}
}