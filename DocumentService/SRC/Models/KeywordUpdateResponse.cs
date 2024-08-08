// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   WC.Services.Document
//   KeywordUpdate.cs
// </copyright>
// ------------------------------------------------------------------------------------------------
namespace CareSource.WC.Services.Document.Models
{
	using System;
	using System.Collections.Generic;
	using CareSource.WC.Entities.Exceptions;
	using CareSource.WC.Entities.Responses;
	using CareSource.WC.Entities.Responses.Base;

	/// <summary>
	/// 
	/// </summary>
	public class KeywordUpdateResponse : BaseResponse<KeywordUpdateResult>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="KeywordUpdateResponse"/> class.
		/// </summary>
		/// <param name="status">The status.</param>
		/// <param name="message">The message.</param>
		/// <param name="errorCode">The error code.</param>
		/// <param name="correlationGuid">The correlation unique identifier.</param>
		/// <param name="responseData">The response data.</param>
		public KeywordUpdateResponse(
			ResponseStatus status,
			string message,
			ErrorCode errorCode,
			Guid correlationGuid,
			KeywordUpdateResult responseData)
			: base(status,
				message,
				errorCode,
				correlationGuid,
				responseData)
		{ }
	}

	public class KeywordUpdateResult
	{
		public IEnumerable<long> SuccessfulIds { get; set; }
		public IEnumerable<string> Failures { get; set; }
	}
}