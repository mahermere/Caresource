﻿// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   WC.Services.Document
//   ISearchResult.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.Document.Dotnet8.Models.v6.Interfaces
{
	using System.Collections.Generic;

	public interface ISearchResult<TDataModel>
	{
		IEnumerable<TDataModel> Documents { get; set; }
		int SuccessRecordCount { get; set; }
	}
}