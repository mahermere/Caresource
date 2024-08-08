// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   WC.Services.Document
//   TotalDocumentCountResponse.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.Document.Models.v6
{
	using System;
	using Newtonsoft.Json;

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

		[JsonProperty("Result")]
		public override long ResponseData { get; set; }
	}
}