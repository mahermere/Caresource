// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   Core
//   CareSourceDefaultConfiguration.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Core.Http
{
	using System;
	using CareSource.WC.Core.Extensions;
	using CareSource.WC.Core.Http.Middleware;
	using Microsoft.AspNetCore.Builder;
	using Microsoft.AspNetCore.Hosting;
	using Microsoft.Extensions.Configuration;
	using Microsoft.Extensions.DependencyInjection;
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
			services.AddSwaggerConfigurations(configuration, setupAction);

			return services;
		}

		public static IApplicationBuilder AddCareSourceDefaultWebApiConfiguration(
			this IApplicationBuilder app,
			IHostingEnvironment env)
		{
			app.AddSwaggerConfigurations(env);

			//Custom Middleware, they are run in this order.
			app.UseMiddleware<ExceptionMiddleware>();
			app.UseMiddleware<TransactionContextMiddleware>();
			app.UseMiddleware<AuthorizationMiddleware>();

			return app;
		}
	}
}