// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   CareSource.WC.Entities
//   GetClaimResponse.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Entities.Claims
{
	using System;
    using CareSource.WC.Entities.Exceptions;
    using CareSource.WC.Entities.Responses;
	using CareSource.WC.Entities.Responses.Base;

	public class GetClaimResponse : BaseResponse<ClaimInfo>
	{
		public GetClaimResponse(
			ResponseStatus status,
			string message,
			ErrorCode errorCode,
			Guid correlationGuid,
			ClaimInfo responseData)
			: base(
				status,
				message,
				errorCode,
				correlationGuid,
				responseData)
		{ }
	}
}