// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   WC.Services.Document
//   DocumentController.FileLink.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.Document.Controllers.v3
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Net;
	using System.Web.Http;
	using CareSource.WC.Entities.Exceptions;
	using CareSource.WC.Entities.Responses;
	using CareSource.WC.Services.Document.Models.v3;
	using CareSource.WC.OnBase.Core.ExtensionMethods;
	using Swashbuckle.Swagger.Annotations;

	/// <summary>
	///    Version 3 of the Document Controller, File upload Functions
	/// </summary>
	public partial class DocumentController
	{
		/// <summary>
		///    Success Message Document Controller option
		/// </summary>
		private const string SuccessMessage = "Success";

		/// <summary>
		///    Gets the document.
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
		[Obsolete("Please use the V5 routes.")]
		public IHttpActionResult Post(
			[FromBody]
			CreateDocumentFileLinkRequest request)
		{
			using (_logger.BeginScope(
						new Dictionary<string, string>
						{
							{ GlobalConstants.CorrelationGuid, request.CorrelationGuid.ToString() },
							{ GlobalConstants.Service, GlobalConstants.ServiceName }
						}
					))
			{
				string methodName = $"{nameof(DocumentController)}.{nameof(Post)}";

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
				catch (FileNotFoundException e)
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