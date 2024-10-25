// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   WC.Services.Document
//   SearchRequest.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.Document.Dotnet8.Models.v6.Requests
{
	using System;
	using System.ComponentModel.DataAnnotations;
    using WC.Services.Document.Dotnet8.Models.v6.Interfaces;

    public abstract class Request: IRequest
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
		//Required]
		public string SourceApplication { get; set; }

		/// <summary>
		/// Gets or sets the Request User Identifier
		/// </summary>
		//[Required]
		public string UserId { get; set; }
	}
}