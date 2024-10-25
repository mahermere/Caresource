// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   WC.Services.Document
//   ISearchRequest.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.Document.Dotnet8.Models.v6.Interfaces
{
	using System.Collections.Generic;
	using CareSource.WC.Entities.Requests.Base;

	/// <summary>
	/// Basic Information needed to search for documents
	/// </summary>
	public interface ISearchRequest: IFilteredRequest
	{
		/// <summary>
		/// Gets or sets the Search Request Display Columns
		/// </summary>
		IEnumerable<string> DisplayColumns { get; set; }

		/// <summary>
		/// Gets or sets the Search Request Paging
		/// </summary>
		Paging Paging { get; set; }

		/// <summary>
		/// Gets or sets the Search Request Sort
		/// </summary>
		Sort Sorting { get; set; }

	}
}