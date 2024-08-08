// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   CreateGrievanceAppeals
//   WebApiConfig.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.CreateGrievanceAppeals
{
	using System.Web.Http;
	using CareSource.WC.OnBase.Core.Http;

	public static class WebApiConfig
	{
		public static void Register(
			HttpConfiguration config)
		{
			// Web API configuration and services
			// Configure Web API to use only bearer token authentication.
			//config.SuppressDefaultHostAuthentication();
			//config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

			// Web API routes
			config.MapHttpAttributeRoutes();

			config.Routes.MapHttpRoute(
				"DefaultApi",
				"api/{controller}/{id}",
				new {id = RouteParameter.Optional}
			);

			ServiceConfigMiddelware.Load(config);
			//config.MessageHandlers.Add(new LogRequestAndResponseHandler());
		}
	}
}