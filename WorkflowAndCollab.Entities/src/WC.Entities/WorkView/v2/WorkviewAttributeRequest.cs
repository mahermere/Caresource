// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   Entities
//   WorkviewAttributeRequest.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Entities.Workview.v2
{
	using System.ComponentModel.DataAnnotations;

	/// <summary>
	/// Data and functions describing a CareSource.WC.Entities.Workview.v2.WorkviewAttributeRequest object.
	/// </summary>
	public class WorkviewAttributeRequest
	{
		/// <summary>
		/// Gets or sets the Workview Attribute Request Name
		/// </summary>
		[Required]
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the Workview Attribute Request Value
		/// </summary>
		[Required]
		public string Value { get; set; }
	}
}