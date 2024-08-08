// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2021. All rights reserved.
// 
//   ImportProcessor
//   CreateDocumentFileLinkRequest.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace ImportProcessor.Models.v1
{
	using System;
	using System.Collections.Generic;
	using Newtonsoft.Json;

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
		[JsonProperty("RequestData")]
		public RequestData RequestData { get; set; }

		[JsonProperty("CorrelationGuid")]
		public Guid CorrelationGuid { get; set; }

		[JsonProperty("RequestDateTime")]
		public DateTimeOffset RequestDateTime { get; set; }

		[JsonProperty("SourceApplication")]
		public string SourceApplication { get; set; }

		[JsonProperty("UserId")]
		public string UserId { get; set; }
	}

	public class RequestData
	{
		[JsonProperty("FileName")]
		public string FileName { get; set; }

		[JsonProperty("DocumentType")]
		public string DocumentType { get; set; }

		[JsonProperty("Keywords")]
		public IDictionary<string, string> Keywords { get; set; }
	}
}