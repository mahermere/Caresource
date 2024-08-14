// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   Core
//   CareSourceEnvironmentHelper.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Core.Helpers
{
	using CareSource.WC.Core.DependencyInjection;
	using CareSource.WC.Core.Helpers.Interfaces;

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
}