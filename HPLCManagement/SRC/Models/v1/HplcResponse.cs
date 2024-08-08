// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   WC.Services.Hplc
//   HplcResponse.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace HplcManagement.Models.v1
{
	using System.Collections.Generic;

	/// <summary>
	///    Data and functions describing a CareSource.WC.Services.Hplc.Models.v1.HplcResponse object.
	/// </summary>
	public class HplcResponse
	{
		public Data Data { get; set; }

		/// <summary>
		///    Gets or sets the HPLC Response Filters
		/// </summary>
		public IEnumerable<Filter> Filters { get; set; }

	}
}