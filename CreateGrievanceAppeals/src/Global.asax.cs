// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   CreateGrievanceAppeals
//   Global.asax.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.CreateGrievanceAppeals
{
	using System.Web;
	using System.Web.Http;

	public class WebApiApplication : HttpApplication
	{
		protected void Application_Start()
		{
			GlobalConfiguration.Configure(WebApiConfig.Register);
		}
	}
}