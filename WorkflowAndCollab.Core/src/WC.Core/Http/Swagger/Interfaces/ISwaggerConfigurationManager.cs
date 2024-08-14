// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   Core
//   ISwaggerConfigurationManager.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Core.Http.Swagger.Interfaces
{
	using System;
	using Microsoft.AspNetCore.Builder;
	using Microsoft.AspNetCore.Hosting;
	using Microsoft.Extensions.Configuration;
	using Microsoft.Extensions.DependencyInjection;
	using Swashbuckle.AspNetCore.SwaggerGen;

	public interface ISwaggerConfigurationManager
	{
		void Configure(
			IServiceCollection services,
			IConfiguration configuration,
			Action<SwaggerGenOptions> setupAction);

		void Configure(
			IApplicationBuilder app,
			IHostingEnvironment env);
	}
}