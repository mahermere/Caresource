// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   Entities
//   ListDocumentsRequest.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Entities.Documents
{
	using System.Collections.Generic;
	using CareSource.WC.Entities.Common;
	using CareSource.WC.Entities.Documents.Interfaces;
	using CareSource.WC.Entities.Requests.Base;

	/// <summary>
	///    Data and functions describing a CareSource.WC.Entities.Documents.ListDocumentsRequest object.
	/// </summary>
	/// <seealso cref="CareSource.WC.Entities.Requests.Base.PagedSortedRequest" />
	/// <seealso cref="CareSource.WC.Entities.Documents.Interfaces.IListDocumentsRequest" />
	public class ListDocumentsRequest : PagedSortedRequest, IListDocumentsRequest
	{
		/// <summary>
		///    Gets or sets the List Documents Request Document Types
		/// </summary>
		public IEnumerable<string> DocumentTypes { get; set; }
			= new HashSet<string>();

		/// <summary>
		///    Gets or sets the List Documents Request Filters
		/// </summary>
		public IEnumerable<Filter> Filters { get; set; }
			= new HashSet<Filter>();

		/// <summary>
		///    Gets or sets the List Documents Request Start Date
		/// </summary>
		public string StartDate { get; set; }

		/// <summary>
		///    Gets or sets the List Documents Request End Date
		/// </summary>
		public string EndDate { get; set; }

		/// <summary>
		///    Gets or sets the List Documents Request Display Columns
		/// </summary>
		public IEnumerable<string> DisplayColumns { get; set; }
			= new HashSet<string>();
	}
}