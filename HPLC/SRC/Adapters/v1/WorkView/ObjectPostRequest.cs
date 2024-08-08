// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   WC.Services.Hplc
//   ObjectPostRequest.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.Hplc.Adapters.v1.WorkView
{
	using System.Collections.Generic;
	using CareSource.WC.Entities.WorkView;

	/// <summary>
	///    Data and functions describing a
	///    CareSource.WC.Services.Hplc.Adapters.v1.WorkView.WorkViewObjectPostRequest object.
	/// </summary>
	public class ObjectPostRequest : Request, IRequest
	{
		/// <summary>
		///    Gets or sets the WorkView Object Post Request Attributes
		/// </summary>
		public IEnumerable<WorkviewAttributeRequest> Attributes { get; set; }

		/// <summary>
		///    Gets or sets the WorkView Object Post Request Object Identifier
		/// </summary>
		public long ObjectId { get; set; }
	}
}