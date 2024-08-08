// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2022. All rights reserved.
// 
//   Workview
//   SearchRequest.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.WorkView.Models.v5
{
	using System.Collections.Generic;

	public class SearchResult : ContextHeader
	{
		public string ClassName { get; set; }

		public long TotalRecords { get; set; }

		public IEnumerable<WorkViewObject> Results { get; set; } = new List<WorkViewObject>();
	}
}