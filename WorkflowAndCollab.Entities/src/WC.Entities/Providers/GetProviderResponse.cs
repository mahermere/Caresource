// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   Caresource.WC.Entities.WC.Entities
//   GetProviderResponse.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Entities.Providers
{
	using System;
    using CareSource.WC.Entities.Exceptions;
    using CareSource.WC.Entities.Responses;
	using CareSource.WC.Entities.Responses.Base;

	public class GetProviderResponse : BaseResponse<Provider>
	{
		public GetProviderResponse(
			ResponseStatus status,
			string message,
			ErrorCode errorCode,
			Guid correlationGuid,
			Provider responseData)
			: base(
				status,
				message,
				errorCode,
				correlationGuid,
				responseData)
		{ }
	}
}