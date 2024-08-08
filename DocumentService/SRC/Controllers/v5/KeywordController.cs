// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2022. All rights reserved.
// 
//   WC.Services.Document
//   KeywordController.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.Document.Controllers.v5
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Net;
	using System.Web.Http;
	using CareSource.WC.OnBase.Core.ExtensionMethods;
	using CareSource.WC.OnBase.Core.Http.Filters;
	using CareSource.WC.Services.Document.Managers.v5;
	using CareSource.WC.Services.Document.Models.v5;
	using Microsoft.Extensions.Logging;
	using Microsoft.Web.Http;
	using Swashbuckle.Swagger.Annotations;

	/// <summary>
	///    Version 4 of the Document Controller
	/// </summary>
	[OnBaseAuthorizeFilter]
	[ApiVersion("5")]
	[RoutePrefix("api/v{version:apiVersion}/keywords")]
	public class KeywordController : ApiController
	{
		/// <inheritdoc />
		/// <summary>
		///    Initializes a new instance of the
		///    <see cref="T:CareSource.WC.Services.Document.Controllers.v5.DocumentController" /> class.
		/// </summary>
		/// <param name="keywordManager"></param>
		/// <param name="getDocumentManager"></param>
		/// <param name="logger"></param>
		public KeywordController(
			IKeywordManager keywordManager,
			IGetDocumentManager<Document> getDocumentManager,
			ILogger logger)
		{
			_keywordManager = keywordManager;
			_getDocumentManager = getDocumentManager;
			_logger = logger;
		}

		/// <summary>
		///    Success Message Document Controller option
		/// </summary>
		private const string SuccessMessage = "Success";

		private readonly IGetDocumentManager<Document> _getDocumentManager;
		private readonly IKeywordManager _keywordManager;

		private readonly ILogger _logger;

		/// <summary>
		///    Gets the keywords by document identifier.
		/// </summary>
		/// <param name="documentId">The document identifier.</param>
		/// <param name="request">The request.</param>
		/// <returns></returns>
		[HttpGet]
		[Route("{documentId}")]
		[SwaggerResponse(
			HttpStatusCode.OK,
			Type = typeof(DocumentKeywordResponse))]
		[SwaggerResponse(
			HttpStatusCode.BadRequest,
			Type = typeof(ValidationProblemResponse))]
		[SwaggerResponse(HttpStatusCode.Unauthorized)]
		[SwaggerResponse(HttpStatusCode.InternalServerError)]
		public IHttpActionResult GetKeywordsByDocumentId(
			long documentId,
			[FromUri]
			DocumentKeywordRequest request)
		{
			string methodName = $"{nameof(KeywordController)}.{nameof(GetKeywordsByDocumentId)}";
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

					if (!ModelState.IsValid)
					{
						return Content(
							HttpStatusCode.BadRequest,
							new ValidationProblemResponse(
								request.CorrelationGuid,
								ModelState));
					}

					Document document = _getDocumentManager.GetDocument(
						documentId,
						request.DisplayColumns);

					DocumentKeywordResponse response = new DocumentKeywordResponse(
						HttpStatusCode.OK,
						$"Successfully retrieved Keywords for Document Id [{documentId}].",
						request.CorrelationGuid,
						document);

					_logger.LogInformation(
						$"Response for {methodName}",
						new Dictionary<string, object> { { $"Response for {methodName}", response } });

					return Ok(
						response);
				}
				catch (Exception exception)
				{
					_logger.LogError(
						exception.Message,
						exception,
						new Dictionary<string, object>
						{
							{ "CorrelationGuid", request.CorrelationGuid }
						});

					return Content(
						HttpStatusCode.InternalServerError,
						new KeywordUpdateResponse(
							(int)HttpStatusCode.InternalServerError,
							"An Error has occurred, please see logs and reference the correlation" +
							$" GUID {request.CorrelationGuid}.",
							request.CorrelationGuid,
							null));
				}
				finally
				{
					_logger.LogInformation($"Finished {methodName}");
				}
			}
		}

		/// <summary>
		///    Updates the keywords.
		/// </summary>
		/// <param name="request">Array of document and their keywords that need updating</param>
		/// <returns>
		/// </returns>
		[HttpPut]
		[Route("update")]
		[SwaggerResponse(
			HttpStatusCode.OK,
			Type = typeof(KeywordUpdateResponse))]
		[SwaggerResponse(
			(HttpStatusCode)207,
			Type = typeof(ValidationProblemResponse))]
		[SwaggerResponse(HttpStatusCode.Unauthorized)]
		[SwaggerResponse(HttpStatusCode.InternalServerError)]
		public IHttpActionResult UpdateKeywords([FromBody] BatchUpdateKeywordsRequest request)
		{
			using (_logger.BeginScope(
						new Dictionary<string, string>
						{
							{ GlobalConstants.CorrelationGuid, request.CorrelationGuid.ToString() },
							{ GlobalConstants.Service, GlobalConstants.ServiceName }
						}
					))
			{
				string methodName = $"{nameof(KeywordController)}.{nameof(UpdateKeywords)}";
				
				try
				{
					_logger.LogInformation(
						$"Starting {methodName}",
						new Dictionary<string, object> { { "Request", request } },
						Request);

					if (!_keywordManager.ValidateRequest(
							request,
							ModelState))
					{
						return Content(
							HttpStatusCode.BadRequest,
							new ValidationProblemResponse(
								request.CorrelationGuid,
								ModelState));
					}

					(IEnumerable<long> successfulIds, IDictionary<long, IEnumerable<string>> errors)
						= _keywordManager.UpdateKeywords(request);

					string message = SuccessMessage;
					int status = (int)HttpStatusCode.OK;

					bool anySuccessfulIds = successfulIds.Any();

					bool anyErrors = errors.Any();
					if (anyErrors && anySuccessfulIds)
					{
						message = "Success with failures, see result data";
						status = 207;
					}
					else if (anyErrors && !anySuccessfulIds)
					{
						message = "Error(s), see response data";
						status = 207;
					}

					KeywordUpdateResponse response = new KeywordUpdateResponse(
						status,
						message,
						request.CorrelationGuid,
						new KeywordUpdateResult
						{
							SuccessfulIds = successfulIds,
							Failures = errors.Select(
								e => new Failure
								{
									Id = e.Key.ToString(),
									Messages = e.Value
								})
						});

					_logger.LogInformation(
						$"Response for {methodName}",
						new Dictionary<string, object> { { "Response", response } });

					return Content(
						(HttpStatusCode)status,
						response);
				}
				catch (Exception exception)
				{
					_logger.LogError(
						exception.Message,
						exception);

					return Content(
						HttpStatusCode.InternalServerError,
						new KeywordUpdateResponse(
							(int)HttpStatusCode.InternalServerError,
							"An Error has occurred, please see logs and reference the correlation" +
							$" GUID {request.CorrelationGuid}.",
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