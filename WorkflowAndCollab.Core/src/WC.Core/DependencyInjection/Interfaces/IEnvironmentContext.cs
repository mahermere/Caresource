// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   Core
//   IEnvironmentContext.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Core.DependencyInjection.Interfaces
{
	public interface IEnvironmentContext
	{
		string CareSourceEnvironment { get; }

		bool IsContainerEnvironment { get; }

		bool IsOperatingSystemWindows { get; }

		bool HumanFriendlyLogs { get; }
	}
}