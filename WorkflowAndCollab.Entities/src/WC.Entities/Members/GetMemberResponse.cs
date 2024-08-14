// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   CareSource.WC.Entities
//   GetMemberResponse.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Entities.Members
{
	using System;
    using CareSource.WC.Entities.Exceptions;
    using CareSource.WC.Entities.Responses;
	using CareSource.WC.Entities.Responses.Base;

	public class GetMemberResponse : BaseResponse<Member>
	{
		public GetMemberResponse(
			ResponseStatus status,
			string message,
			ErrorCode errorCode,
			Guid correlationGuid,
			Member responseData)
			: base(
				status,
				message,
				errorCode,
				correlationGuid,
				responseData)
		{ }
	}
}