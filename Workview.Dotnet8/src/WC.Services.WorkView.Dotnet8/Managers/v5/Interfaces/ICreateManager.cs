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
	//using System.Web.Http.ModelBinding;
	using WC.Services.WorkView.Dotnet8.Models.v5;

	public interface ICreateManager
	{
		bool ValidateRequest(
			string workviewApplicationName,
			CreateRequest request,
			ModelStateDictionary modelState);

		IEnumerable<WorkViewObject> CreateNewObject(
			string workviewApplicationName,
			CreateRequest request);
	}
}