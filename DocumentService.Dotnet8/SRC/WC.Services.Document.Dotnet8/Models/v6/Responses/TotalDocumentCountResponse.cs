// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   WC.Services.Document
//   TotalDocumentCountResponse.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.Document.Dotnet8.Models.v6.Responses
{
	using System;
    using System.Text.Json.Serialization;
    
	public class TotalDocumentCountResponse : BaseResponse<long>
	{
		public TotalDocumentCountResponse(
			string title,
			int status,
			Guid correlationGuid,
			long result)
			: base(
				title,
				status,
				correlationGuid,
				result)
		{ }

		[JsonPropertyName("Result")]
		public override long ResponseData { get; set; }
	}
}