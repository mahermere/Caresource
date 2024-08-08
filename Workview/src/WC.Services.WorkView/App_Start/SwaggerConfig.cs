﻿// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   Workview
//   SwaggerConfig.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

using System.Web;
using CareSource.WC.Services.WorkView;

[assembly: PreApplicationStartMethod(
	typeof(SwaggerConfig),
	"Register")]

namespace CareSource.WC.Services.WorkView
{
	using System;
	using System.IO;
	using System.Linq;
	using System.Reflection;
	using System.Web.Http;
	using CareSource.WC.OnBase.Core.Http.Swagger;
	using Microsoft.Web.Http;
	using Microsoft.Web.Http.Description;
	using Swashbuckle.Application;

	/// <summary>
	///    Represents the data used to define a the swagger configuration
	/// </summary>
	public class SwaggerConfig
	{
		/// <summary>
		///    Registers this instance.
		/// </summary>
		public static void Register()
		{
			Assembly thisAssembly = typeof(SwaggerConfig).Assembly;

			// reporting api versions will return the headers "api-supported-versions" and "api-deprecated-versions"

			GlobalConfiguration.Configuration.AddApiVersioning(
				options => options.ReportApiVersions = true);

			VersionedApiExplorer apiExplorer =
				GlobalConfiguration.Configuration.AddVersionedApiExplorer(
					options =>
					{
						options.GroupNameFormat = "'v'VVV";

						// note: this option is only necessary when versioning by url segment. the
						// SubstitutionFormat can also be used to control the format of the API version in
						// route templates

						options.SubstituteApiVersionInUrl = true;
					});

			GlobalConfiguration.Configuration
				.EnableSwagger(
					"{apiVersion}/swagger",
					c =>
					{
						c.MultipleApiVersions(
							(
									apiDesc,
									version)
								=>
							{
								ApiVersionAttribute attr
									= apiDesc.ActionDescriptor.ControllerDescriptor
										.GetCustomAttributes<ApiVersionAttribute>()
										.FirstOrDefault();

								if (attr == null &&
								    (version == "v1" || version == "v1.0"))
								{
									return true;
								}

								bool match = attr != null
									&& attr.Versions.FirstOrDefault(v => "v" + v == version) != null;
								return match;
							},
							info
								=>
							{
								info.Version(
									"v5",
									"W&C.Services.WorkView API V5");
								info.Version(
									"v4",
									"W&C.Services.WorkView API V4 - deprecated");
								info.Version(
									"v2",
									"W&C.Services.WorkView API V2 - deprecated");
								info.Version(
									"v1",
									"W&C.Services.WorkView API V1 - deprecated");
							});

						string xmlFile = string.Format(
							$@"{AppDomain.CurrentDomain.BaseDirectory}\bin\CareSource.WC.Services.WorkView.xml");

						if (File.Exists(xmlFile))
						{
							c.IncludeXmlComments(xmlFile);
						}
						
						SwaggerMiddelware.Load(c);
					})
				.EnableSwaggerUi(
					c =>
					{
						c.DocExpansion(DocExpansion.List);
						c.EnableDiscoveryUrlSelector();
						SwaggerMiddelware.LoadUI(c);
					});
		}
	}
}