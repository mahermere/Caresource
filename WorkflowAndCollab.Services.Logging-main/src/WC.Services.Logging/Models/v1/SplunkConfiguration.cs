//  ------------------------------------------------------------------------------------------------
//  <copyright>
//    Copyright (c) CareSource, 2020-2022.  All rights reserved.
// 
//    WC.Services.Logging
//    SplunkConfiguration.cs
//  </copyright>
//  ------------------------------------------------------------------------------------------------

namespace WC.Services.Logging.Models.v1
{
	using Microsoft.Extensions.Logging;

	public class SplunkConfiguration
	{
		/// <summary>
		///    Gets or sets the splunk configuration URL
		/// </summary>
		public string Url { get; set; } = string.Empty;

		/// <summary>
		///    Gets or sets the splunk configuration token
		/// </summary>
		public LogLevel LogLevel { get; set; } = LogLevel.Information;

	}
}