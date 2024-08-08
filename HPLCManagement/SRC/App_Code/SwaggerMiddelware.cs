// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2021. All rights reserved.
// 
//   WC.Services.Hplc
//   Class1.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace HplcManagement
{
	using Swashbuckle.Application;

	public static class SwaggerMiddelware
	{
		public static void Load(SwaggerDocsConfig config)
		{
			config.BasicAuth("basic")
				.Description("Basic HTTP Authentication");
			config.OperationFilter<AddAuthorizationHeaderParameterOperationFilter>();
		}

		public static void LoadUI(SwaggerUiConfig config)
		{ }
	}
}