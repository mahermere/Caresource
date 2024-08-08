﻿// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   WC.Services.Document
//   ISearchResult.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.Document.Models.v6
{
	using System.Collections.Generic;

	public interface ISearchResult<TDataModel>
	{
		IEnumerable<TDataModel> Documents { get; set; }
		int SuccessRecordCount { get; set; }
	}
}