// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   WC.Services.Document
//   SearchResult.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.Document.Models.v6
{
	using System.Collections.Generic;
	public class ExportDocumentResponse : IExportResult<DocumentHeader>
	{
		public IEnumerable<DocumentHeader> Documents { get; set; }
		public int SuccessRecordCount { get; set; }
		public int ErrorRecordCount { get; set; }
	}
}