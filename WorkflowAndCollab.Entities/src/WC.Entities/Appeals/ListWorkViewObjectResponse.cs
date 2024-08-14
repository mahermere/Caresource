using System;
using System.Collections.Generic;
using CareSource.WC.Entities.Exceptions;
using CareSource.WC.Entities.Responses;
using CareSource.WC.Entities.Responses.Base;

namespace CareSource.WC.Entities.Appeals
{
    public class ListWorkViewObjectsResponse : BasePagingResponse<IEnumerable<WorkViewObjectsHeader>>
	{
		public ListWorkViewObjectsResponse(
			ResponseStatus status,
			string message,
			ErrorCode errorCode,
			Guid correlationGuid,
			int totalItems,
			IEnumerable<WorkViewObjectsHeader> responseData)
			: base(status,
				message,
				errorCode,
				correlationGuid,
				totalItems,
				responseData)
		{ }


	}
}