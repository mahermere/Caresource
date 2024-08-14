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

	public class WorkviewFilterRequest
	{
		/// <summary>
		/// Gets or sets the Workview Filter Request Name
		/// </summary>
		[Required]
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the Workview Filter Request Value
		/// </summary>
		[Required]
		public string Value { get; set; }
	}
}