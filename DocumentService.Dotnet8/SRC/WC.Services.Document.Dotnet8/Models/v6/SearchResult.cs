// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   WC.Services.Document
//   SearchResult.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.Document.Dotnet8.Models.v6
{
	using System.Collections.Generic;
	using CareSource.WC.Entities.Documents;
    using WC.Services.Document.Dotnet8.Models.v6.Interfaces;

    public class SearchResult : ISearchResult<DocumentHeader>
	{
		public IEnumerable<DocumentHeader> Documents { get; set; }
		public int SuccessRecordCount { get; set; }
	}
}