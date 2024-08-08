// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   WC.Services.OnBase
//   WorkViewManager.Application.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.OnBase.Managers.v1
{
	using System.Collections.Generic;
	using CareSource.WC.Services.OnBase.Adapters.v1;
	using CareSource.WC.Services.OnBase.Models.v1;

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