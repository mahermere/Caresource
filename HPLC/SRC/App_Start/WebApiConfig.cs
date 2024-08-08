﻿// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2021. All rights reserved.
// 
//   WC.Services.Hplc
//   WebApiConfig.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.Hplc
{
	using System.Web.Http;
	using System.Web.Http.Routing;
	using CareSource.WC.OnBase.Core.Http;
	using Microsoft.Web.Http.Routing;

	/// <summary>
	///    Represents the data used to define a the web API configuration
	/// </summary>
	public static class WebApiConfig
	{
		/// <summary>
		///    Registers the specified configuration.
		/// </summary>
		/// <param name="config">The configuration.</param>
		public static void Register(HttpConfiguration config)
		{
			DefaultInlineConstraintResolver constraintResolver = new DefaultInlineConstraintResolver
			{
				ConstraintMap = { ["apiVersion"] = typeof(ApiVersionRouteConstraint) }
			};

			config.MapHttpAttributeRoutes(constraintResolver);
			config.AddApiVersioning();

			ServiceConfigMiddelware.Load(config);
		}
	}
}