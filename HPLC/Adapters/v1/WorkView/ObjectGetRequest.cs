// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   WC.Services.Hplc
//   ObjectGetRequest.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.Hplc.Adapters.v1.WorkView
{
	using System.Collections.Generic;

	/// <summary>
	///    Data and functions describing a
	///    CareSource.WC.Services.Hplc.Adapters.v1.WorkView.ObjectGetRequest object.
	/// </summary>
	/// <seealso cref="Request" />
	/// <seealso cref="IObjectGetRequest" />
	public class ObjectGetRequest : Request, IObjectGetRequest
	{
		/// <summary>
		///    Gets or sets the Object Get Request Object Identifier
		/// </summary>
		public long ObjectId { get; set; }

		/// <summary>
		///    Gets or sets the Object Get Request Filters
		/// </summary>
		public IEnumerable<FilterRequest> Filters { get; set; }

		/// <summary>
		///    Gets or sets the Object Get Request Attributes
		/// </summary>
		public IEnumerable<string> Attributes { get; set; }
	}
}