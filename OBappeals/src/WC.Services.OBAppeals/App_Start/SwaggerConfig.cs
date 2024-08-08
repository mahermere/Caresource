using System.Web.Http;
using WebActivatorEx;
using Swashbuckle.Application;
using WorkFlowAndCollab.API.OBAppeals;
using CareSource.WC.OnBase.Core.Http.Swagger;

[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

namespace WorkFlowAndCollab.API.OBAppeals
{
	public class SwaggerConfig
	{
		public static void Register()
		{
			System.Reflection.Assembly thisAssembly = typeof(SwaggerConfig).Assembly;

			GlobalConfiguration.Configuration
				.EnableSwagger(c =>
				{
					c.SingleApiVersion("v1", "CareSource.WC.Services.OBAppeals");

					SwaggerMiddelware.Load(c);
				})
				.EnableSwaggerUi(c =>
				{
					c.DocExpansion(DocExpansion.List);
					SwaggerMiddelware.LoadUI(c);
				});
		}
	}
}
