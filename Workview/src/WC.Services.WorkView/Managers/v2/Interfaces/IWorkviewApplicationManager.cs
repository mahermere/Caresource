// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   Workview
//   IWorkviewApplicationManager.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.WorkView.Managers.v2
{
	using System.Collections.Generic;
	using System.Web.Http.ModelBinding;
	using CareSource.WC.Entities.Workview.v2;

	public interface IWorkViewApplicationManager
	{
		bool ValidateRequest(
			WorkviewObjectGetRequest request,
			ModelStateDictionary modelState);

		bool ValidateRequest(
			WorkviewObjectBatchRequest request,
			ModelStateDictionary modelState);

		IEnumerable<WorkviewObject> CreateNewObjects(WorkviewObjectBatchRequest request);

		IEnumerable<WorkviewObject> FindWorkviewObjects(WorkviewObjectGetRequest request);

		WorkviewObject GetWorkviewObject(long id, WorkviewObjectGetRequest request);
	}
}