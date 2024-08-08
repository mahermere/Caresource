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
	using CareSource.WC.Entities.Documents;

	public class SearchResult : ISearchResult<DocumentHeader>
	{
		public IEnumerable<DocumentHeader> Documents { get; set; }
		public int SuccessRecordCount { get; set; }
	}
}