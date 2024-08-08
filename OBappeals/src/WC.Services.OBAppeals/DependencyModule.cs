// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   WorkFlowAndCollab.API.OBAppeals
//   DependencyModule.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.OBAppeals
{
	using CareSource.WC.Entities.Appeals;
	using CareSource.WC.OnBase.Core.DependencyInjection;
	using CareSource.WC.OnBase.Core.Http;
	using CareSource.WC.OnBase.Core.Http.Interfaces;
	using CareSource.WC.Services.OBAppeals.Adapters;
	using CareSource.WC.Services.OBAppeals.Adapters.Interfaces;
	using CareSource.WC.Services.OBAppeals.Managers;
	using CareSource.WC.Services.OBAppeals.Managers.Interfaces;
	using Unity;

	public class DependencyModule : IDependencyModule
	{
		public void Load(IUnityContainer container)
		{
			container
				.RegisterSingleton<IAppealsManager<Appeal>, AppealsManager>();
			container
				.RegisterSingleton<IProviderAppealsManager<Appeal>, ProviderAppealsManager>();
			container
				.RegisterSingleton<IMemberAppealsManager<Appeal>, MemberAppealsManager>();
			container
				.RegisterSingleton<IClaimAppealsManager<Appeal>, ClaimAppealsManager>();
			container
				.RegisterSingleton<IGrievancesManager<Appeal>, GrievancesManager>();
			container
				.RegisterSingleton<IMemberGrievancesManager<Appeal>, MemberGrievancesManager>();
			container
				.RegisterSingleton<IProviderGrievancesManager<Appeal>, ProviderGrievancesManager>();
			container
				.RegisterSingleton<IWorkViewObjectsAdapter<WorkViewObjectsHeader>, WorkViewObjectsAdapter>();
			container
				.RegisterSingleton<IRequestAuthorizer, OnBaseRequestAuthorizer>();
		}

		public ushort LoadOrder => 500;
	}
}