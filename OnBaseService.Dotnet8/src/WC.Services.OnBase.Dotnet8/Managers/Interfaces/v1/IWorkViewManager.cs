// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   WC.Services.OnBase
//   IWorkViewManager.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.OnBase.Dotnet8.Managers.Interfaces.v1
{
	using System.Collections.Generic;
	using CareSource.WC.Entities.Requests.Base;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using WC.Services.OnBase.Dotnet8.Models.v1;

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
