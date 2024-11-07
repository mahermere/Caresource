// ------------------------------------------------------------------------------------------------
//  <copyright>
//    Copyright (c) CareSource, 2020-2022.  All rights reserved.
// 
//    FXIAuthentication
//    DependencyModuleLoader.cs
//  </copyright>
//  ------------------------------------------------------------------------------------------------

namespace FXIAuthentication.Core.DependencyInjection;

using FXIAuthentication.Core.Helpers;

public class DependencyModuleLoader
{
	private readonly IEnvironmentContextFactory _environmentContextFactory;
	private readonly IReflectionHelper _reflectionHelper;

	public DependencyModuleLoader(
		IReflectionHelper reflectionHelper,
		IEnvironmentContextFactory environmentContextFactory)
	{
		_reflectionHelper = reflectionHelper;
		_environmentContextFactory = environmentContextFactory;
	}

	public void LoadAllDependenciesFromModules(
		IServiceCollection serviceCollection)
	{
		IEnumerable<Type> dependencyModuleTypes =
			_reflectionHelper.GetTypesAssignableFromBaseType<IDependencyModule>(true);

		IEnumerable<IDependencyModule> dependencyModules =
			dependencyModuleTypes.Select(t => _reflectionHelper.CreateInstance(t))
				.Cast<IDependencyModule>()
				.OrderBy(dm => dm.LoadOrder);

		DependencyCollection dependencies = new(serviceCollection);

		IEnvironmentContext environmentContext =
			_environmentContextFactory.CreateEnvironmentContext();

		foreach (IDependencyModule dependencyModule in dependencyModules)
		{
			dependencyModule.Load(
				dependencies,
				environmentContext);
		}
	}
}