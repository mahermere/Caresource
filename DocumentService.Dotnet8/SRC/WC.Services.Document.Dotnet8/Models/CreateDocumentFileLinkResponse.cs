namespace WC.Services.Document.Dotnet8.Models
{
	using System;
	using CareSource.WC.Entities.Exceptions;
	using CareSource.WC.Entities.Responses;
	using CareSource.WC.Entities.Responses.Base;
    using log4net.Core;
    using ErrorCode = CareSource.WC.Entities.Exceptions.ErrorCode;

    public class CreateDocumentFileLinkResponse : BaseResponse<OnBaseDocument>
	{
		public CreateDocumentFileLinkResponse(
			ResponseStatus status,
			string message,
			ErrorCode errorCode,
			Guid correlationGuid,
			OnBaseDocument responseData)
			: base(status,
				message,
				errorCode,
				correlationGuid,
				responseData)
		{ }
	}
}