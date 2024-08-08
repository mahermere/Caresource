// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   WC.Services.Document
//   DocumentController.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.Document.Controllers.v3
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Net;
	using System.Web.Http;
	using CareSource.WC.OnBase.Core.ExtensionMethods;
	using CareSource.WC.Entities.Documents;
	using CareSource.WC.Services.Document.Models.v3;
	using Swashbuckle.Swagger.Annotations;

	/// <summary>
	/// Version 3 of the Document Controller, Keyword Functions
	/// </summary>
	/// <seealso cref="System.Web.Http.ApiController" />
	public partial class DocumentController
	{
		/// <summary>
		/// Updates the keywords.
		/// </summary>
		/// <param name="request">Array of document and their keywords that need updating</param>
		/// <returns>
		/// </returns>
		[HttpPut]
		[Route("keywords/update")]
		[Obsolete("Please use the V5 routes.")]
		[SwaggerResponse(HttpStatusCode.OK, Type = typeof(KeywordUpdateResponse))]
		[SwaggerResponse((HttpStatusCode)207, Type = typeof(ValidationProblemResponse))]
		[SwaggerResponse(HttpStatusCode.Unauthorized)]
		[SwaggerResponse(HttpStatusCode.InternalServerError)]
		public IHttpActionResult UpdateKeywords([FromBody] Models.BatchUpdateKeywordsRequest request)
		{
			using (_logger.BeginScope(
						new Dictionary<string, string>
						{
							{ GlobalConstants.CorrelationGuid, request.CorrelationGuid.ToString() },
							{ GlobalConstants.Service, GlobalConstants.ServiceName }
						}
					))
			{
				string methodName = $"{nameof(DocumentController)}.{nameof(UpdateKeywords)}";

				try
				{
					_logger.LogInformation(
						$"Starting {methodName}",
						new Dictionary<string, object> { { "Request", request } },
						Request);

					VerifyRequest(request);

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

					return Content(
						(HttpStatusCode)status,
						new KeywordUpdateResponse(
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
							}));

				}
				catch (Exception exception)
				{
					_logger.LogError(
						exception,
						exception.Message);

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
		/// Gets the keywords by document identifier.
		/// </summary>
		/// <param name="documentId">The document identifier.</param>
		/// <param name="request">The request.</param>
		/// <returns></returns>
		[HttpGet]
		[Route("keywords/{documentId}")]
		[Obsolete("Please use the V5 routes.")]
		[SwaggerResponse(HttpStatusCode.OK, Type = typeof(DocumentKeywordResponse))]
		[SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(ValidationProblemResponse))]
		[SwaggerResponse(HttpStatusCode.Unauthorized)]
		[SwaggerResponse(HttpStatusCode.InternalServerError)]
		public IHttpActionResult GetKeywordsByDocumentId(
			long documentId,
			[FromUri] DocumentKeywordRequest request)
		{

			using (_logger.BeginScope(
						new Dictionary<string, string>
						{
							{ GlobalConstants.CorrelationGuid, request.CorrelationGuid.ToString() },
							{ GlobalConstants.Service, GlobalConstants.ServiceName }
						}
					))
			{
				string methodName = $"{nameof(DocumentController)}.{nameof(GetKeywordsByDocumentId)}";

				try
				{
					_logger.LogInformation(
						$"Starting {methodName}",
						new Dictionary<string, object> { { "Request", request } },
						Request);
					VerifyRequest(request);

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

				_logger.LogInformation($"Successfully retrieved Keywords for Document Id [{ documentId}].");
				return Ok(
					new DocumentKeywordResponse(
						HttpStatusCode.OK,
						$"Successfully retrieved Keywords for Document Id [{documentId}].",
						request.CorrelationGuid,
						document));

			}
			catch (Exception exception)
			{
				_logger.LogError(
					exception,
					exception.Message,
					new Dictionary<string, object> {{"CorrelationGuid", request.CorrelationGuid}});

				return Content(
					HttpStatusCode.InternalServerError,
					new KeywordUpdateResponse(
						(int) HttpStatusCode.InternalServerError,
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