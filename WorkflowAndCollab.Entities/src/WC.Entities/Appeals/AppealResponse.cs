using System;
using System.Collections.Generic;
using System.Text;

namespace CareSource.WC.Entities.Appeals
{
	using CareSource.WC.Entities.Exceptions;
	using CareSource.WC.Entities.Responses;
	using CareSource.WC.Entities.Responses.Base;

	public class AppealResponse : BasePagingResponse<Appeal>
	{
		public AppealResponse(
			ResponseStatus status,
			string message,
			ErrorCode errorCode,
			Guid correlationGuid,
			int totalItems,
			Appeal responseData)
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
