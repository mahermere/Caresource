// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   Entities
//   PagingRequest.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Entities.Requests.Base
{
	using CareSource.WC.Entities.Requests.Base.Interfaces;

	/// <summary>
	/// Data and functions describing a CareSource.WC.Entities.Requests.Base.PagedSortedRequest object.
	/// </summary>
	/// <seealso cref="CareSource.WC.Entities.Requests.Base.Interfaces.PagingRequest" />
	/// <seealso cref="CareSource.WC.Entities.Requests.Base.Interfaces.ISortingRequest" />
	public abstract class PagedSortedRequest : PagingRequest, ISortingRequest
	{
		/// <summary>
		/// Gets or sets the Paged Sorted Request Sorting
		/// </summary>
		public Sort Sorting { get; set; }
	}
}