// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   CareSource.WC.Entities.WC.Entities
//   IBaseResponse.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

using CareSource.WC.Entities.Exceptions;

namespace CareSource.WC.Entities.Responses.Base.Interfaces
{

    public interface IBaseResponse<out TResponse>
	{
		/// <summary>
		///    Gets the error code.
		/// </summary>

		ErrorCode ErrorCode { get; }

		/// <summary>
		///    Gets the message.
		/// </summary>
		string Message { get; }

		TResponse ResponseData { get; }

		/// <summary>
		///    Gets the status.
		/// </summary>
		ResponseStatus Status { get; }
	}
}