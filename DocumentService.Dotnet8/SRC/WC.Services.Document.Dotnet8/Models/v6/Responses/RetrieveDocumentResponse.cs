// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   WC.Services.Document
//   RequestDocumentResponse.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.Document.Dotnet8.Models.v6.Responses
{
	using System;
	using CareSource.WC.Entities.Exceptions;
	using CareSource.WC.Entities.Responses;

	public class RetrieveDocumentResponse : CareSource.WC.Entities.Responses.Base.BaseResponse<Document>
	{
		public RetrieveDocumentResponse(ResponseStatus status,
			string message,
			ErrorCode errorCode,
			Guid correlationGuid,
			Document responseData)
			: base(status,
				message,
				errorCode,
				correlationGuid,
				responseData)
		{ }
	}
}