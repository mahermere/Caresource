// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   CareSource.WC.Entities.WC.Entities
//   SearchResults.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Entities.Common
{
	using System.Collections.Generic;
	using CareSource.WC.Entities.Common.Interfaces;

	public class SearchResults<TDataModel> : ISearchResults<TDataModel>
	{
		public IEnumerable<TDataModel> Results { get; set; }

		public int TotalRecordCount { get; set; }
	}
}