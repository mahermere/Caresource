// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2018. All rights reserved.
// 
//   CareSource.WC.Entities
//   GetProviderResponse.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Entities.Eligibility
{
    using System;
	using System.Collections.Generic;
    using CareSource.WC.Entities.Exceptions;
    using CareSource.WC.Entities.Responses;
    using CareSource.WC.Entities.Responses.Base;

    public class GetEligibilityResponse : BaseResponse<IEnumerable<Eligibility>>
	{
		public GetEligibilityResponse(
            ResponseStatus status,
			string message,
			ErrorCode errorCode,
			Guid correlationGuid,
			IEnumerable<Eligibility> responseData)
			: base(
				status,
				message,
				errorCode,
				correlationGuid,
				responseData)
		{ }
	}
}