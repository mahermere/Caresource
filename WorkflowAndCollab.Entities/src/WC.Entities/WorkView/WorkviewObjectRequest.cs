// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   Entities
//   WorkviewObjectRequest.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Entities.WorkView
{
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;

	public class WorkviewObjectRequest
	{
		[Required]
		public string ApplicationName { get; set; }

		[Required]
		public string ClassName { get; set; }

		public List<WorkviewAttributeRequest> Attributes { get; set; }
	}
}