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
    using WC.Services.Document.Dotnet8.Models.v6.Interfaces;

    /// <summary>
    ///    Data and functions describing a CareSource.WC.Services.Document.Models.v6.SearchRequest object.
    /// </summary>
    /// <remarks>
    ///    Basic request to search for Documents
    /// </remarks>
    /// <seealso cref="IFilteredRequest" />
    public class CountRequest : Request, IFilteredRequest
	{
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
	}
}