// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2022. All rights reserved.
// 
//   Workview
//   Filter.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.WorkView.Models.v5
{
	using System.ComponentModel.DataAnnotations;

	public class Filter
	{
		[Required]
		public string Name { get; set; }

		[Required]
		public string Value { get; set; }
	}
}