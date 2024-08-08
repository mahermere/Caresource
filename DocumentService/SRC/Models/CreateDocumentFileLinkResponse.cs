namespace CareSource.WC.Services.Document.Models
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
			: base(status,
				message,
				errorCode,
				correlationGuid,
				responseData)
		{ }
	}
}