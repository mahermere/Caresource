// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2021. All rights reserved.
// 
//   ImportProcessor
//   CreateDocumentFileLinkRequest.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.ImportProcessor.Dotnet8.Models.v1
{
	using System;
	using System.Collections.Generic;
	using System.Text.Json.Serialization;

	//public class CreateDocumentFileLink
	//{
	//	[Required]
	//	public string FileName { get; set; }

	//	[Required]
	//	public string DocumentType { get; set; }

	//	[Required]
	//	public IDictionary<string, string> Keywords { get; set; }
	//}


	public class CreateDocumentFileLinkRequest
	{
		[JsonPropertyName("RequestData")]
		public RequestData RequestData { get; set; }

		[JsonPropertyName("CorrelationGuid")]
		public Guid CorrelationGuid { get; set; }

		[JsonPropertyName("RequestDateTime")]
		public DateTimeOffset RequestDateTime { get; set; }

		[JsonPropertyName("SourceApplication")]
		public string SourceApplication { get; set; }

		[JsonPropertyName("UserId")]
		public string UserId { get; set; }
	}

	public class RequestData
	{
		[JsonPropertyName("FileName")]
		public string FileName { get; set; }

		[JsonPropertyName("DocumentType")]
		public string DocumentType { get; set; }

		[JsonPropertyName("Keywords")]
		public IDictionary<string, string> Keywords { get; set; }
	}
}