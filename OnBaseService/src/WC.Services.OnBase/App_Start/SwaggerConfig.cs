// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   WC.Services.OnBase
//   SwaggerConfig.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

using System.Web;
using CareSource.WC.Services.OnBase;

[assembly: PreApplicationStartMethod(
	typeof(SwaggerConfig),
	"Register")]

namespace CareSource.WC.Services.OnBase
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
									return true;

								bool match = attr != null &&
								             attr.Versions.FirstOrDefault(v => $"v{v}" == version) != null;
								return match;
							},
							info
								=>
							{
								info.Version(
									"v1",
									"W&C.Services.OnBase API V1");
							});

						string xmlFile = string.Format(
							$@"{AppDomain.CurrentDomain.BaseDirectory}\bin\CareSource.WC.Services.OnBase.xml");

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