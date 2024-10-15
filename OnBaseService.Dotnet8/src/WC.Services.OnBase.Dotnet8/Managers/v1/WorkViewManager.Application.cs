// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   WC.Services.OnBase
//   WorkViewManager.Application.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.OnBase.Dotnet8.Managers.v1
{
    using global::WC.Services.OnBase.Dotnet8.Models.v1;
    using System.Collections.Generic;

	public partial class WorkViewManager
	{
		public IEnumerable<WVApplication> ListApplications()
			=> _wvadapter.GetApplications();

		public WVApplication GetApplicationById(long applicationId)
			=> _wvadapter.GetApplicationById(applicationId);

		public WVApplication GetApplicationByName(string applicationName)
			=> _wvadapter.GetApplicationByName(applicationName);

	}
}