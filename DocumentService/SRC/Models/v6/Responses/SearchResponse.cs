// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   WC.Services.Document
//   SearchResult.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.Document.Models.v6
{
	using System;
	using System.Collections.Generic;
	using CareSource.WC.Entities.Documents;
	using CareSource.WC.Entities.Exceptions;
	using CareSource.WC.Entities.Responses;

	public class SearchResponse : Entities.Responses.Base.BasePagingResponse<IEnumerable<DocumentHeader>>
	{
		public SearchResponse(ResponseStatus status,
			string message,
			ErrorCode errorCode,
			Guid correlationGuid,
			int totalItems,
			IEnumerable<DocumentHeader> responseData)
			: base(status,
				message,
				errorCode,
				correlationGuid,
				totalItems,
				responseData)
		{ }
	}
}