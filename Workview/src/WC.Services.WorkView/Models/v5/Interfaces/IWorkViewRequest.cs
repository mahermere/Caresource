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
	using System.ComponentModel.DataAnnotations;

	public interface IWorkViewRequest : IBaseRequest
	{
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