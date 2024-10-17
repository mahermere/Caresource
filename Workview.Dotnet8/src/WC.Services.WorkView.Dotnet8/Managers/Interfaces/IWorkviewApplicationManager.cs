// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   Workview
//   IWorkviewApplicationManager.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.WorkView.Dotnet8.Managers
{
	using System.Collections.Generic;
	//using System.Web.Http.ModelBinding;
	using CareSource.WC.Entities.WorkView;
    using Microsoft.AspNetCore.Mvc.ModelBinding;

    public interface IWorkViewApplicationManager
	{
		bool ValidateRequest(WorkviewObjectRequest request, ModelStateDictionary modelState);

		WorkviewObject CreateNewObject(WorkviewObjectRequest request);

		IEnumerable<WorkviewObject> FindWorkviewObjects(WorkviewObjectRequest request);

		WorkviewObject GetWorkviewObject(long id, WorkviewObjectRequest request);
	}
}