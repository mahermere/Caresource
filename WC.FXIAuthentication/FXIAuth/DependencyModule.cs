// ------------------------------------------------------------------------------------------------
//  <copyright>
//    Copyright (c) CareSource, 2020-2022.  All rights reserved.
// 
//    FXIAuthentication
//    DependencyModule.cs
//  </copyright>
//  ------------------------------------------------------------------------------------------------

namespace FXIAuthentication;

using FXIAuthentication.Adapters.v1;
using FXIAuthentication.Core.DependencyInjection;
using FXIAuthentication.Managers.v1;
using Microsoft.Extensions.Caching.Memory;

public class DependencyModule : IDependencyModule
{
	public ushort LoadOrder => 500;


	public void Load(
		DependencyCollection dependencyCollection,
		IEnvironmentContext environmentContext)
		=> ConfigureVersion1(dependencyCollection);

	private void ConfigureVersion1(DependencyCollection dependencyCollection)
	{
		dependencyCollection.AddDependency<IAuthManager, AuthManager>();
		dependencyCollection.AddDependency<IAuthAdapter, AuthAdapter>();

		dependencyCollection.AddSingletonDependency<IMemoryCache, MemoryCache>();
	}
}