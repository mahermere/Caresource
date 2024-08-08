// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2023. All rights reserved.
// 
//   WC.Services.HplcManagement
//   Message.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace HplcManagement.Models.Core
{
	using System;
	using System.Collections.Generic;
	using Newtonsoft.Json;

	public class Message
	{
		[JsonProperty("time")]
		public long Time { get; set; } = DateTimeOffset.Now.ToUniversalTime()
			.ToUnixTimeMilliseconds();

		[JsonProperty("host")]
		public string Host { get; set; }

		[JsonProperty("source")]
		public string Source { get; set; }

		[JsonProperty("sourcetype")]
		public string SourceType { get; set; }

		[JsonProperty("event")]
		public Event Event { get; set; }

		[JsonProperty("fields")]
		public IDictionary<string, string> Fields { get; set; } = new Dictionary<string, string>();
	}
}