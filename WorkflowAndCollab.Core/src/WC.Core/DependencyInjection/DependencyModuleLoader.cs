// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   Core
//   DependencyModuleLoader.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Core.DependencyInjection
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using CareSource.WC.Core.DependencyInjection.Interfaces;
	using CareSource.WC.Core.Helpers.Interfaces;
	using Microsoft.Extensions.DependencyInjection;

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

			DependencyCollection dependencies = new DependencyCollection(serviceCollection);

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
}