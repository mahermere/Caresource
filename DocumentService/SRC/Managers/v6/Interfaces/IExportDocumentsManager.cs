// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2023. All rights reserved.
// 
//   WC.Services.Document
//   IExportDocumentsManager.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.Document.Managers.v6
{
	using System.Collections.Generic;
	using CareSource.WC.Services.Document.Models.v6;

	public interface IExportDocumentManager
	{
		IExportResult<DocumentHeader> ExportDocuments(IExportDocumentRequest request);

		ISearchResult<DocumentHeader> SearchDocuments(IExportDocumentRequest request);
	}
}