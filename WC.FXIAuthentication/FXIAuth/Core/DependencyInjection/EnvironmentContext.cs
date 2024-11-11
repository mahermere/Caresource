// ------------------------------------------------------------------------------------------------
//  <copyright>
//    Copyright (c) CareSource, 2020-2022.  All rights reserved.
// 
//    FXIAuthentication
//    EnvironmentContext.cs
//  </copyright>
//  ------------------------------------------------------------------------------------------------

namespace FXIAuthentication.Core.DependencyInjection;

public class EnvironmentContext : IEnvironmentContext
{
	public string CareSourceEnvironment { get; internal set; }

	public bool IsContainerEnvironment { get; internal set; }

	public bool IsOperatingSystemWindows { get; internal set; }

	public bool HumanFriendlyLogs { get; internal set; }
}