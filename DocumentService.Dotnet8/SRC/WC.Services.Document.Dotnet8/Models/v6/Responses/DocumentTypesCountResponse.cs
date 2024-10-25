// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   WC.Services.Document
//   DocumentTypesCountResponse.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.Document.Dotnet8.Models.v6.Responses
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text.Json.Serialization;

	public class DocumentTypesCountResponse : BaseResponse<IDictionary<string, int>>
	{
		public DocumentTypesCountResponse(
			string title,
			int status,
			Guid correlationGuid,
			IDictionary<string, int> result)
			: base(
				title,
				status,
				correlationGuid,
				result)
		{ }

		public DocumentTypesCountResponse(
			string title,
			int status,
			Guid correlationGuid)
			: base(
				title,
				status,
				correlationGuid)
		{ }


		[JsonPropertyName("Result")]
		public override IDictionary<string, int> ResponseData { get; set; }

		public int TotalRecords => ResponseData.Sum(e => e.Value);
	}
}