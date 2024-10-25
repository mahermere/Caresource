// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2023. All rights reserved.
// 
//   WC.Services.Document
//   IExportDocumentsManager.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.Document.Dotnet8.Managers.v6.Interfaces
{
	using System.Collections.Generic;
	using WC.Services.Document.Dotnet8.Models.v6;
    using WC.Services.Document.Dotnet8.Models.v6.Interfaces;

    public interface IExportDocumentManager
	{
		IExportResult<DocumentHeader> ExportDocuments(IExportDocumentRequest request);

		ISearchResult<DocumentHeader> SearchDocuments(IExportDocumentRequest request);
	}
}