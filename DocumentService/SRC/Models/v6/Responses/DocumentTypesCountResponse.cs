// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   WC.Services.Document
//   DocumentTypesCountResponse.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.Document.Models.v6
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Newtonsoft.Json;

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


		[JsonProperty("Result")]
		public override IDictionary<string, int> ResponseData { get; set; }

		public int TotalRecords => ResponseData.Sum(e => e.Value);
	}
}