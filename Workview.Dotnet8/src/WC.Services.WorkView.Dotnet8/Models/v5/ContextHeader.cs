// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2022. All rights reserved.
// 
//   Workview
//   ContextHeader.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.WorkView.Dotnet8.Models.v5
{
	using System;
	using System.ComponentModel.DataAnnotations;

	public class ContextHeader
	{
		/// <summary>
		/// Gets or sets the Context Header Correlation Unique identifier
		/// </summary>
		/// <remarks>
		///	This is an ID that can be used across systems to track requests.
		/// </remarks>
		public Guid CorrelationGuid { get; set; } = Guid.NewGuid();

		/// <summary>
		/// Gets or sets the Context Header Current User
		/// </summary>
		/// <remarks>
		///	Because the API is accessed with service accounts this is used to capture the current
		///	signed in user.
		/// </remarks>
		[Required]
		public string CurrentUser { get; set; }

		/// <summary>
		/// Gets or sets the Context Header Request Date Time
		/// </summary>
		public DateTime RequestDateTime { get; set; } = DateTime.Now;

		/// <summary>
		/// Gets or sets the Context Header Source Application
		/// </summary>
		[Required]
		public string SourceApplication { get; set; }
	}
}