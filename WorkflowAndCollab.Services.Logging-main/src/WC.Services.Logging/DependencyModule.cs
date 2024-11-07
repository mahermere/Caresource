//  ------------------------------------------------------------------------------------------------
//  <copyright>
//    Copyright (c) CareSource, 2020-2022.  All rights reserved.
// 
//    WC.Services.Logging
//    DependencyModule.cs
//  </copyright>
//  ------------------------------------------------------------------------------------------------

namespace WC.Services.Logging
{
	using CareSource.WC.Core.DependencyInjection;
	using CareSource.WC.Core.DependencyInjection.Interfaces;
	using WC.Services.Logging.Adapters.v1;
	using WC.Services.Logging.Adapters.v1.Splunk;
	using WC.Services.Logging.Managers.v1;

	public class DependencyModule : IDependencyModule
	{
		public ushort LoadOrder => 500;

		public void Load(
			DependencyCollection dependencyCollection,
			IEnvironmentContext environmentContext)
			=> ConfigureVersion1(dependencyCollection);

		private void ConfigureVersion1(DependencyCollection dependencyCollection)
		{
			dependencyCollection.AddDependency<ILoggingManager, LoggingManager>();
			dependencyCollection.AddDependency<ILoggingAdapter, SplunkAdapter>();
		}
	}
}