// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   WC.Services.Hplc
//   ObjectBatchPostRequest.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.Hplc.Adapters.v1.WorkView
{
	using System.Collections.Generic;

	/// <summary>
	///    Data and functions describing a
	///    CareSource.WC.Services.Hplc.Adapters.v1.WorkView.ObjectBatchPostRequest object.
	/// </summary>
	public class ObjectBatchPostRequest
	{
		/// <summary>
		///    Gets or sets the Object Batch Post Request WorkView Objects
		/// </summary>
		public IEnumerable<ObjectPostRequest> WorkViewObjects { get; set; }
	}
}