// ------------------------------------------------------------------------------------------------
//  <copyright>
//    Copyright (c) CareSource, 2020-2022.  All rights reserved.
// 
//    FXIAuthentication
//    IApplicationBuilderExtensions.cs
//  </copyright>
//  ------------------------------------------------------------------------------------------------

namespace FXIAuthentication.Core.Extensions;

using FXIAuthentication.Core.Configuration.Models;
using FXIAuthentication.Core.Helpers;
using FXIAuthentication.Core.Http.Swagger.Interfaces;

public static class ApplicationBuilderExtensions
{
	public static void AddSwaggerConfigurations(
		this IApplicationBuilder app,
		IWebHostEnvironment env = null)
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
				$"Both types '{nameof(ISwaggerConfigurationManager)}' " +
				$"and '{nameof(ICareSourceEnvironmentHelper)}' must be correctly" +
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