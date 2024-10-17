// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   Workview
//   WorkViewDataSetRequest.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.WorkView.Dotnet8.Models.v5
{
	using System.ComponentModel.DataAnnotations;

	public interface IWorkViewDataSetRequest : ICreateRequest
	{
		[Required]
		string DataSetName { get; set; }
	}
}