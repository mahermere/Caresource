// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2023. All rights reserved.
// 
//   WC.Services.HplcManagement
//   Message.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.HplcManagement.Dotnet8.Models.Core
{
	using System;
	using System.Collections.Generic;
	using System.Text.Json.Serialization;

	public class Message
	{
		[JsonPropertyName("time")]
		public long Time { get; set; } = DateTimeOffset.Now.ToUniversalTime()
			.ToUnixTimeMilliseconds();

		[JsonPropertyName("host")]
		public string Host { get; set; }

		[JsonPropertyName("source")]
		public string Source { get; set; }

		[JsonPropertyName("sourcetype")]
		public string SourceType { get; set; }

		[JsonPropertyName("event")]
		public Event Event { get; set; }

		[JsonPropertyName("fields")]
		public IDictionary<string, string> Fields { get; set; } = new Dictionary<string, string>();
	}
}