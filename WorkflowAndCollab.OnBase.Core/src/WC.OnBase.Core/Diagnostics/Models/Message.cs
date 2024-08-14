//  ------------------------------------------------------------------------------------------------
//  <copyright>
//    Copyright (c) CareSource, 2020-2022.  All rights reserved.
// 
//    WC.Services.Logging
//    Message.cs
//  </copyright>
//  ------------------------------------------------------------------------------------------------

namespace CareSource.WC.OnBase.Core.Diagnostics.Models
{
	using System;
	using System.Collections.Generic;
	using Newtonsoft.Json;

	public class Message
	{
		[JsonProperty("time")]
		public long Time { get; set; } = DateTimeOffset.Now.ToUniversalTime().ToUnixTimeMilliseconds();

		[JsonProperty("host")]
		public string Host { get; set; } = null;

		[JsonProperty("source")]
		public string Source { get; set; } = null;

		[JsonProperty("sourcetype")]
		public string SourceType { get; set; } = null;

		[JsonProperty("event")]
		public Event Event { get; set; } = null;

		[JsonProperty("fields")]
		public IDictionary<string, string> Fields { get; set; } = new Dictionary<string, string>();
	}

	public class Event
	{
		/// <summary>Gets or sets the message.</summary>
		/// <value>The message.</value>
		[JsonProperty("message")]
		public string Message { get; set; } = string.Empty;

		/// <summary>Gets or sets the event log level</summary>
		[JsonProperty("logLevel")]
		public string LogLevel { get; set; } =
			Microsoft.Extensions.Logging.LogLevel.Information.ToString();
	}
}