// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   WorkFlowAndCollab.API.OBAppeals
//   Global.asax.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.OBAppeals
{
	using System.Web;
	using System.Web.Http;

	public class WebApiApplication : HttpApplication
	{
		protected void Application_Start() => GlobalConfiguration.Configure(WebApiConfig.Register);
	}
}