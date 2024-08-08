// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2021. All rights reserved.
// 
//   WC.Services.Hplc
//   SearchRequest.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace HplcManagement.Models.v1
{
	using System.Collections.Generic;
	using CareSource.WC.Entities.Requests.Base;

	/// <summary>
	/// Data and functions describing a CareSource.WC.Services.Hplc.Models.v1.SearchRequest object.
	/// </summary>
	public class SearchRequest : BaseRequest
	{
		/// <summary>
		/// Gets or sets the Search Request Filters
		/// </summary>
		public IEnumerable<Filter> Filters { get; set; } = new List<Filter>();

		/// <summary>
		/// Gets or sets the Search Request Attributes
		/// </summary>
		public IEnumerable<Attribute> Attributes { get; set; } = new List<Attribute>();
	}
}