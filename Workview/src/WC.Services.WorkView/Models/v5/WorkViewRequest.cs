// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   Workview
//   WorkViewRequest.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.WorkView.Models.v5
{
	using System;
	using System.ComponentModel.DataAnnotations;

	public class WorkViewRequest : IWorkViewRequest
	{
		public Guid CorrelationGuid { get; set; } = Guid.NewGuid();

		public DateTime RequestDateTime { get; set; } = DateTime.Now;

		[Required]
		public string SourceApplication { get; set; }

		[Required]
		public string UserId { get; set; }

		[Required]
		public string ApplicationName { get; set; }

		[Required]
		public string ClassName { get; set; }
	}
}