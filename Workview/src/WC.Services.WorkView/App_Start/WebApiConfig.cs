// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   Workview
//   WebApiConfig.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.WorkView
{
	using System.Web.Http;
	using System.Web.Http.Routing;
	using CareSource.WC.OnBase.Core.Http;
	using Microsoft.Web.Http.Routing;
	using Newtonsoft.Json;

	public static class WebApiConfig
	{
		public static void Register(HttpConfiguration config)
		{
			DefaultInlineConstraintResolver constraintResolver = new DefaultInlineConstraintResolver
			{
				ConstraintMap =
				{
					["apiVersion"] = typeof(ApiVersionRouteConstraint)
				}
			};

			
			config.MapHttpAttributeRoutes(constraintResolver);
			config.AddApiVersioning();
			//config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
			ServiceConfigMiddelware.Load(config);

			config.Formatters.JsonFormatter.SerializerSettings = new JsonSerializerSettings
			{
				NullValueHandling = NullValueHandling.Ignore,
				DateFormatString = "s",
				DefaultValueHandling = DefaultValueHandling.Ignore,
				ReferenceLoopHandling = ReferenceLoopHandling.Ignore
			};
		}
	}
}