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
	public class ExportResult : SearchResult, IExportResult<DocumentHeader>
	{
		public int ErrorRecordCount { get; set; }
	}
}