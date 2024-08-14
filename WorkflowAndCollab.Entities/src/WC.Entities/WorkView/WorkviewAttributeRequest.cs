// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   Entities
//   WorkviewAttributeRequest.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Entities.WorkView
{
	using System.ComponentModel.DataAnnotations;

	public class WorkviewAttributeRequest
	{
		[Required]
		public string Name { get; set; }

		[Required]
		public string Value { get; set; }
	}
}