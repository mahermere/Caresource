// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2021. All rights reserved.
// 
//   ImportProcessor
//   DocumentAdapter.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace ImportProcessor.Adapters.v1
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Text.RegularExpressions;
	using System.Threading;
	using CareSource.WC.OnBase.Core.Configuration.Interfaces;
	using CareSource.WC.OnBase.Core.Connection.Interfaces;
	using ImportProcessor.Adapters.v1.Interfaces;
	using ImportProcessor.Models.v1;
	using Document = Models.v1.Document;
	using Hyland.Unity;
	using CareSource.WC.Entities.Documents;
	using CareSource.WC.Entities.Exceptions;
	using CareSource.WC.OnBase.Core.ExtensionMethods;
	using Microsoft.Extensions.Logging;

	//using CareSource.WC.Core.Configuration.Interfaces;
	public class DocumentAdapter : IDocumentAdapter
	{
		private readonly ILogger _logger;
		private readonly ISettingsAdapter _settingsAdapter;
		private readonly IFileAdapter _fileAdapter;
		private readonly IApplicationConnectionAdapter<Application> _hylandApplication;
		private readonly IKeywordAdapter<Hyland.Unity.Keyword> _KeywordAdapter;

		private const string ImageFileFormat = "Image File Format";
		private const string MsExcelSpreadsheet = "MS Excel Spreadsheet";
		private const string MsPowerPoint = "MS Power Point";
		private const string MsWordDocument = "MS Word Document";
		private const string Pdf = "PDF";
		private const string RichTextFormat = "Rich Text Format";
		private const string TextReportFormat = "Text Report Format";
		private const string WavAudioFile = "WAV Audio File";
		private const string ZipCompressionArchive = "Zip Compression Archive";
		private const string Html = "HTML";

		protected const string GUID = "GUID";

		private IDictionary<string, string> keywords;
		
		private IEnumerable<Hyland.Unity.Document> Documents { get; set; }

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

		public DocumentAdapter(
			IKeywordAdapter<Hyland.Unity.Keyword> KeywordAdapter,
			IApplicationConnectionAdapter<Application> applicationConnectionAdapter,
			ILogger logger,
			ISettingsAdapter settingsAdapter,
			IFileAdapter fileAdapter)
		{
			_KeywordAdapter = KeywordAdapter;
			_hylandApplication = applicationConnectionAdapter;
			_logger = logger;
			_settingsAdapter = settingsAdapter;
			_fileAdapter = fileAdapter;
		}

		public long CreateDocument(
			Document document,
			string cpsProcessingDirectory,
			Guid correlationGuid,
			string filename,
			string documentTypeName)
		{
			string uploadFileName = document.document.fileName;
			string sourceFileName = $"{cpsProcessingDirectory}\\{uploadFileName}";

			if (!File.Exists(sourceFileName))
			{
				_logger.LogError($"Attachment file not found: {sourceFileName}");

				throw new FileNotFoundException($"Attachment file not found: {sourceFileName}");
			}

			_logger.LogDebug($"Creating Document: {sourceFileName}");

			List<Stream> documentPages = new List<Stream>();

			Stream fileStream = _fileAdapter
				.ReadFileContents(sourceFileName);
			documentPages = new List<Stream> { fileStream };

			if (string.IsNullOrWhiteSpace(documentTypeName))
			{
				throw new ArgumentException(
					"Must have Document Type object when adding add a new document to OnBase.");
			}

			DocumentTypeList documentTypeList = _hylandApplication
				.Application
				.Core
				.DocumentTypes;

			Hyland.Unity.DocumentType obDocumentType = documentTypeList.Find(documentTypeName);

			if (obDocumentType == null)
			{
				throw new ArgumentException(
					$"Could not find document type '{documentTypeName}' in OnBase.");
			}

			_logger.LogDebug(
				$"Creating document storage properties for '{obDocumentType}' document type name " +
				$"and '{obDocumentType.DefaultFileType.Name}' file type name.");

			string fileTypeExtension = obDocumentType.DefaultFileType.Extension;

			if (!string.IsNullOrEmpty(sourceFileName))
			{
				Match fileExtensionSearch = Regex.Match(
					sourceFileName,
					@"\.([A-z,0-9]+)$");

				if (!fileExtensionSearch.Success &&
					fileExtensionSearch.Groups[1]
						.Success)
				{
					throw new ArgumentException(
						$"Unable to find file extension from file name '{filename}'.");
				}

				_logger.LogDebug(
					$"Overriding default file extension '{fileTypeExtension}' " +
					$"with found file extension '{fileExtensionSearch.Groups[1].Value}' " +
					$"from file name '{sourceFileName}'.");

				fileTypeExtension = fileExtensionSearch.Groups[1]
					.Value;
			}

			FileType fileType = GetFileTypeByExtension(fileTypeExtension) ??
				obDocumentType.DefaultFileType;

			StoreNewDocumentProperties newDocumentProperties = _hylandApplication
				.Application
				.Core
				.Storage
				.CreateStoreNewDocumentProperties(
					obDocumentType,
					fileType);

			keywords = document.document.keywords.ToDictionary(
				keyword => keyword.Name,
				keyword => keyword.Value.Trim());

			keywords.Add("Original Filename",
				filename);

			AddKeywordRecordsToDocumentProperties(
				obDocumentType,
				keywords,
				newDocumentProperties);
			
			IEnumerable<PageData> pages = documentPages
				.Select(
					p => _hylandApplication
						.Application
						.Core
						.Storage
						.CreatePageData(
							p,
							fileTypeExtension));

			_logger.LogDebug(
				$"Saving document for '{obDocumentType.Name}' " +
				"document type name through Hyland Unity API.");

			Hyland.Unity.Document doc = _hylandApplication
				.Application
				.Core
				.Storage
				.StoreNewDocument(
					pages,
					newDocumentProperties);

			fileStream.Close();

			return doc.ID;
		}


		private FileType GetFileTypeByExtension(string fileExtension)
		{
			_logger.LogDebug(
				$"Retrieving file type from OnBase API for '{fileExtension}' file type name.");

			FileType fileType = _hylandApplication
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


		private void AddKeywordRecordsToDocumentProperties(
			Hyland.Unity.DocumentType documentType,
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

						Hyland.Unity.Keyword keyword = _KeywordAdapter.CreateKeyword(
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

						Hyland.Unity.Keyword keyword = _KeywordAdapter.CreateKeyword(
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


		public string DocumentType(long documentTypeId)
		{
			Hyland.Unity.DocumentType documentType = _hylandApplication.Application.Core.DocumentTypes
				.FirstOrDefault(d => d.ID.Equals(documentTypeId));

			if (documentType != null)
			{
				return documentType.Name;
			}

			_logger.LogError($"Document Type name not found for Document Type Id:{documentTypeId}");

			throw new InvalidDataException(
				$"Document Type name not found for Document Type Id :{documentTypeId}");

		}


		public IEnumerable<DocumentHeader> SearchDocuments(
			IEnumerable<string> guids,
			string docTypeName)
		{
			DocumentQuery documentQuery = _hylandApplication.Application.Core.CreateDocumentQuery();

			documentQuery.RetrievalOptions = DocumentRetrievalOptions.None;

			AddKeywords(
				guids,
				documentQuery);

			AddDisplayColumn(
				GUID,
				documentQuery);

			AddDocumentType(
				docTypeName,
				documentQuery);

			QueryResult queryResults = documentQuery.ExecuteQueryResults(10000);
			List<QueryResultItem> items = queryResults.QueryResultItems.ToList();

			items = items.GroupBy(doc => doc.Document.ID)
				.Select(doc => doc.First())
				.ToList();

			return
				items.Select(
					item => new DocumentHeader()
					{
						DocumentName = item.Document.Name,
						DocumentId = item.Document.ID,
						DocumentDate = item.Document.DocumentDate,
						DocumentType = item.Document.DocumentType.Name,
						DisplayColumns = new Dictionary<string, object>
						{
							{
								GUID, GetKeyWordValue(
									item,
									GUID,
									string.Empty)
							}
						}
					});
		}

		private void AddDisplayColumn(
			string keywordName,
			DocumentQuery documentQuery)
			=> documentQuery.AddDisplayColumn(GetKeywordType(keywordName));


		private void AddKeywords(
			IEnumerable<string> guids,
			DocumentQuery documentQuery)
		{
			foreach (string guid in guids)
			{
				KeywordType keyword = GetKeywordType(GUID);

				if (guid.IsNullOrWhiteSpace())
				{
					continue;
				}

				documentQuery.AddKeyword(
					keyword.ID,
					guid,
					KeywordOperator.Equal,
					KeywordRelation.Or);
			}
		}
		
		private KeywordType GetKeywordType(
			string filterName)
		{
			KeywordType keyword =
				_hylandApplication.Application.Core.KeywordTypes
					.FirstOrDefault(
						krt => krt.Name.Equals(
							filterName,
							StringComparison.InvariantCulture));

			if (keyword == null)
			{
				throw new InvalidKeywordException(filterName);
			}

			return keyword;
		}

		private void AddDocumentType(
			string documentType,
			DocumentQuery documentQuery)
		{
			Hyland.Unity.DocumentType dt = _hylandApplication.Application.Core.DocumentTypes
				.FirstOrDefault(
					doctype =>
						doctype.Name.Equals(
							documentType,
							StringComparison.InvariantCultureIgnoreCase));

			documentQuery.AddDocumentType(dt);
		}

		private string GetKeyWordValue(
			QueryResultItem item,
			string columnName,
			string defaultValue)
		{
			Hyland.Unity.Keyword column = null;

			foreach (KeywordRecord kr in item.Document.KeywordRecords)
			{
				foreach (Hyland.Unity.Keyword k in kr.Keywords)
				{
					if (k.KeywordType.Name.Equals(
						columnName,
						StringComparison.InvariantCulture))
					{
						column = k;

						break;
					}
				}

				if (column != null) continue;
			}

			if (column == null ||
				column.IsBlank)
			{
				return defaultValue;
			}

			return column.AlphaNumericValue;
			
		}
		
		public IEnumerable<string> UpdateKeywords(
			IEnumerable<KeywordUpdate> updates)
		{

			//List<long> successfulIds = new List<long>();
			List<string> errors = new List<string>();

			foreach (Hyland.Unity.Document document in Documents)
			{
				try
				{
					KeywordModifier keywordModifier = document.CreateKeywordModifier();

					foreach (KeyValuePair<string, string> keyword in updates
						.FirstOrDefault(d => d.DocumentId == document.ID)
						.Keywords)
					{

						KeywordType keywordType =
							_hylandApplication.Application.Core.KeywordTypes.FirstOrDefault(
								kwt => kwt.Name.Equals(
									keyword.Key,
									StringComparison.InvariantCulture));

						Hyland.Unity.Keyword newKeyword = MapValueToKeyword(
							keywordType,
							keyword.Value);

						_logger.LogDebug(
							$"Created new {newKeyword.KeywordType.Name} value {newKeyword.Value}");

						Hyland.Unity.Keyword currentKeyword = document.KeywordRecords.Find(keywordType)
							?.Keywords.Find(keywordType);

						keywordModifier.UpdateKeyword(
							currentKeyword,
							newKeyword);
					}

					const int maxAttempts = 3;
					int lockAttempt = 1;
					do
					{
						using (DocumentLock documentLock = document.LockDocument())
						{
							if (documentLock.Status != DocumentLockStatus.LockObtained)
							{
								_logger.LogDebug(
									"Unable to obtain Exclusive lock to update document "
									+ $"[{document.ID}]; Attempt #{lockAttempt}");

								Thread.Sleep(
									int.Parse(
										System.Configuration.ConfigurationManager
											.AppSettings["OnBase.DocumentLock.Wait"]));
							}
							else
							{
								keywordModifier.ApplyChanges();
								//successfulIds.Add(document.ID);
								_logger.LogDebug(
									$"Updated document [{document.ID}] on attempt #{lockAttempt}");

								documentLock.Release();
								break;
							}
						}
					} while (lockAttempt++ < maxAttempts);

					if (lockAttempt > maxAttempts)
					{
						_logger.LogError(
							$"Unable to obtain Exclusive lock to update document [{document.ID}]");
						errors.Add($"Unable to obtain Exclusive lock to update document [{document.ID}]");
					}
				}
				catch (InvalidCastException invalidCastException)
				{
					_logger.LogError(
						invalidCastException,
						invalidCastException.Message);

					errors.Add($"{document.ID} {invalidCastException.Message}");
					continue;
				}
				catch (Exception e)
				{
					_logger.LogError(e,
						e.Message);
					throw;
				}
			}

			return errors;
		}

		public (bool isValid, IEnumerable<long> badIds) ValidateDocumentIds(IEnumerable<long> documentIds)
		{
			List<long> errors = new List<long>();

			List<Hyland.Unity.Document> documents = new List<Hyland.Unity.Document>();

			foreach (long id in documentIds)
			{
				Hyland.Unity.Document doc = _hylandApplication.Application.Core.GetDocumentByID(id);

				if (doc == null)
				{
					errors.Add(id);
					_logger.LogInformation($"No Document found for Id: [{id}].");
					continue;
				}

				documents.Add(doc);
			}

			Documents = documents.AsEnumerable();
			return (!errors.Any(), errors);
		}

		private Hyland.Unity.Keyword MapValueToKeyword(
			KeywordType keywordType,
			string keywordValue)
		{
			if (!keywordValue.IsNullOrWhiteSpace())
			{
				switch (keywordType.DataType)
				{
					case KeywordDataType.Numeric20:
					case KeywordDataType.SpecificCurrency:
					case KeywordDataType.Currency:
						decimal? decimalValue = keywordValue
							.Replace("$", string.Empty)
							.Replace("%", string.Empty)
							.ToSafeDecimal();

						if (decimalValue.HasValue)
						{
							return keywordType.CreateKeyword(decimalValue.Value);
						}

						break;

					case KeywordDataType.Date:
					case KeywordDataType.DateTime:
						DateTime? dateValue = keywordValue.ToSafeDateTime();

						if (dateValue.HasValue)
						{
							return keywordType.CreateKeyword(dateValue.Value);
						}

						break;
					case KeywordDataType.FloatingPoint:
						double? doubleValue = keywordValue
							.Replace("$", string.Empty)
							.Replace("%", string.Empty)
							.ToSafeDouble();

						if (doubleValue.HasValue)
						{
							return keywordType.CreateKeyword(doubleValue.Value);
						}

						break;
					case KeywordDataType.Numeric9:
						long? longValue = keywordValue
							.Replace("$", string.Empty)
							.Replace("%", string.Empty)
							.ToSafeLong();

						if (longValue.HasValue)
						{
							return keywordType.CreateKeyword(longValue.Value);
						}

						break;
					case KeywordDataType.AlphaNumeric:
					case KeywordDataType.Undefined:
					default:
						return keywordType.CreateKeyword(keywordValue);
				}

				throw new InvalidCastException(
					$"Keyword value of {keywordValue} cannot be converted "
					+ $"to {keywordType.DataType.ToString()}");
			}

			return keywordType.CreateKeyword(null);
		}
	}
}
