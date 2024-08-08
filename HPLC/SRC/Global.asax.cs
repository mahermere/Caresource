// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2021. All rights reserved.
// 
//   WC.Services.Hplc
//   Global.asax.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.Hplc
{
	using System.Web;
	using System.Web.Http;

	public class WebApiApplication : HttpApplication
	{
		protected void Application_Start()
			=> GlobalConfiguration.Configure(WebApiConfig.Register);
	}
}