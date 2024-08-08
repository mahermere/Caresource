// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   Workview
//   IWorkviewApplicationManager.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.WorkView.Managers.v5
{
	using System.Collections.Generic;
	using System.Web.Http.ModelBinding;
	using CareSource.WC.Services.WorkView.Models.v5;

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