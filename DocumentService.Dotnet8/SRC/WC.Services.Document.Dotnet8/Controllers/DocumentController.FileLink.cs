// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2022. All rights reserved.
// 
//   WC.Services.Document
//   DocumentController.FileLink.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Service.Document.Dotnet8.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
	using CareSource.WC.Entities.Exceptions;
	using CareSource.WC.Entities.Responses;
	using WC.Services.Document.Dotnet8.Models.v6;
    using Microsoft.AspNetCore.Mvc;
    using Swashbuckle.AspNetCore.Annotations;
    using WC.Services.Document.Dotnet8;
    using WC.Services.Document.Dotnet8.Models.v6.Requests;
    using log4net;
    using InvalidKeywordException = Services.Document.Dotnet8.Models.v6.InvalidKeywordException;
    using InvalidDocumentTypeException = Services.Document.Dotnet8.Models.v6.InvalidDocumentTypeException;

    //using InvalidKeywordException = CareSource.WC.Entities.Exceptions.InvalidKeywordException;
    //using InvalidDocumentTypeException = CareSource.WC.Entities.Exceptions.InvalidDocumentTypeException;

    /// <summary>
    ///    Version 3 of the Document Controller, File upload Functions
    /// </summary>
    public partial class DocumentController
	{
		/// <summary>
		///    Uploads a Document form a share.
		/// </summary>
		/// <param name="request">The request.</param>
		/// <returns>
		///    OnBase Document
		/// </returns>
		/// <response code="200">Returns the created OnBase document.</response>
		/// <response code="400">Given if request parameters are not valid.</response>
		/// <response code="401">If the given authorization is not valid for OnBase.</response>
		/// <response code="500">If there is an uncaught server exception.</response>
		//[HttpPost]
		[HttpPost("create")]
		[SwaggerResponse((int)HttpStatusCode.Created)]
		[SwaggerResponse((int)HttpStatusCode.BadRequest)]
		[SwaggerResponse((int)HttpStatusCode.Unauthorized)]
		[SwaggerResponse((int)HttpStatusCode.InternalServerError)]
		public IActionResult Create([FromBody] CreateDocumentRequest request)
		{
			//throw new NotImplementedException();
			string methodName = $"{nameof(DocumentController)}.{nameof(Create)}";
			
			{
				try
				{

                    _logger.Info($"Request for {methodName}. Request details: {request}");

                    if (_createDocumentManager.IsValid(
							request,
							ModelState))
					{
						OnBaseDocument result = _createDocumentManager.CreateDocument(request);

						return StatusCode((int)HttpStatusCode.Created,
							new DownloadRequest
							{
								SourceApplication = request.SourceApplication,
								UserId = request.UserId,
								DocumentId = result.Id
							}
							);
					}

                    //throw new InvalidDataException();
                    return BadRequest(ModelState);
                }
				catch (Exception e)
					when (e is InvalidDataException)
				{
					return BadRequest(ModelState);
				}
				catch (Exception e)
					when (e is InvalidKeywordException)
				{
                    _logger.Info($"Request for {methodName}. Request details: {request}");

                    string[] messages = e.Message.Split(',');
					foreach (string message in messages)
					{
						ModelState.AddModelError(
							"Invalid Keyword",
							message.Trim());
					}

					return BadRequest(ModelState);
				}
				catch (Exception e)
					when (e is InvalidDocumentTypeException)
				{
                    _logger.Info($"Request for {methodName}. Request details: {request}");

                    string[] messages = e.Message.Split(',');
					foreach (string message in messages)
					{
						ModelState.AddModelError(
							"Invalid Document Type",
							message.Trim());
					}

					return BadRequest(ModelState);
				}

				catch (Exception e)
				{
                    _logger.Info($"Request for {methodName}. Request details: {request}");

                    throw;
				}
				finally
				{
					_logger.Info($"Finished {methodName}");
				}
			}
		}

		/// <summary>
		///    Uploads a Document form a share.
		/// </summary>
		/// <param name="request">The request.</param>
		/// <returns>
		///    OnBase Document
		/// </returns>
		/// <response code="200">Returns the created OnBase document.</response>
		/// <response code="400">Given if request parameters are not valid.</response>
		/// <response code="401">If the given authorization is not valid for OnBase.</response>
		/// <response code="500">If there is an uncaught server exception.</response>
		//[HttpPost]
		[HttpPost("filelink")]
		[SwaggerResponse((int)HttpStatusCode.OK)]
		[SwaggerResponse((int)HttpStatusCode.BadRequest)]
		[SwaggerResponse((int)HttpStatusCode.Unauthorized)]
		[SwaggerResponse((int)HttpStatusCode.InternalServerError)]
		public IActionResult Upload(
			[FromBody]
			CreateDocumentFileLinkRequest request)
		{
			//throw new NotImplementedException();
			string methodName = $"{nameof(DocumentController)}.{nameof(Upload)}";
			{
				try
				{
                    _logger.Info($"Request for {methodName}. Request details: {request}");

                    if ((request?.RequestData?.FileName?.LastIndexOf(
								".",
								StringComparison.InvariantCulture) ??
							0) <
						0)
					{
						ModelState.AddModelError(
							"RequestData.FileName",
							"File name must contain a file extension.");
					}

					if (ModelState.IsValid)
					{
						OnBaseDocument result = _createDocumentManager.CreateDocument(request);

						if (result == null)
						{
							return BadRequest(
								new CreateDocumentFileLinkResponse(
									ResponseStatus.Error,
									"Error retrieving document after storing file in OnBase.",
									ErrorCode.NoDocuments,
									request.CorrelationGuid,
									null));
						}

						return Ok(
							new CreateDocumentFileLinkResponse(
								ResponseStatus.Success,
								SuccessMessage,
								ErrorCode.Success,
								request.CorrelationGuid,
								result));
                    }

                    _logger.Info($"Bad Request - Model State: {ModelState}, Request: {request}");

                    return BadRequest(
						new CreateDocumentFileLinkResponse(
							ResponseStatus.Error,
							string.Join(
								" ",
								ModelState
									.SelectMany(
										m => m.Value.Errors
											.Select(e => e.ErrorMessage))
									.ToList()),
							ErrorCode.UnknownError,
							request.CorrelationGuid,
							null));
				}
				catch (Exception e)
					when (e is InvalidDataException)
				
				{
                    _logger.Error($"File length is 0bytes. : {e.Message}. Model State: {ModelState}, Request: {request}", e);
                   
                    return BadRequest(new CreateDocumentFileLinkResponse(
						ResponseStatus.Error,
						e.Message,
						ErrorCode.InvalidRequest,
						request.CorrelationGuid,
						null));
                }
				catch (Exception e)
					when (e is FileNotFoundException)

				{
                    _logger.Error($"File Not Found. : {e.Message}. Model State: {ModelState}, Request: {request}", e);
                    
                    return BadRequest(new CreateDocumentFileLinkResponse(
                        ResponseStatus.Error,
                        e.Message,
                        ErrorCode.InvalidRequest,
                        request.CorrelationGuid,
                        null));
                }
				catch (ArgumentException e)
				{
                    _logger.Error($"Argument Exception : {e.Message}. Model State: {ModelState}, Request: {request}", e);
            
					return BadRequest(new CreateDocumentFileLinkResponse(
						ResponseStatus.Error,
                            e.Message,
							ErrorCode.UnknownError,
							request.CorrelationGuid,
							null));
				}
				catch (Exception e)
				{
                    _logger.Error($"Unknown Exception : {e.Message}. Model State: {ModelState}, Request: {request}", e);

        			return BadRequest(
						new CreateDocumentFileLinkResponse(
							ResponseStatus.Error,
							e.Message,
							ErrorCode.UnknownError,
							request.CorrelationGuid,
							null));
				}
				finally
				{
					_logger.Info($"Finished {methodName}");
				}
			}
		}
	}
}