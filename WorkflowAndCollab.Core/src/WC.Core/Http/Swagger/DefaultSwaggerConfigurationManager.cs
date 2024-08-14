// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   Core
//   DefaultSwaggerConfigurationManager.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Core.Http.Swagger
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using CareSource.WC.Core.Helpers.Interfaces;
	using CareSource.WC.Core.Http.Swagger.Interfaces;
	using Microsoft.AspNetCore.Builder;
	using Microsoft.AspNetCore.Hosting;
	using Microsoft.Extensions.Configuration;
	using Microsoft.Extensions.DependencyInjection;
	using Swashbuckle.AspNetCore.Swagger;
	using Swashbuckle.AspNetCore.SwaggerGen;

	public class DefaultSwaggerConfigurationManager : ISwaggerConfigurationManager
	{
		private readonly IAssemblyHelper _assemblyHelper;

		public DefaultSwaggerConfigurationManager(IAssemblyHelper assemblyHelper)
		{
			_assemblyHelper = assemblyHelper;
		}

		public void Configure(IServiceCollection services, IConfiguration configuration
			, Action<SwaggerGenOptions> setupAction)
		{
			services.AddSwaggerGen(
				c =>
				{
					c.AddSecurityDefinition("Bearer", new ApiKeyScheme
					{
						In = "header",
						Description = "Please Enter OnBase Username and Password",
						Name = "Authorization",
						Type = "basic"
					});

					c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>> {
						{ "Bearer", Enumerable.Empty<string>() },
					});

					// Set the comments path for the Swagger JSON and UI.
					var assemblyName = _assemblyHelper.GetEntryAssemblyName()?
						.Split(",")?[0];
					var xmlFile = $"{assemblyName}.xml";
					var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
					if (File.Exists(xmlPath))
					{
						c.IncludeXmlComments(xmlPath);
					}

					setupAction?.Invoke(c);
				});
		}

		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
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
}