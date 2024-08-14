// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   Core
//   IEnvironmentVariableHelper.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Core.Helpers.Interfaces
{
	using System;
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
}