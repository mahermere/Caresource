//  ------------------------------------------------------------------------------------------------
//  <copyright>
//    Copyright (c) CareSource, 2020-2022.  All rights reserved.
// 
//    WC.Services.Logging
//    SplunkResponse.cs
//  </copyright>
//  ------------------------------------------------------------------------------------------------

namespace WC.Services.Logging.Models.v1
{
	using Newtonsoft.Json;

	public class SplunkResponse : ILoggingResponse
	{
		[JsonProperty("text")]
		public string Text { get; set; }

		[JsonProperty("code")]
		public int Code { get; set; }

		[JsonProperty("correlationGuid")]
		public string CorrelationGuid { get; set; }

		public string Message { get; set; }
	}
}