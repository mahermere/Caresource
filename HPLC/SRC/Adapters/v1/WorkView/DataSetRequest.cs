// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   WC.Services.Hplc
//   DataSetRequest.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.Hplc.Adapters.v1.WorkView
{
	using System.ComponentModel.DataAnnotations;

	/// <summary>
	///    Data and functions describing a CareSource.WC.Services.Hplc.Adapters.v1.WorkView.DataSetRequest
	///    object.
	/// </summary>
	/// <seealso cref="Request" />
	public class DataSetRequest : Request
	{
		/// <summary>
		///    Gets or sets the Data Set Request Data Set Name
		/// </summary>
		[Required]
		public string DataSetName { get; set; }
	}
}