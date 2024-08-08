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