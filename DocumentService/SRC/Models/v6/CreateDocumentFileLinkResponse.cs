// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   WC.Services.Document
//   CreateDocumentFileLinkResponse.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.Document.Models.v6
{
	using System;
	using CareSource.WC.Entities.Exceptions;
	using CareSource.WC.Entities.Responses;

	public class CreateDocumentFileLinkResponse
		: Entities.Responses.Base.BaseResponse<Models.OnBaseDocument>
	{
		public CreateDocumentFileLinkResponse(
			ResponseStatus status,
			string message,
			ErrorCode errorCode,
			Guid correlationGuid,
			Models.OnBaseDocument responseData)
			: base(
				status,
				message,
				errorCode,
				correlationGuid,
				responseData)
		{ }
	}
}