using CareSource.WC.Services.Document;
using System.Web;

[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

namespace CareSource.WC.Services.Document
{
	using System.Web.Http;
	using Swashbuckle.Application;
	using System.Reflection;
	using CareSource.WC.OnBase.Core.Http.Swagger;
	using Microsoft.Web.Http.Description;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.IO;
	using System.Web.Http.Description;
	using CareSource.WC.Entities.Transactions;
	using Swashbuckle.Swagger;

	/// <summary>
	/// Represents the data used to define a the swagger configuration
	/// </summary>
	public class SwaggerConfig
	{
		/// <summary>
		/// Registers this instance.
		/// </summary>
		public static void Register()
		{

			Assembly thisAssembly = typeof(SwaggerConfig).Assembly;

			// reporting api versions will return the headers "api-supported-versions" and "api-deprecated-versions"

			GlobalConfiguration.Configuration.AddApiVersioning(options => options.ReportApiVersions = true);

			VersionedApiExplorer apiExplorer = GlobalConfiguration.Configuration.AddVersionedApiExplorer(
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
						c.SchemaId(x => x.FullName);
						c.MultipleApiVersions(
							(
									apiDesc,
									version)
								=>
							{
								Microsoft.Web.Http.ApiVersionAttribute attr
									= apiDesc.ActionDescriptor.ControllerDescriptor
										.GetCustomAttributes<Microsoft.Web.Http.ApiVersionAttribute>()
										.FirstOrDefault();

								if (attr == null && (version == "v1" || version == "v1.0"))
									return true;

								bool match = (attr != null)
									&& (attr.Versions.FirstOrDefault(v => $"v{v}" == version) != null);
								return match;
							},
								info
									=>
							{
								info.Version("v5", "W&C.Services.Document API v5");
								info.Version("v6", "W&C.Services.Document API v6 (beta)");
								info.Version("v4", "W&C.Services.Document API v4 (Deprecated)");
								info.Version("v3", "W&C.Services.Document API v3 (Deprecated)");
								info.Version("v2", "W&C.Services.Document API V2 (Deprecated)");
								info.Version("v1", "W&C.Services.Document API V1 (Deprecated)");
							});

						string xmlFile = string.Format(
							$@"{AppDomain.CurrentDomain.BaseDirectory}bin\CareSource.WC.Services.Document.xml");

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
