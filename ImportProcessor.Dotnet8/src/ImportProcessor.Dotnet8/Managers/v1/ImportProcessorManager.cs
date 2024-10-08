// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2022. All rights reserved.
// 
//   ImportProcessor
//   ImportProcessorManager.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.ImportProcessor.Dotnet8.Managers.v1
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.IO.Compression;
	using System.Linq;
	using CareSource.WC.Entities.Documents;
	using CareSource.WC.Entities.Exceptions;
	using CareSource.WC.Entities.Workview.v2;
	//using CareSource.WC.OnBase.Core.Configuration.Interfaces;
	//using CareSource.WC.OnBase.Core.ExtensionMethods;
    using WC.Services.ImportProcessor.Dotnet8.Adapters.v1.Interfaces;
    using WC.Services.ImportProcessor.Dotnet8.Managers.v1.Interfaces;
    using WC.Services.ImportProcessor.Dotnet8.Models.v1;
	using Microsoft.Extensions.Logging;
	using Newtonsoft.Json;
    using Document = WC.Services.ImportProcessor.Dotnet8.Models.v1.Document;

	public class ImportProcessorManager : IImportProcessorManager<ImportProcessorResponse>
	{
		public const string RNL = "RNL";
		public const string CBRL = "CBRL";
		public const string EOB = "EOB";
		public const string COBRNL = "COB RNL";

		protected const string RnlLetterNumber = "RNL Letter Number";
		protected const string CBRLLetterNumber = "CBRL Letter Number";
		protected const string ProviderId = "Provider ID";
		protected const string DoctypeIndEob = "EOB";
		protected const string DoctypeIndRnl = "RNL";
		protected const string DoctypeIndCobRnl = "COB RNL";
		protected const string GUID = "GUID";

		private readonly IDocumentAdapter _docadapter;
		private readonly log4net.ILog _logger;
		private readonly IConfiguration _configuration;
		private readonly IWorkViewAdapter _wvadapter;

		public ImportProcessorManager(
			IWorkViewAdapter wvAdapter,
			IDocumentAdapter docAdapter,
			log4net.ILog logger,
			IConfiguration configuration)
		{
			_wvadapter = wvAdapter;
			_docadapter = docAdapter;
			_logger = logger;
			_configuration = configuration;
		}

		public ImportProcessorResponse CreateOnBaseObjects(
			string documentTypeIndicator,
			Guid correlationGuid)
		{
			long docObjectId = 0;
			long claimsCount = 0;
			string cpsDirectory = RNL;

			switch (documentTypeIndicator)
			{
				case RNL:
					cpsDirectory = _configuration["ImportSettings:CPSDirectoryLocation"];
					break;

				case CBRL:
					cpsDirectory = _configuration["ImportSettings:CBRLDirectoryLocation"];
					break;

				case EOB:
					cpsDirectory = _configuration["ImportSettings:EOBDirectoryLocation"];
					break;

				case COBRNL:
					cpsDirectory = _configuration["ImportSettings:COBRNLDirectoryLocation"];
					break;
			}

			string processingLocation = $"{cpsDirectory}\\Processing";

			//if (Directory.GetFiles(processingLocation)
			//		.Length >
			//	0)
			//{
			//	_logger.Info("Claims Service is currently busy processing a batch.");

			//	return new ImportProcessorResponse
			//	{
			//		errorCode = ErrorCode.Success,
			//		message = "Busy",
			//		correlationGuid = correlationGuid
			//	};
			//}

			DirectoryInfo di = new DirectoryInfo(cpsDirectory);

			// Pick up the files in order by date created (last to first)
			string[] zipFileNames = di.GetFiles(
					"*.zip",
					SearchOption.TopDirectoryOnly)
				.OrderBy(f => f.LastWriteTime)
				.Select(s => s.FullName)
				.ToArray();

			// Go through each zip file in the share.
			foreach (string zipFileName in zipFileNames)
			{
				if (!File.Exists(zipFileName))
				{
					continue;
				}

				bool updateKeywords = false;

				_logger.Debug($"Backing up zip file: {zipFileName}");
				CopyToBackupFolder(
					zipFileName,
					cpsDirectory);

				string filenameWoExt = Path.GetFileNameWithoutExtension(zipFileName);
				string filenameWithExt = Path.GetFileName(zipFileName);
				string destinationFolder = $"{processingLocation}\\{filenameWoExt}";

				if (Directory.Exists(destinationFolder))
				{
					_logger.Debug($"Deleting Existing Folder: {destinationFolder}");
					DeleteFolder(destinationFolder);
				}

				File.Move(
					zipFileName,
					$"{processingLocation}\\{filenameWithExt}");

				_logger.Debug($"Extracting file to: {destinationFolder}");
				ZipFile.ExtractToDirectory(
					$"{processingLocation}\\{filenameWithExt}",
					destinationFolder);

				long pdfFileCount = (
					from file in Directory.EnumerateFiles(
						destinationFolder,
						"*.pdf")
					select file).Count();

				long jsonFileCount = (
					from file in Directory.EnumerateFiles(
						destinationFolder,
						"*.json")
					select file).Count();

				// Check for 1 JSON file
				if (jsonFileCount != 1)
				{
					CleanupFolders(
						destinationFolder,
						cpsDirectory,
						processingLocation,
						zipFileName,
						"Error");

					_logger.Error($"Json file missing in zip file:{zipFileName}");

					continue;
				}

				// Deserialize
				string[] jsonFile = Directory.GetFiles(
					destinationFolder,
					"*.json");

				string jsonFileName = jsonFile.GetValue(0)
					.ToString();

				string fileBytes = File.ReadAllText(jsonFileName);

				_logger.Debug($"Deserializing Json file: {jsonFileName}");

				CpsManifest cpsImportData = JsonConvert.DeserializeObject<CpsManifest>(fileBytes);

				if (cpsImportData != null)
				{
					int jsonDocsCount = cpsImportData.documents.Count();
					_logger.Debug("Total Number of Documents (JSON): {jsonDocsCount}; (PDF): {pdfFileCount}.");

					// Check if any PDFs exist, if not, then must be an update to
					// keyword(s) only.

					// Make sure the number of physical files matches what's in the json file.
					// If they don't match, don't process it.

					if (pdfFileCount == 0)
					{
						updateKeywords = true;
					}
				}
				else
				{
					CleanupFolders(
						cpsDirectory,
						destinationFolder,
						processingLocation,
						zipFileName,
						"Error");

					_logger.Error($"Error deserializing the JSON file :{jsonFileName}");

					continue;
				}

				_logger.Debug(
					$"Successful import of {jsonFileName}.");

				// Get the documment type
				string docTypeName = _docadapter.DocumentType(
					Convert.ToInt64(
						cpsImportData.documents.FirstOrDefault()
							?.document.documentTypeNumber));

				_logger.Info($"Document Type Name: {docTypeName}");

				// Check to Update Keywords only (EOB).
				if (documentTypeIndicator == EOB && updateKeywords)
				{
					try
					{
						string errorMessage = string.Empty;

						// Load a list of filters with GUID data.
						IEnumerable<string> guids = cpsImportData.documents.Select(
							document => document.document.keywords
								.FirstOrDefault(attribute => attribute.Name.Equals(GUID))
								?.Value.ToString());

						int guidCount = 10;
						int guidPage = 0;

						IEnumerable<string> failures = new List<string>();
						List<string> totalFailures = new List<string>();
						List<DocumentHeader> searchResponse = new List<DocumentHeader>();
						List<KeywordUpdate> keywordUpdates = new List<KeywordUpdate>();

						do
						{
							searchResponse.AddRange(
								_docadapter.SearchDocuments(
									guids.Skip(guidPage++ * guidCount)
										.Take(guidCount),
									docTypeName));

							if (searchResponse.Count == 0)
							{
								_logger.Error("Error in Document/Search call.");

								throw new InvalidDataException(
									errorMessage);
							}

							keywordUpdates.AddRange(
								from dh in searchResponse
								let guid = dh.DisplayColumns.FirstOrDefault(dc => dc.Key.Equals(GUID))
									.Value.ToString()
								select new KeywordUpdate
								{
									Keywords = new Dictionary<string, string>
									{
										{
											"Date Letter Mailed", cpsImportData.documents.FirstOrDefault(
													id => id.document.keywords
															.FirstOrDefault(dc => dc.Name.Equals(GUID))
															?.Value ==
														guid)
												?.document.keywords.FirstOrDefault(
													dc => dc.Name.Equals("Date Letter Mailed"))
												?.Value
										}
									},
									DocumentId = dh.DocumentId
								});

							_docadapter.ValidateDocumentIds(
								searchResponse.Select(
									sr => sr.DocumentId));

							failures = _docadapter.UpdateKeywords(
								keywordUpdates);

							totalFailures.AddRange(failures);
							searchResponse.Clear();
							keywordUpdates.Clear();
						} while (guidPage * guidCount < guids.Count());


						if (totalFailures.Any())
						{
							//failures.Sort();
							string path = $"{cpsDirectory}\\Error\\{filenameWoExt}_KW_Failures";
							errorMessage = "Error updating keyword for GUID: ";

							using (TextWriter tw = new StreamWriter(path))
							{
								foreach (string failure in totalFailures)
								{
									tw.WriteLine(errorMessage + failure);
								}
							}

							CleanupFolders(
								destinationFolder,
								cpsDirectory,
								processingLocation,
								zipFileName,
								"Error");

							_logger.Error("Error Updating Document Keywords.");
						}
						else
						{
							CleanupFolders(
								destinationFolder,
								cpsDirectory,
								processingLocation,
								zipFileName,
								"Done");
						}

						continue;
					}
					catch (Exception e)
					{
						CleanupFolders(
							destinationFolder,
							cpsDirectory,
							processingLocation,
							zipFileName,
							"Error");

						_logger.Error(e.Message);

						throw;
					}
				}

				// Otherwise, Process/Create the document in OnBase and objects in WV.
				foreach (Document document in cpsImportData.documents)
				{
					long documentId = 0;
					string errorMessage = string.Empty;

					try
					{
						// Create the OnBase document
						documentId = _docadapter.CreateDocument(
							document,
							destinationFolder,
							correlationGuid,
                            filenameWoExt,
							docTypeName);

						if (documentId <= 0)
						{
							CleanupFolders(
								destinationFolder,
								cpsDirectory,
								processingLocation,
								zipFileName,
								"Error");

							_logger.Error("Error creating OnBase document.");

							throw new InvalidDataException(
								errorMessage);
						}
					}
					catch (Exception e)
					{
						CleanupFolders(
							destinationFolder,
							cpsDirectory,
							processingLocation,
							zipFileName,
							"Error");

						_logger.Error("An unexpected error occurred. "+ e.Message);

						throw;
					}

					_logger.Debug(
						$"Created OnBase Document Successfully - Id: {documentId}.");

					// Create the Document object in Workview.
					try
					{
						string letterNumber = string.Empty;
						string providerId = string.Empty;

						if (documentTypeIndicator != DoctypeIndEob)
						{
							string letterNumberType = documentTypeIndicator == DoctypeIndRnl ||
								documentTypeIndicator == DoctypeIndCobRnl
									? RnlLetterNumber
									: CBRLLetterNumber;

							letterNumber =
								document.document.keywords
									.FirstOrDefault(attribute => attribute.Name.Equals(letterNumberType))
									?.Value.ToString();

							_logger.Debug($"Letter Number being processed: {letterNumber}");

							providerId =
								document.document.keywords.FirstOrDefault(
										attribute => attribute.Name.Equals(ProviderId))
									?.Value.ToString();

							if (String.IsNullOrWhiteSpace(letterNumber) ||
								String.IsNullOrWhiteSpace(providerId))
							{
								_logger.Error(
									$"Letter Number or Provider Id is null.  WV Documents object class not created for Doc Id: {documentId}");

								continue;
							}
						}

						List<WorkviewAttributeRequest> attributes = new List<WorkviewAttributeRequest>
						{
							new WorkviewAttributeRequest
							{
								Name = "DocumentId",
								Value = documentId.ToString()
							},
							new WorkviewAttributeRequest
							{
								Name = "DocumentType",
								Value = documentTypeIndicator
							},
							new WorkviewAttributeRequest
							{
								Name = "LetterNumber",
								Value = letterNumber
							},
							new WorkviewAttributeRequest
							{
								Name = "ProviderId",
								Value = providerId
							},
							new WorkviewAttributeRequest
							{
								Name = "OriginalFileName",
								Value = filenameWoExt
							}
						};

						// Create Documents object
						docObjectId = _wvadapter.CreateDocumentObject(
							attributes);

						if (docObjectId <= 0)
						{
							CleanupFolders(
								destinationFolder,
								cpsDirectory,
								processingLocation,
								zipFileName,
								"Error");

							_logger.Error(
								"Error creating Document Object in CreateDocumentObject call.");

							throw new InvalidDataException(
								"Error creating Document Object in CreateDocumentObject call.");
						}
					}
					catch (ArgumentNullException)
					{
						CopyFileTo(
							$"{cpsDirectory}\\{Path.GetFileName(zipFileName)}",
							$"{cpsDirectory}\\Error\\{Path.GetFileName(zipFileName)}");
					}
					catch (Exception e)
					{
						CleanupFolders(
							destinationFolder,
							cpsDirectory,
							processingLocation,
							zipFileName,
							"Error");

						_logger.Error("An unexpected error occurred creating the WV Document object." + e.Message);

						throw;
					}

					_logger.Debug(
						$"Created WV Documents Class Object Successfully - Id: {docObjectId}.");

					// Create the Claims object in Workview.
					try
					{
						IEnumerable<Claim> errorsClaims = document.document.claims.Where(
							c => c.attributes.Any(i => String.IsNullOrWhiteSpace(i.Value)));

						if (errorsClaims.Any())
						{
							CleanupFolders(
								destinationFolder,
								cpsDirectory,
								processingLocation,
								zipFileName,
								"Error");

							_logger.Error(
								$"Claim Number or Member Id is null. Document Id: {documentId}");

							throw new ArgumentNullException(nameof(docObjectId));
						}

						// Create the Claims objects.
						claimsCount = _wvadapter.CreateClaimsObjects(
							docObjectId,
							document.document.claims);

						if (claimsCount <= 0)
						{
							CleanupFolders(
								destinationFolder,
								cpsDirectory,
								processingLocation,
								zipFileName,
								"Error");

							errorMessage = "Invalid import to Claims class";

							_logger.Error(
								errorMessage);

							throw new InvalidDataException(
								errorMessage);
						}
					}
					catch (ArgumentNullException)
					{ }
					catch (Exception e)
					{
						CleanupFolders(
							destinationFolder,
							cpsDirectory,
							processingLocation,
							zipFileName,
							"Error");

						_logger.Error("An unexpected error occurred when creating the WV claims object. "+e.Message);

						throw;
					}

					_logger.Debug(
						$"Created Workview Claims Class Objects Successfully - Count: {claimsCount}.");
				}

				CleanupFolders(
					destinationFolder,
					cpsDirectory,
					processingLocation,
					zipFileName,
					"Done");
			}

			return new ImportProcessorResponse
			{
				errorCode = ErrorCode.Success,
				message = "Success",
				correlationGuid = correlationGuid
			};
		}

		~ImportProcessorManager()
			=> _wvadapter.Dispose();

		// Private methods
		// ************************************************

		private static void CleanupFolders(
			string destinationFolder,
			string cpsDirectory,
			string processingLocation,
			string filename,
			string targetFolder)
		{
			DeleteFolder(destinationFolder);

			if (File.Exists($"{cpsDirectory}\\{targetFolder}\\{Path.GetFileName(filename)}"))
			{
				File.Delete($"{cpsDirectory}\\{targetFolder}\\{Path.GetFileName(filename)}");
			}

			CopyFileTo(
				$"{processingLocation}\\{Path.GetFileName(filename)}",
				$"{cpsDirectory}\\{targetFolder}\\{Path.GetFileName(filename)}");

			File.Delete($"{processingLocation}\\{Path.GetFileName(filename)}");
		}

		private static void DeleteFolder(string destinationFolder)
		{
			string[] filePaths = Directory.GetFiles(destinationFolder);

			foreach (string filePath in filePaths)
			{
				File.Delete(filePath);
			}

			Directory.Delete(destinationFolder);
		}

		private static void CopyFileTo(
			string file,
			string destinationFolder)
		{
			if (File.Exists(file))
			{
				File.Copy(
					file,
					destinationFolder);
			}
		}

		private static void CopyToBackupFolder(
			string fileName,
			string cpsDirectory)
		{
			string destination = cpsDirectory + "\\Backup\\" + Path.GetFileName(fileName);

			if (!File.Exists(destination))
			{
				File.Copy(
					fileName,
					destination);
			}
		}
	}
}