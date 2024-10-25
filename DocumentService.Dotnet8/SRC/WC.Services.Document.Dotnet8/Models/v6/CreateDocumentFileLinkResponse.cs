// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   WC.Services.Document
//   CreateDocumentFileLinkResponse.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.Document.Dotnet8.Models.v6
{
	using System;
	using CareSource.WC.Entities.Exceptions;
	using CareSource.WC.Entities.Responses;
    using log4net.Core;

    public class CreateDocumentFileLinkResponse
		: CareSource.WC.Entities.Responses.Base.BaseResponse<Models.OnBaseDocument>
	{
		public CreateDocumentFileLinkResponse(
			ResponseStatus status,
			string message,
			CareSource.WC.Entities.Exceptions.ErrorCode errorCode,
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