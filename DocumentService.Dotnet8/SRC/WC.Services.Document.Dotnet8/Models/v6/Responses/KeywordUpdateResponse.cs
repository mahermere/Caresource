// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   WC.Services.Document
//   KeywordUpdate.cs
// </copyright>
// ------------------------------------------------------------------------------------------------
namespace WC.Services.Document.Dotnet8.Models.v6.Responses
{
	using System;

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
		/// <param name="correlationGuid">The correlation unique identifier.</param>
		/// <param name="responseData">The response data.</param>
		public KeywordUpdateResponse(
			int status,
			string message,
			Guid correlationGuid,
			KeywordUpdateResult responseData)
			: base(
				message,
				status,
				correlationGuid,
				responseData)
		{ }
	}
}