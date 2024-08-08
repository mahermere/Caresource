// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   Workview
//   WorkViewRequest.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.WorkView.Models.v4
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using CareSource.WC.Entities.Requests.Base.Interfaces;

	public interface IWorkViewRequest : IBaseRequest
	{
		/// <summary>
		/// Gets or sets the I Work View Request Correlation Unique identifier
		/// </summary>
		Guid CorrelationGuid { get; set; }
		/// <summary>
		/// Gets or sets the Work View Request Application Name
		/// </summary>
		[Required]
		string ApplicationName { get; set; }

		/// <summary>
		/// Gets or sets the Work View Request Class Name
		/// </summary>
		[Required]
		string ClassName { get; set; }
	}
}