// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   WC.Services.Document
//   SearchRequest.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.Document.Dotnet8.Models.v6.Requests
{
	using System;
	using System.Collections.Generic;
	using CareSource.WC.Entities.Common;
	using CareSource.WC.Entities.Requests.Base;
    using WC.Services.Document.Dotnet8.Models.v6.Interfaces;

    /// <summary>
    ///    Data and functions describing a CareSource.WC.Services.Document.Models.v6.SearchRequest object.
    /// </summary>
    /// <remarks>
    ///    Basic request to search for Documents
    /// </remarks>
    /// <seealso cref="CareSource.WC.Services.Document.Models.v6.ISearchRequest" />
    public class SearchRequest : Request, ISearchRequest
	{
		/// <summary>
		/// Gets or sets the Search Request Display Columns
		/// </summary>
		public IEnumerable<string> DisplayColumns { get; set; }

		/// <summary>
		/// Gets or sets the Search Request Document Types
		/// </summary>
		public IEnumerable<string> DocumentTypes { get; set; }

		/// <summary>
		/// Gets or sets the Search Request Filters
		/// </summary>
		public IEnumerable<Filter> Filters { get; set; }

		/// <summary>
		/// Gets or sets the Search Request Start Date
		/// </summary>
		public DateTime? StartDate { get; set; }

		/// <summary>
		/// Gets or sets the Search Request End Date
		/// </summary>
		public DateTime? EndDate { get; set; }

		/// <summary>
		/// Gets or sets the Search Request Paging
		/// </summary>
		/// <remarks>
		///	Determines the paging for the response, defaults Page number = 1, page size = 100
		/// </remarks>
		public Paging Paging { get; set; } = new Paging()
		{
			PageNumber = 1,
			PageSize = 100
		};

		/// <summary>
		/// Gets or sets the Search Request Sort
		/// </summary>
		/// <remarks>
		/// The Sort of the search, can be one column and defaults to DocumentDate Descending,
		/// most recent on top
		/// </remarks>
		public Sort Sorting { get; set; } = new Sort()
		{
			ColumnName = "DocumentDate",
			SortAscending = false
		};
	}
}