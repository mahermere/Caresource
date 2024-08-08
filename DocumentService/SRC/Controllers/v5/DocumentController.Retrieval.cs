// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2022. All rights reserved.
// 
//   WC.Services.Document
//   DocumentController.Retrieval.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.Document.Controllers.v5
{
	using System;
	using System.Collections.Generic;
	using System.Net;
	using System.Web.Http;
	using CareSource.WC.Entities.Exceptions;
	using CareSource.WC.Entities.Responses;
	using CareSource.WC.OnBase.Core.ExtensionMethods;
	using CareSource.WC.Services.Document.Models.v5;

	public partial class DocumentController
	{
		/// <summary>
		///    Gets the document.
		/// </summary>
		/// <param name="documentId"></param>
		/// <param name="request">The request.</param>
		/// <returns>
		///    A base64 encoded string of the file bytes
		/// </returns>
		[HttpGet]
		[Route(
			"{documentId}",
			Name = "GetDocumentById")]
		public IHttpActionResult GetDocument(
			long documentId,
			[FromUri]
			DownloadRequest request)
		{
			string methodName = $"{nameof(DocumentController)}.{nameof(GetDocument)}";
			using (_logger.BeginScope(
					new Dictionary<string, string>
					{
						{ GlobalConstants.CorrelationGuid, request.CorrelationGuid.ToString() },
						{ GlobalConstants.Service, GlobalConstants.ServiceName }
					}
				))
			{
				try
				{
					_logger.LogInformation(
						$"Starting {methodName}",
						new Dictionary<string, object> { { "Request", request } },
						Request);

					request.DocumentId = documentId;
					// TODO: Add new 400(Bad Request) response instead of allowing it to continue.
					_documentManager.IsValid(
						request,
						ModelState);

					Document document = _documentManager.Download(request);

					RetrieveDocumentResponse response = new RetrieveDocumentResponse(
						ResponseStatus.Success,
						"Success",
						ErrorCode.Success,
						request.CorrelationGuid,
						document);

					_logger.LogInformation(
						$"Response for {methodName}",
						new Dictionary<string, object> { { "Response", response } });

					return Content(
						HttpStatusCode.OK,
						response);
				}
				catch (OnBaseExceptionBase exception)
				{
					_logger.LogError(
						exception,
						exception.Message);

					return Content(
						HttpStatusCode.BadRequest,
						new ExceptionResponse<int>(
							exception.Message,
							(int)ErrorCode.InvalidKeyword,
							request.CorrelationGuid,
							0
						));
				}
				catch (Exception exception)
				{
					_logger.LogError(
						exception,
						exception.Message);

					return Content(
						HttpStatusCode.InternalServerError,
						new ExceptionResponse<string>(
							"An unexpected error has occurred",
							(int)HttpStatusCode.InternalServerError,
							request.CorrelationGuid,
							"Please logs and reference the correlation GUID"));
				}
				finally
				{
					_logger.LogInformation($"Finished {methodName}");
				}
			}
		}
	}
}