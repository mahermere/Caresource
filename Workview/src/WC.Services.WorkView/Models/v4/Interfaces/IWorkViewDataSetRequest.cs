// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   Workview
//   WorkViewDataSetRequest.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.WorkView.Models.v4
{
	using System.ComponentModel.DataAnnotations;

	public interface IWorkViewDataSetRequest : IWorkViewRequest
	{
		[Required]
		string DataSetName { get; set; }
	}
}