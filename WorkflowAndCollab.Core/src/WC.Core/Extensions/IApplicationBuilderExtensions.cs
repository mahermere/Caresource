// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   Core
//   IApplicationBuilderExtensions.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Core.Extensions
{
	using System;
	using CareSource.WC.Core.Helpers.Interfaces;
	using CareSource.WC.Core.Http.Swagger.Interfaces;
	using Microsoft.AspNetCore.Builder;
	using Microsoft.AspNetCore.Hosting;
	using Environment = CareSource.WC.Core.Configuration.Models.Environment;

	public static class IApplicationBuilderExtensions
	{
		public static void AddSwaggerConfigurations(
			this IApplicationBuilder app,
			IHostingEnvironment env = null)
		{
			ISwaggerConfigurationManager swaggerConfigManager =
				(ISwaggerConfigurationManager)app.ApplicationServices.GetService(
					typeof(ISwaggerConfigurationManager));
			ICareSourceEnvironmentHelper environmentHelper =
				(ICareSourceEnvironmentHelper)app.ApplicationServices.GetService(
					typeof(ICareSourceEnvironmentHelper));

			if (swaggerConfigManager == null ||
			    environmentHelper == null)
			{
				throw new Exception(
					$"Both types '{typeof(ISwaggerConfigurationManager).Name}' " +
					$"and '{typeof(ICareSourceEnvironmentHelper).Name}' must be correctly" +
					" dependency injected to add swagger configurations.");
			}

			switch (environmentHelper.GetCareSourceEnvironment())
			{
				case Environment.DevelopLocal:
				case Environment.Develop:
				case Environment.Integration:
				case Environment.Certification:
					swaggerConfigManager.Configure(
						app,
						env);
					break;
			}
		}
	}
}