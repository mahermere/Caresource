// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2022. All rights reserved.
// 
//   WC.Services.Document
//   KeywordController.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Service.Document.Dotnet8.Controllers
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Net;
	//using System.Web.Http;
	//using CareSource.WC.OnBase.Core.ExtensionMethods;
	//using CareSource.WC.OnBase.Core.Http.Filters;
	//using WC.Services.Document.MVC.Dotnet8.Managers;
	using WC.Services.Document.Dotnet8.Models.v6;
	using Microsoft.Extensions.Logging;
	//using Microsoft.Web.Http;
    using Microsoft.AspNetCore.Mvc;
    using Swashbuckle.AspNetCore.Annotations;
    using WC.Services.Document.Dotnet8;
    using WC.Services.Document.Dotnet8.Managers.v6;
    using WC.Services.Document.Dotnet8.Managers.v6.Interfaces;
    using WC.Services.Document.Dotnet8.Models.v6.Responses;
    using WC.Services.Document.Dotnet8.Models.v6.Requests;
    using Microsoft.AspNetCore.Authorization;
    using Hyland.Public;

    //using Swashbuckle.Swagger.Annotations;

    /// <summary>
    ///    Version 6 of the Document Controller
    /// </summary>
    [Authorize(Policy = "OnBaseAuthorization")]
	[Route("api/[controller]")]
    [ApiController]
    public class KeywordController : ControllerBase
	{
		/// <inheritdoc />
		/// <summary>
		///    Initializes a new instance of the
		///    <see cref="T:CareSource.WC.Services.Document.Controllers.v6.DocumentController" /> class.
		/// </summary>
		/// <param name="keywordManager"></param>
		/// <param name="getDocumentManager"></param>
		/// <param name="logger"></param>
		public KeywordController(
			IKeywordManager keywordManager,
			IGetDocumentManager<Document> getDocumentManager,
            log4net.ILog logger)
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

        private readonly log4net.ILog _logger;

        /// <summary>
        ///    Gets the keywords by document identifier.
        /// </summary>
        /// <param name="documentId">The document identifier.</param>
        /// <param name="request">The request.</param>
        /// <returns></returns>
      //  [HttpGet]
		[HttpPost("GetKeywordsByDocumentId/{documentId}")]
		[SwaggerResponse(
			(int)HttpStatusCode.OK,
			Type = typeof(DocumentKeywordResponse))]
		[SwaggerResponse(
			(int)HttpStatusCode.BadRequest,
			Type = typeof(ValidationProblemResponse))]
		[SwaggerResponse((int)HttpStatusCode.Unauthorized)]
		[SwaggerResponse((int)HttpStatusCode.InternalServerError)]
		public IActionResult GetKeywordsByDocumentId(
			long documentId,
			[FromBody]
			DocumentKeywordRequest request)
		{
			//throw new NotImplementedException();
			string methodName = $"{nameof(KeywordController)}.{nameof(GetKeywordsByDocumentId)}";
			{


				try
				{
                    _logger.Info($"Request for {methodName}. Request details: {request}");

                    if (!ModelState.IsValid)
					{
						return BadRequest(
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

                    _logger.Info($"Response for {methodName}. response details: {response}");
                    
					return Ok(
						response);
				}
				catch (Exception exception)
				{
                    _logger.Error($"Error: {exception.Message}, CorrelationGuid: {request.CorrelationGuid}", exception);

                    return BadRequest(
						new KeywordUpdateResponse(
							(int)HttpStatusCode.InternalServerError,
							"An Error has occurred, please see logs and reference the correlation" +
							$" GUID {request.CorrelationGuid}.",
							request.CorrelationGuid,
							null));
				}
				finally
				{
					_logger.Info($"Finished {methodName}");
				}
			}
		}

		/// <summary>
		///    Updates the keywords.
		/// </summary>
		/// <param name="request">Array of document and their keywords that need updating</param>
		/// <returns>
		/// </returns>
		//[HttpPut]
		[HttpPut("update")]
		[SwaggerResponse(
			(int)HttpStatusCode.OK,
			Type = typeof(KeywordUpdateResponse))]
		[SwaggerResponse(
			(int)(HttpStatusCode)207,
			Type = typeof(ValidationProblemResponse))]
		[SwaggerResponse((int)HttpStatusCode.Unauthorized)]
		[SwaggerResponse((int)HttpStatusCode.InternalServerError)]
		public IActionResult UpdateKeywords([FromBody] BatchUpdateKeywordsRequest request)
		{
			//throw new NotImplementedException();
			{
				string methodName = $"{nameof(KeywordController)}.{nameof(UpdateKeywords)}";
				
				try
				{
					_logger.Info($"Starting {methodName}, Request: {request}");
					
					if (!_keywordManager.ValidateRequest(
							request,
							ModelState))
					{
						return BadRequest(
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

                    _logger.Info($"Response for {methodName}. Response details: {response}");

                    return StatusCode((int)HttpStatusCode.MultiStatus, response);
                }
				catch (Exception exception)
				{
					_logger.Error(
						exception.Message,
						exception);

					return BadRequest(
						new KeywordUpdateResponse(
							(int)HttpStatusCode.InternalServerError,
							"An Error has occurred, please see logs and reference the correlation" +
							$" GUID {request.CorrelationGuid}.",
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