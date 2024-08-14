// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   CareSource.WC.Entities.WC.Entities
//   BaseResponse.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Entities.Responses.Base
{
	using System;
    using CareSource.WC.Entities.Exceptions;
    using CareSource.WC.Entities.Responses.Base.Interfaces;
	using Newtonsoft.Json;
	using Newtonsoft.Json.Converters;

	public class BaseResponse<TResponse> : IBaseResponse<TResponse>
	{
		public BaseResponse(
			ResponseStatus status,
			string message,
			ErrorCode errorCode,
			Guid correlationGuid,
			TResponse responseData)
		{
			Status = status;
			Message = message;
			ErrorCode = errorCode;
			CorrelationGuid = correlationGuid;
			ResponseData = responseData;
		}

		public Guid CorrelationGuid { get; }

		public ErrorCode ErrorCode { get; }

		public string Message { get; }

		public TResponse ResponseData { get; }

		[JsonConverter(typeof(StringEnumConverter))]
		public ResponseStatus Status { get; }
	}
}