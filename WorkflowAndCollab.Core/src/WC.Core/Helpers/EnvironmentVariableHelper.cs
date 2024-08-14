// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   Core
//   EnvironmentVariableHelper.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Core.Helpers
{
	using System;
	using System.Collections;
	using CareSource.WC.Core.DependencyInjection;
	using CareSource.WC.Core.Helpers.Interfaces;

	[DependencyInjectionDependency]
	public class EnvironmentVariableHelper : IEnvironmentVariableHelper
	{
		public IDictionary GetEnvironmentVariables(
			EnvironmentVariableTarget? target = null)
		{
			if (null == target)
			{
				return Environment.GetEnvironmentVariables();
			}

			return Environment.GetEnvironmentVariables(target.Value);
		}

		public string GetEnvironmentVariableValue(
			string variable,
			EnvironmentVariableTarget? target = null)
		{
			if (target == null)
			{
				return Environment.GetEnvironmentVariable(variable);
			}

			return Environment.GetEnvironmentVariable(
				variable,
				target.Value);
		}

		public void SetEnvironmentVariableValue(
			string variable,
			string value,
			EnvironmentVariableTarget? target = null)
		{
			if (target == null)
			{
				Environment.SetEnvironmentVariable(
					variable,
					value);
			}
			else
			{
				Environment.SetEnvironmentVariable(
					variable,
					value,
					target.Value);
			}
		}
	}
}