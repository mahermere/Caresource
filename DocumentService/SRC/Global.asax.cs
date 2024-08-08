// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   Caresource.WC.Services.Document.WC.Services.Document
//   Global.asax.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.Document
{
	using System.Web;
	using System.Web.Http;

	/// <summary>
	/// Represents the data used to define a the web API application
	/// </summary>
	/// <seealso cref="System.Web.HttpApplication" />
	public class WebApiApplication : HttpApplication
	{
		/// <summary>
		/// Application Start 
		/// </summary>
		protected void Application_Start()
			=> GlobalConfiguration.Configure(WebApiConfig.Register);
	}
}