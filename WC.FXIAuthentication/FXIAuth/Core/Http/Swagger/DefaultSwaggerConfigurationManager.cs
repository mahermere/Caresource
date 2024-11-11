// ------------------------------------------------------------------------------------------------
//  <copyright>
//    Copyright (c) CareSource, 2020-2022.  All rights reserved.
// 
//    FXIAuthentication
//    DefaultSwaggerConfigurationManager.cs
//  </copyright>
//  ------------------------------------------------------------------------------------------------

namespace FXIAuthentication.Core.Http.Swagger;

using FXIAuthentication.Core.Helpers;
using FXIAuthentication.Core.Http.Swagger.Interfaces;
using Swashbuckle.AspNetCore.SwaggerGen;

public class DefaultSwaggerConfigurationManager : ISwaggerConfigurationManager
{
	private readonly IAssemblyHelper _assemblyHelper;

	public DefaultSwaggerConfigurationManager(IAssemblyHelper assemblyHelper) =>
		_assemblyHelper = assemblyHelper;

	public void Configure(IServiceCollection services,
		IConfiguration configuration,
		Action<SwaggerGenOptions> setupAction) =>
		services.AddSwaggerGen(
			c =>
			{
				//c.AddSecurityDefinition("Bearer",
				//	new ApiKeyScheme
				//	{
				//		In = "header",
				//		Description = "Please Enter OnBase Username and Password",
				//		Name = "Authorization",
				//		Type = "basic"
				//	});

				//c.AddSecurityRequirement(
				//	new Dictionary<string, IEnumerable<string>>
				//	{
				//		{ "Bearer", Enumerable.Empty<string>() }
				//	});

				// Set the comments path for the Swagger JSON and UI.
				string? assemblyName = _assemblyHelper.GetEntryAssemblyName()?
					.Split(",")?[0];
				string xmlFile = $"{assemblyName}.xml";
				string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
				if (File.Exists(xmlPath))
				{
					c.IncludeXmlComments(xmlPath);
				}

				setupAction?.Invoke(c);
			});

	public void Configure(IApplicationBuilder app,
		IWebHostEnvironment env)
	{
		app.UseSwagger(o => o.RouteTemplate = "swagger/{documentName}/swagger.json");
		app.UseSwaggerUI(
			o =>
			{
				o.SwaggerEndpoint(
					"v1/swagger.json",
					"Version 1");
				o.RoutePrefix = "swagger";
			});
	}
}