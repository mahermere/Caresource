// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   WC.Services.OnBase
//   IWorkViewManager.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.OnBase.Managers.Interfaces.v1
{
	using System.Collections.Generic;
	using System.Web.Http.ModelBinding;
	using CareSource.WC.Entities.Requests.Base;
	using CareSource.WC.Services.OnBase.Models.v1;

	public interface IWorkViewManager
	{
		IEnumerable<WVApplication> ListApplications();

		WVApplication GetApplicationById(long applicationId);

		WVApplication GetApplicationByName(string applicationName);

		bool ValidateRequest(
			BaseRequest request,
			ModelStateDictionary modelState);
		}
}
