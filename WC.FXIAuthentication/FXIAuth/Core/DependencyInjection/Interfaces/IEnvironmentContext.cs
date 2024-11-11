// ------------------------------------------------------------------------------------------------
//  <copyright>
//    Copyright (c) CareSource, 2020-2022.  All rights reserved.
// 
//    FXIAuthentication
//    IEnvironmentContext.cs
//  </copyright>
//  ------------------------------------------------------------------------------------------------

namespace FXIAuthentication.Core.DependencyInjection;

public interface IEnvironmentContext
{
	string CareSourceEnvironment { get; }

	bool IsContainerEnvironment { get; }

	bool IsOperatingSystemWindows { get; }

	bool HumanFriendlyLogs { get; }
}