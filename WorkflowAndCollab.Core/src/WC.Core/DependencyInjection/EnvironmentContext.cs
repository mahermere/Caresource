// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   Core
//   EnvironmentContext.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Core.DependencyInjection
{
	using CareSource.WC.Core.DependencyInjection.Interfaces;

	public class EnvironmentContext : IEnvironmentContext
	{
		public string CareSourceEnvironment { get; internal set; }

		public bool IsContainerEnvironment { get; internal set; }

		public bool IsOperatingSystemWindows { get; internal set; }

		public bool HumanFriendlyLogs { get; internal set; }
	}
}