// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   CareSource.WC.Entities.WC.Entities
//   BasePagingResponse.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Entities.Responses.Base
{
	using System;
    using CareSource.WC.Entities.Exceptions;
    using CareSource.WC.Entities.Responses.Base.Interfaces;

	public class BasePagingResponse<TResponse> : BaseResponse<TResponse>,
        IBasePagingResponse<TResponse>
	{
		public BasePagingResponse(
			ResponseStatus status,
			string message,
			ErrorCode errorCode,
			Guid correlationGuid,
			int totalItems,
			TResponse responseData)
			: base(
				status,
				message,
				errorCode,
				correlationGuid,
				responseData)
		{
			TotalItems = totalItems;
		}

		/// <summary>
		///    Gets the total items of the base paging response{ t response} class.
		/// </summary>
		public int TotalItems { get; }
	}
}