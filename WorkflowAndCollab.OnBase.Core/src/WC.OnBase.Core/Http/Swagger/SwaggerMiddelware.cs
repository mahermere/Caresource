using Swashbuckle.Application;

namespace CareSource.WC.OnBase.Core.Http.Swagger
{
	public static class SwaggerMiddelware
    {
		public static void Load(SwaggerDocsConfig config)
		{
            config.BasicAuth("basic").Description("Basic HTTP Authentication");
            config.OperationFilter<AddAuthorizationHeaderParameterOperationFilter>();
        }

        public static void LoadUI(SwaggerUiConfig config)
        {
        }
    }
}