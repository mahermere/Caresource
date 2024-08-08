// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2021. All rights reserved.
// 
//   ImportProcessor
//   KeywordsUpdateRequest.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace ImportProcessor.Models.v1
{
	using System;
	using System.Collections.Generic;
	using Newtonsoft.Json;

	public class KeywordsUpdateRequestKw
	{
		[JsonProperty("RequestData")]
		public RequestDataKw[] RequestData { get; set; }

		[JsonProperty("CorrelationGuid")]
		public Guid CorrelationGuid { get; set; }

		[JsonProperty("RequestDateTime")]
		public DateTimeOffset RequestDateTime { get; set; }

		[JsonProperty("SourceApplication")]
		public string SourceApplication { get; set; }

		[JsonProperty("UserId")]
		public string UserId { get; set; }
	}

	public class RequestDataKw
	{
		[JsonProperty("DocumentId")]
		public long DocumentId { get; set; }

		[JsonProperty("Keywords")]
		public IDictionary<string, string> Keywords { get; set; }
	}
}