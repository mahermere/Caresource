// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   WC.Services.Document
//   SearchResult.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

using WC.Services.Document.Dotnet8.Models.v6.Interfaces;

namespace WC.Services.Document.Dotnet8.Models.v6
{
	public class ExportResult : SearchResult, IExportResult<DocumentHeader>
	{
		public int ErrorRecordCount { get; set; }
	}
}