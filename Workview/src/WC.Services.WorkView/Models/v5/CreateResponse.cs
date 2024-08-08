// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2022. All rights reserved.
// 
//   Workview
//   CreateResponse.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.WorkView.Models.v5
{
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;

	public class CreateResponse : ContextHeader
	{
		[Required]
		public string ApplicationName { get; set; }

		[Required]
		public IEnumerable<WorkViewObject> Data { get; set; }
	}
}