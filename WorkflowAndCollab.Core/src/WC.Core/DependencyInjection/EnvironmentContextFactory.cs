// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   Core
//   EnvironmentContextFactory.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Core.DependencyInjection
{
	using System.Runtime.InteropServices;
	using CareSource.WC.Core.DependencyInjection.Interfaces;
	using CareSource.WC.Core.Helpers.Interfaces;

	[DependencyInjectionDependency]
	public class EnvironmentContextFactory : IEnvironmentContextFactory
	{
		private readonly ICareSourceEnvironmentHelper _careSourceEnvironmentHelper;
		private readonly IEnvironmentVariableHelper _environmentVariableHelper;

		public EnvironmentContextFactory(
			ICareSourceEnvironmentHelper careSourceEnvironmentHelper,
			IEnvironmentVariableHelper environmentVariableHelper)
		{
			_careSourceEnvironmentHelper = careSourceEnvironmentHelper;
			_environmentVariableHelper = environmentVariableHelper;
		}

		public virtual IEnvironmentContext CreateEnvironmentContext()
		{
			string careSourceIsContainer =
				_environmentVariableHelper.GetEnvironmentVariableValue(
					"CARESOURCE_ENVIRONMENT_IS_CONTAINER");

			bool.TryParse(
				careSourceIsContainer,
				out bool isContainerEnvironment);

			string careSourceHumanFriendlyLogs =
				_environmentVariableHelper.GetEnvironmentVariableValue(
					"CARESOURCE_HUMAN_FRIENDLY_LOGS");

			bool.TryParse(
				careSourceHumanFriendlyLogs,
				out bool humanFriendlyLogs);

			return new EnvironmentContext
			{
				CareSourceEnvironment = _careSourceEnvironmentHelper.GetCareSourceEnvironment(),
				IsContainerEnvironment = isContainerEnvironment,
				IsOperatingSystemWindows = IsOSPlatformWindows(),
				HumanFriendlyLogs = humanFriendlyLogs
			};
		}

		internal virtual bool IsOSPlatformWindows()
			=> RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
	}
}