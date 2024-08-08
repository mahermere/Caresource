// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2022. All rights reserved.
// 
//   WC.Services.Document
//   DocumentController.FileLink.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.Document.Controllers.v6
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Net;
	using System.Web.Http;
	using CareSource.WC.Entities.Exceptions;
	using CareSource.WC.Entities.Responses;
	using CareSource.WC.OnBase.Core.ExtensionMethods;
	using CareSource.WC.Services.Document.Models.v6;
	using Swashbuckle.Swagger.Annotations;
	using InvalidDocumentTypeException =
		CareSource.WC.Services.Document.Models.v6.InvalidDocumentTypeException;
	using InvalidKeywordException =
		CareSource.WC.Services.Document.Models.v6.InvalidKeywordException;

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
		[HttpPost]
		[Route("create")]
		[SwaggerResponse(HttpStatusCode.Created)]
		[SwaggerResponse(HttpStatusCode.BadRequest)]
		[SwaggerResponse(HttpStatusCode.Unauthorized)]
		[SwaggerResponse(HttpStatusCode.InternalServerError)]
		public IHttpActionResult Create([FromBody] CreateDocumentRequest request)
		{
			throw new NotImplementedException();
			string methodName = $"{nameof(DocumentController)}.{nameof(Create)}";
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

					if (_createDocumentManager.IsValid(
							request,
							ModelState))
					{
						OnBaseDocument result = _createDocumentManager.CreateDocument(request);

						return CreatedAtRoute(
							"GetDocumentById",
							new DownloadRequest
							{
								SourceApplication = request.SourceApplication,
								UserId = request.UserId,
								DocumentId = result.Id
							},
							result.Id);
					}

					throw new InvalidDataException();
				}
				catch (Exception e)
					when (e is InvalidDataException)
				{
					return BadRequest(ModelState);
				}
				catch (Exception e)
					when (e is InvalidKeywordException)
				{
					_logger.LogError(
						e,
						e.Message,
						new Dictionary<string, object> { { "request", request } });

					string[] messages = e.Message.Split(',');
					foreach (string message in messages)
					{
						ModelState.AddModelError(
							"Invalid Keyword",
							message.SafeTrim());
					}

					return BadRequest(ModelState);
				}
				catch (Exception e)
					when (e is InvalidDocumentTypeException)
				{
					_logger.LogError(
						e,
						e.Message,
						new Dictionary<string, object> { { "request", request } });

					string[] messages = e.Message.Split(',');
					foreach (string message in messages)
					{
						ModelState.AddModelError(
							"Invalid Document Type",
							message.SafeTrim());
					}

					return BadRequest(ModelState);
				}

				catch (Exception e)
				{
					_logger.LogError(
						e,
						e.Message,
						new Dictionary<string, object> { { "request", request } });

					throw;
				}
				finally
				{
					_logger.LogInformation($"Finished {methodName}");
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
		[HttpPost]
		[Route("filelink")]
		[SwaggerResponse(HttpStatusCode.OK)]
		[SwaggerResponse(HttpStatusCode.BadRequest)]
		[SwaggerResponse(HttpStatusCode.Unauthorized)]
		[SwaggerResponse(HttpStatusCode.InternalServerError)]
		public IHttpActionResult Upload(
			[FromBody]
			CreateDocumentFileLinkRequest request)
		{
			throw new NotImplementedException();
			string methodName = $"{nameof(DocumentController)}.{nameof(Upload)}";
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
							return Content(
								HttpStatusCode.BadRequest,
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

					_logger.LogInformation(
						"Bad Request",
						new Dictionary<string, object>
						{
							{ "Model State", ModelState },
							{ "Request", request }
						});

					return Content(
						HttpStatusCode.BadRequest,
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
					_logger.LogError(
						e,
						"File length is 0bytes. ",
						new Dictionary<string, object>
						{
							{ "Model State", ModelState },
							{ "Request", request }
						});

					return Content(
						HttpStatusCode.BadRequest,
						new CreateDocumentFileLinkResponse(
							ResponseStatus.Error,
							e.Message,
							ErrorCode.InvalidRequest,
							request.CorrelationGuid,
							null));
				}
				catch (Exception e)
					when (e is FileNotFoundException)

				{
					_logger.LogError(
						e,
						"File Not Found",
						new Dictionary<string, object>
						{
							{ "Model State", ModelState },
							{ "Request", request }
						});

					return Content(
						HttpStatusCode.BadRequest,
						new CreateDocumentFileLinkResponse(
							ResponseStatus.Error,
							e.Message,
							ErrorCode.InvalidRequest,
							request.CorrelationGuid,
							null));
				}
				catch (ArgumentException e)
				{
					_logger.LogError(
						e,
						"Argument Exception",
						new Dictionary<string, object>
						{
							{ "Model State", ModelState },
							{ "Request", request }
						});

					return Content(
						HttpStatusCode.BadRequest,
						new CreateDocumentFileLinkResponse(
							ResponseStatus.Error,
							e.Message,
							ErrorCode.UnknownError,
							request.CorrelationGuid,
							null));
				}
				catch (Exception e)
				{
					_logger.LogError(
						e,
						"Unknown Exception",
						new Dictionary<string, object>
						{
							{ "Model State", ModelState },
							{ "Request", request }
						});

					return Content(
						HttpStatusCode.InternalServerError,
						new CreateDocumentFileLinkResponse(
							ResponseStatus.Error,
							e.Message,
							ErrorCode.UnknownError,
							request.CorrelationGuid,
							null));
				}
				finally
				{
					_logger.LogInformation($"Finished {methodName}");
				}
			}
		}
	}
}