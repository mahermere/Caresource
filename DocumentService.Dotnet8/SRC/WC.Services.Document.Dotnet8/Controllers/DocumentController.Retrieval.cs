// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2022. All rights reserved.
// 
//   WC.Services.Document
//   DocumentController.Retrieval.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Service.Document.Dotnet8.Controllers
{
	using System;
	using System.Collections.Generic;
	using System.Net;
	using CareSource.WC.Entities.Exceptions;
	using CareSource.WC.Entities.Responses;
    using Microsoft.AspNetCore.Mvc;
    using WC.Services.Document.Dotnet8;
    //using CareSource.WC.OnBase.Core.ExtensionMethods;
    using WC.Services.Document.Dotnet8.Models.v6;
    using WC.Services.Document.Dotnet8.Models.v6.Requests;
    using WC.Services.Document.Dotnet8.Models.v6.Responses;

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
		//[HttpGet]
		[HttpPost("GetDocument/{documentId}")]
		public IActionResult GetDocument(
			long documentId,
			[FromBody]
			DownloadRequest request)
		{
			//throw new NotImplementedException();
			string methodName = $"{nameof(DocumentController)}.{nameof(GetDocument)}";
			{
				try
				{
                    _logger.Info($"Request for {methodName}. Request details: {request}");

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

                    _logger.Info($"Response for {methodName}. Response details: {response}");

                    return StatusCode(StatusCodes.Status200OK, response);
				}
				catch (OnBaseExceptionBase exception)
				{
					_logger.Error(exception.Message, exception);
						
					return (IActionResult)Results.BadRequest(
						new ExceptionResponse<int>(
							exception.Message,
							(int)ErrorCode.InvalidKeyword,
							request.CorrelationGuid,
							0
						));
				}
				catch (Exception exception)
				{
                    _logger.Error(exception.Message, exception);

                    return BadRequest(
						new ExceptionResponse<string>(
							"An unexpected error has occurred",
							(int)HttpStatusCode.InternalServerError,
							request.CorrelationGuid,
							"Please logs and reference the correlation GUID"));
				}
				finally
				{
					_logger.Info($"Finished {methodName}");
				}
			}
		}
	}
}