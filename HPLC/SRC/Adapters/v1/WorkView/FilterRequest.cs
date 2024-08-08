// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   WC.Services.Hplc
//   FilterRequest.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.Hplc.Adapters.v1.WorkView
{
	using System.ComponentModel.DataAnnotations;

	/// <summary>
	///    Data describing a CareSource.WC.Services.Hplc.Adapters.v1.WorkView.FilterRequest object.
	/// </summary>
	public class FilterRequest
	{
		/// <summary>
		///    Gets or sets the Filter Request Name
		/// </summary>
		[Required]
		public string Name { get; set; }

		/// <summary>
		///    Gets or sets the Filter Request Value
		/// </summary>
		[Required]
		public string Value { get; set; }
	}
}