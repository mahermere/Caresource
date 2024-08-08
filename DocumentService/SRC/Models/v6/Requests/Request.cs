// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   WC.Services.Document
//   SearchRequest.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.Document.Models.v6
{
	using System;
	using System.ComponentModel.DataAnnotations;

	public abstract class Request: OnBase.Core.Diagnostics.IRequest
	{
		/// <summary>
		/// Gets or sets the Request Correlation Unique identifier
		/// </summary>
		/// <remarks>
		///	Defaults to a new guid if not provided
		/// </remarks>
		public Guid CorrelationGuid { get; set; } = Guid.NewGuid();

		/// <summary>
		/// Gets or sets the Request Request Date Time
		/// </summary>
		public DateTime RequestDateTime { get; set; } = DateTime.Now;

		/// <summary>
		/// Gets or sets the Request Source Application
		/// </summary>
		[Required]
		public string SourceApplication { get; set; }

		/// <summary>
		/// Gets or sets the Request User Identifier
		/// </summary>
		[Required]
		public string UserId { get; set; }
	}
}