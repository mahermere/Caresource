// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   Workview
//   IWorkviewApplicationManager.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.WorkView.Dotnet8.Managers.v5
{
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using System.Collections.Generic;
	using WC.Services.WorkView.Dotnet8.Models.v5;

	public interface ISearchManager
	{
		bool ValidateRequest(
			string workviewApplicationName,
			SearchRequest request,
			ModelStateDictionary modelState);

		IEnumerable<WorkViewObject> Search(
			string workViewApplicationName,
			SearchRequest request);
	}
}