// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   CareSource.WC.Entities
//   ListDocumentsResponse.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Entities.Documents
{
	using System;
	using System.Collections.Generic;
    using CareSource.WC.Entities.Exceptions;
    using CareSource.WC.Entities.Responses;
	using CareSource.WC.Entities.Responses.Base;
	public class ListDocumentsResponse : BasePagingResponse<IEnumerable<DocumentHeader>>
	{
		public ListDocumentsResponse(
			ResponseStatus status,
			string message,
			ErrorCode errorCode,
			Guid correlationGuid,
			int totalItems,
			IEnumerable<DocumentHeader> responseData)
			: base(
				status,
				message,
				errorCode,
				correlationGuid,
				totalItems,
				responseData)
		{ }
	}
}