// ------------------------------------------------------------------------------------------------
//  <copyright>
//    Copyright (c) CareSource, 2020-2022.  All rights reserved.
// 
//    FXIAuthentication
//    CareSourceEnvironmentHelper.cs
//  </copyright>
//  ------------------------------------------------------------------------------------------------

namespace FXIAuthentication.Core.Helpers;

using FXIAuthentication.Core.DependencyInjection;

[DependencyInjectionDependency]
public class CareSourceEnvironmentHelper : ICareSourceEnvironmentHelper
{
	private readonly IEnvironmentVariableHelper _environmentVariableHelper;

	public CareSourceEnvironmentHelper(
		IEnvironmentVariableHelper environmentVariableHelper)
		=> _environmentVariableHelper = environmentVariableHelper;

	public string GetCareSourceEnvironment()
		=> _environmentVariableHelper.GetEnvironmentVariableValue("CARESOURCE_ENVIRONMENT");
}