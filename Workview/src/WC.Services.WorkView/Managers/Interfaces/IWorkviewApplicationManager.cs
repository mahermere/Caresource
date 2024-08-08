// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   Workview
//   IWorkviewApplicationManager.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.WorkView.Managers
{
	using System.Collections.Generic;
	using System.Web.Http.ModelBinding;
	using CareSource.WC.Entities.WorkView;

	public interface IWorkViewApplicationManager
	{
		bool ValidateRequest(WorkviewObjectRequest request, ModelStateDictionary modelState);

		WorkviewObject CreateNewObject(WorkviewObjectRequest request);

		IEnumerable<WorkviewObject> FindWorkviewObjects(WorkviewObjectRequest request);

		WorkviewObject GetWorkviewObject(long id, WorkviewObjectRequest request);
	}
}