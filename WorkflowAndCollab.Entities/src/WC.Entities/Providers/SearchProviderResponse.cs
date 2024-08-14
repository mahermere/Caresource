// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   Caresource.WC.Entities.WC.Entities
//   SearchProviderResponse.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Entities.Providers
{
	using System;
	using System.Collections.Generic;
    using CareSource.WC.Entities.Exceptions;
    using CareSource.WC.Entities.Responses;
	using CareSource.WC.Entities.Responses.Base;

	public class SearchProviderResponse : BaseResponse<IEnumerable<Provider>>
	{
		public SearchProviderResponse(
			ResponseStatus status,
			string message,
			ErrorCode errorCode,
			Guid correlationGuid,
			IEnumerable<Provider> responseData)
			: base(
				status,
				message,
				errorCode,
				correlationGuid,
				responseData)
		{ }
	}
}