// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   WC.Services.Document
//   KeywordUpdate.cs
// </copyright>
// ------------------------------------------------------------------------------------------------
namespace CareSource.WC.Services.Document.Models.v6
{
	using System;
	using System.Net;

	/// <summary>
	/// 
	/// </summary>
	public class DocumentKeywordResponse : BaseResponse<Document>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="KeywordUpdateResponse"/> class.
		/// </summary>
		/// <param name="status">The status.</param>
		/// <param name="message">The message.</param>
		/// <param name="correlationGuid">The correlation unique identifier.</param>
		/// <param name="responseData">The response data.</param>
		public DocumentKeywordResponse(
			HttpStatusCode status,
			string message,
			Guid correlationGuid,
			Document responseData)
			: base(
				message,
				(int)status,
				correlationGuid,
				responseData)
		{ }
	}
}