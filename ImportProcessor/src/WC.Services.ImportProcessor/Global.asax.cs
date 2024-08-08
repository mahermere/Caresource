// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2021. All rights reserved.
// 
//   ImportProcessor
//   Global.asax.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace ImportProcessor
{
	using System.Web;
	using System.Web.Http;
	using System.Web.Mvc;
	using System.Web.Optimization;
	using System.Web.Routing;

	public class WebApiApplication : HttpApplication
	{
		protected void Application_Start()
			=> GlobalConfiguration.Configure(WebApiConfig.Register);
	}
}