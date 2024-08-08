// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2023. All rights reserved.
// 
//   WC.Services.Document
//   ExportDocumentsManager.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.Document.Managers.v6
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Net;
	using CareSource.WC.Entities.Exceptions;
	using CareSource.WC.Entities.Responses;
	using CareSource.WC.Services.Document.Adapters.v6;
	using CareSource.WC.Services.Document.Models.v6;
	using Hyland.Unity.DocumentComposition;
	using Microsoft.Extensions.Logging;

	public class ExportDocumentsManager 
		: IExportDocumentManager
	{
		public ExportDocumentsManager(
			ILogger logger,
			ISqlAdapter sqlAdapter,
			IGetDocumentAdapter<OnBaseDocument> onBaseAdapter)
		{
			_logger = logger;
			_sqlAdapter = sqlAdapter;
			_getDocumentAdapter = onBaseAdapter;
		}

		private readonly ILogger _logger;
		private readonly ISqlAdapter _sqlAdapter;
		private readonly IGetDocumentAdapter<OnBaseDocument> _getDocumentAdapter;

		public IExportResult<DocumentHeader> ExportDocuments(IExportDocumentRequest request)
		{
			string methodName = $"{nameof(ExportDocumentsManager)}.{nameof(ExportDocuments)}";

			_logger.LogDebug($"Starting: {methodName}");

			ISearchResult<DocumentHeader> results = null;

			Directory.CreateDirectory(request.Path);

			int page = 1;
			int totalPages = 0;
			int count = 0;
			int errorCount = 0;

			List<DocumentHeader> documents = new List<DocumentHeader>();
			do
			{
				_logger.LogInformation(
					$"Export Request results, Page:{page}.",
					results);
				
				request.Paging.PageNumber = page;
				results = _sqlAdapter.SearchDocuments((ISearchRequest)request);

				totalPages = results.SuccessRecordCount / request.Paging.PageSize;

				
				foreach (DocumentHeader documentHeader in results.Documents)
				{
					try
					{
						// Get each document
						OnBaseDocument d = _getDocumentAdapter.GetDocument(documentHeader.DocumentId);

						string newPath = Path.Combine(
							request.Path,
							MakeValidFileName($"{d.Id}_{d.Filename}"));

						Hyland.Unity.Utility.WriteStreamToFile(
							d.FileData,
							newPath);

					}
					catch (Exception e)
					{
						_logger.LogError(
							e,
							e.Message);
						errorCount++;
					}

					count++;
				}

				documents.AddRange(results.Documents);

			} while (page++ - 1 <= totalPages);

			IExportResult<DocumentHeader> retval = new ExportResult()
			{
				Documents = documents,
				SuccessRecordCount = count - errorCount,
				ErrorRecordCount = errorCount
			};
			
			_logger.LogDebug($"Finished {methodName}");

			return retval;
		}

		public ISearchResult<DocumentHeader> SearchDocuments(IExportDocumentRequest request)
		{
			string methodName = $"{nameof(ExportDocumentsManager)}.{nameof(SearchDocuments)}";

			_logger.LogDebug($"Starting {methodName}");

			ISearchResult<DocumentHeader> results =
				_sqlAdapter.SearchDocuments((ISearchRequest)request);

			_logger.LogDebug($"Finished {methodName}");


			return results;
		}

		private static string MakeValidFileName(string name)
		{
			string invalidChars =
				System.Text.RegularExpressions.Regex.Escape(new string(Path.GetInvalidFileNameChars()));
			string invalidRegStr = string.Format(@"([{0}]*\.+$)|([{0}]+)", invalidChars);

			return System.Text.RegularExpressions.Regex.Replace(name, invalidRegStr, "_");
		}
	}
}