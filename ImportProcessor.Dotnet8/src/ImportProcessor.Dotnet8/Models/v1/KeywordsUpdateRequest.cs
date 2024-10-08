// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2021. All rights reserved.
// 
//   ImportProcessor
//   KeywordsUpdateRequest.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.ImportProcessor.Dotnet8.Models.v1
{
	using System;
	using System.Collections.Generic;
	using System.Text.Json.Serialization;

	public class KeywordsUpdateRequestKw
	{
		[JsonPropertyName("RequestData")]
		public RequestDataKw[] RequestData { get; set; }

		[JsonPropertyName("CorrelationGuid")]
		public Guid CorrelationGuid { get; set; }

		[JsonPropertyName("RequestDateTime")]
		public DateTimeOffset RequestDateTime { get; set; }

		[JsonPropertyName("SourceApplication")]
		public string SourceApplication { get; set; }

		[JsonPropertyName("UserId")]
		public string UserId { get; set; }
	}

	public class RequestDataKw
	{
		[JsonPropertyName("DocumentId")]
		public long DocumentId { get; set; }

		[JsonPropertyName("Keywords")]
		public IDictionary<string, string> Keywords { get; set; }
	}
}