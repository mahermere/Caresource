using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Filters;
using Swashbuckle.Swagger;

namespace CareSource.WC.OnBase.Core.Http.Swagger
{
	public class AddAuthorizationHeaderParameterOperationFilter : IOperationFilter
	{
		public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
		{
			var filterPipeline = apiDescription.ActionDescriptor.GetFilterPipeline();
			var isAuthorized = filterPipeline
				.Select(filterInfo => filterInfo.Instance)
				.Any(filter => filter is IAuthorizationFilter);

			var allowAnonymous = apiDescription.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any();

			if (true || (isAuthorized && !allowAnonymous))
			{
				operation.parameters.Add(new Parameter
				{
					name = "Authorization",
					@in = "header",
					description = "access token",
					required = true,
					type = "string",
					@default = "Basic dXNlcm5hbWU6cGFzc3dvcmQ="
				});
			}
		}
	}
}