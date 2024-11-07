// ------------------------------------------------------------------------------------------------
//  <copyright>
//    Copyright (c) CareSource, 2020-2023.  All rights reserved.
// 
//    Claims
//    DependencyInjection.cs
//  </copyright>
//  ------------------------------------------------------------------------------------------------

namespace Claims.OnBase.Core
{
	using Claims.Adapter;
	using Claims.Adapter.Interfaces;
	using Claims.Managers;
	using Claims.Managers.Interfaces;
	using Claims.Models;

	public static class DependencyInjection
	{
		public static void ConfigureDependencies(IServiceCollection services)
		{
			services.AddScoped<IClaimsManager, ClaimsManager>();
			services.AddScoped<IClaimsAdapter<Claim>, ClaimsFacetsApiAdapter>();
			services.AddScoped<IMemberAdapter<Member>, MemberFacetsApiAdapter>();
			services.AddScoped<IQueueAdapter<FacetsQueueMessage>, QueueFacetsDapperAdapter>();
			services.AddScoped<IDCNDataAdapter<DCNClaimData>, DCNFWSApiAdapter>();
		}
	}
}