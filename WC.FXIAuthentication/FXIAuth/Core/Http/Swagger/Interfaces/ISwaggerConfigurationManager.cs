// ------------------------------------------------------------------------------------------------
//  <copyright>
//    Copyright (c) CareSource, 2020-2022.  All rights reserved.
// 
//    FXIAuthentication
//    ISwaggerConfigurationManager.cs
//  </copyright>
//  ------------------------------------------------------------------------------------------------

namespace FXIAuthentication.Core.Http.Swagger.Interfaces;

using Swashbuckle.AspNetCore.SwaggerGen;

public interface ISwaggerConfigurationManager
{
	void Configure(
		IServiceCollection services,
		IConfiguration configuration,
		Action<SwaggerGenOptions> setupAction);

	void Configure(IApplicationBuilder app,
		IWebHostEnvironment env);
}