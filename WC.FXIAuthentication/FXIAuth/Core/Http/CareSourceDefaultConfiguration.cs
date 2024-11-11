// ------------------------------------------------------------------------------------------------
//  <copyright>
//    Copyright (c) CareSource, 2020-2022.  All rights reserved.
// 
//    FXIAuthentication
//    CareSourceDefaultConfiguration.cs
//  </copyright>
//  ------------------------------------------------------------------------------------------------

namespace FXIAuthentication.Core.Http;

using FXIAuthentication.Core.Extensions;
using FXIAuthentication.Core.Http.Middleware;
using Swashbuckle.AspNetCore.SwaggerGen;

public static class CareSourceDefaultConfiguration
{
	public static IServiceCollection AddCareSourceDefaultWebApiConfiguration(
		this IServiceCollection services,
		IConfiguration configuration,
		Action<SwaggerGenOptions> setupAction = null)
	{
		services.AddCareSourceServices(configuration);
		services.AddCareSourceLogging(configuration);
		//services.AddSwaggerConfigurations(configuration, setupAction);

		return services;
	}

	public static IApplicationBuilder AddCareSourceDefaultWebApiConfiguration(
		this WebApplication app,
		IWebHostEnvironment env)
	{
		app.AddSwaggerConfigurations(env);

		//Custom Middleware, they are run in this order.
		app.UseMiddleware<ExceptionMiddleware>();
		app.UseMiddleware<TransactionContextMiddleware>();
		app.UseMiddleware<AuthorizationMiddleware>();

		return app;
	}
}