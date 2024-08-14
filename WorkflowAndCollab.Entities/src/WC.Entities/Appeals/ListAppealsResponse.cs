namespace CareSource.WC.Entities.Appeals
{
    using CareSource.WC.Entities.Exceptions;
    using CareSource.WC.Entities.Responses;
    using CareSource.WC.Entities.Responses.Base;
    using System;
	using System.Collections.Generic;

	public class ListAppealsResponse : BasePagingResponse<IEnumerable<Appeal>>
	{
		public ListAppealsResponse(
			ResponseStatus status,
			string message,
			ErrorCode errorCode,
			Guid correlationGuid,
			int totalItems,
			IEnumerable<Appeal> responseData)
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