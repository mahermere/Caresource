// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2022. All rights reserved.
// 
//   Workview
//   IBaseRequest.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.WorkView.Models.v5
{
	using System;
	using System.ComponentModel;
	using System.ComponentModel.DataAnnotations;

	public interface IBaseRequest
	{
		/// <summary>
		/// Gets or sets the I Work View Request Correlation Unique identifier
		/// </summary>
		Guid CorrelationGuid { get; set; }

		/// <summary>
		/// Gets or sets the I Base Request Request Date Time
		/// </summary>
		DateTime RequestDateTime { get; set; }

		/// <summary>
		/// Gets or sets the I Base Request Source Application
		/// </summary>
		[Required]
		[Description("Identifier string to help identify the systems calling the service")]
		string SourceApplication { get; set; }

		/// <summary>
		/// Gets or sets the I Base Request User Identifier
		/// </summary>
		[Required]
		[Description("The current signed in user, used for logging.")]
		string UserId { get; set; }
	}
}