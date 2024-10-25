// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   WC.Services.Document
//   SearchResult.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.Document.Dotnet8.Models.v6.Responses
{
	using System.Collections.Generic;
    using WC.Services.Document.Dotnet8.Models.v6.Interfaces;

    public class ExportDocumentResponse : IExportResult<DocumentHeader>
	{
		public IEnumerable<DocumentHeader> Documents { get; set; }
		public int SuccessRecordCount { get; set; }
		public int ErrorRecordCount { get; set; }
	}
}