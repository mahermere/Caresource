// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2021. All rights reserved.
// 
//   WC.Services.Hplc
//   Class1.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.Hplc
{
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Linq;
	using System.Web.Http;
	using System.Web.Http.Description;
	using System.Web.Http.Filters;
	using Swashbuckle.Swagger;

	public class AddAuthorizationHeaderParameterOperationFilter : IOperationFilter
	{
		public void Apply(
			Operation operation,
			SchemaRegistry schemaRegistry,
			ApiDescription apiDescription)
		{
			Collection<FilterInfo> filterPipeline =
				apiDescription.ActionDescriptor.GetFilterPipeline();

			bool isAuthorized = filterPipeline
				.Select(filterInfo => filterInfo.Instance)
				.Any(filter => filter is IAuthorizationFilter);

			bool allowAnonymous = apiDescription.ActionDescriptor
				.GetCustomAttributes<AllowAnonymousAttribute>()
				.Any();

			if (true || (isAuthorized && !allowAnonymous))
			{
				if (operation.parameters == null)
				{
					operation.parameters = new List<Parameter>();
				}
				operation.parameters.Add(
					new Parameter
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