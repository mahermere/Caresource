// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   CareSource.WC.Entities.WC.Entities
//   ISearchResults.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Entities.Common.Interfaces
{
	using System.Collections.Generic;

	public interface ISearchResults<TDataModel>
	{
		IEnumerable<TDataModel> Results { get; set; }
		int TotalRecordCount { get; set; }
	}
}