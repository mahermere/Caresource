// ------------------------------------------------------------------------------------------------
//  <copyright>
//    Copyright (c) CareSource, 2020-2022.  All rights reserved.
// 
//    FXIAuthentication
//    IEnvironmentVariableHelper.cs
//  </copyright>
//  ------------------------------------------------------------------------------------------------

namespace FXIAuthentication.Core.Helpers;

using System.Collections;

public interface IEnvironmentVariableHelper
{
	IDictionary GetEnvironmentVariables(
		EnvironmentVariableTarget? target = null);

	string GetEnvironmentVariableValue(
		string environmentVariableName,
		EnvironmentVariableTarget? target = null);

	void SetEnvironmentVariableValue(
		string environmentVariableName,
		string environmentVariableValue,
		EnvironmentVariableTarget? target);
}