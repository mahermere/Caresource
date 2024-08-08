// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2022. All rights reserved.
// 
//   Workview
//   CreateRequest.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.WorkView.Models.v5
{
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;

	public class CreateRequest : ContextHeader
	{
		[Required]
		public IEnumerable<WorkViewBaseObject> Data { get; set; }
	}
}