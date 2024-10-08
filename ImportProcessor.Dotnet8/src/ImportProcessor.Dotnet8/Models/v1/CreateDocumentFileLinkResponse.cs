// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2021. All rights reserved.
// 
//   ImportProcessor
//   CreateDocumentFileLinkResponse.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.ImportProcessor.Dotnet8.Models.v1
{
	using System;
	using CareSource.WC.Entities.Exceptions;
	using CareSource.WC.Entities.Responses;
	using CareSource.WC.Entities.Responses.Base;

	public class CreateDocumentFileLinkResponse : BaseResponse<OnBaseDocument>
	{
		public CreateDocumentFileLinkResponse(
			ResponseStatus status,
			string message,
			ErrorCode errorCode,
			Guid correlationGuid,
			OnBaseDocument responseData)
			: base(
				status,
				message,
				errorCode,
				correlationGuid,
				responseData)
		{ }
	}
}