// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2022. All rights reserved.
// 
//   WC.Services.Document
//   DocumentController.Count.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.Document.Controllers.v6
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.Net;
	using System.Web.Http;
	using CareSource.WC.Entities.Exceptions;
	using CareSource.WC.OnBase.Core.ExtensionMethods;
	using CareSource.WC.Services.Document.Models.v6;
	using Swashbuckle.Swagger.Annotations;
	using InvalidDocumentTypeException =
		CareSource.WC.Entities.Exceptions.InvalidDocumentTypeException;
	using InvalidKeywordException = CareSource.WC.Entities.Exceptions.InvalidKeywordException;

	public partial class DocumentController
	{
		/// <summary>
		///    Get the total records that match the parameters
		/// </summary>
		/// <param name="request">The request.</param>
		/// <remarks>
		///    The total Documents matching the request parameter, along with a breakdown by
		///    document type
		/// </remarks>
		/// <exception cref="Exception"></exception>
		[HttpGet]
		[Route("DocumentTypesCount")]
		[SwaggerResponse(
			HttpStatusCode.OK,
			"The total Documents matching the request parameter, along with a breakdown by document type",
			typeof(DocumentTypesCountResponse))]
		public IHttpActionResult DocumentTypesCount(
			[FromUri]
			CountRequest request)
		{
			throw new NotImplementedException();
			using (_logger.BeginScope(
						new Dictionary<string, string>
						{
							{ GlobalConstants.CorrelationGuid, request.CorrelationGuid.ToString() },
							{ GlobalConstants.Service, GlobalConstants.ServiceName }
						}
					))
			{
				string methodName = $"{nameof(DocumentController)}.{nameof(DocumentTypesCount)}";

				try
				{
					_logger.LogInformation(
						$"Starting {methodName}",
						new Dictionary<string, object> { { "Request", request } },
						Request);

					if (!_documentManager.IsValid(
							request,
							ModelState))
					{
						_logger.LogError(
							new ValidationException(),
							"Invalid Model State",
							new Dictionary<string, object> { { "ModelState", ModelState } });

						return Content(
							HttpStatusCode.BadRequest,
							new ValidationProblemResponse(
								request?.CorrelationGuid ?? Guid.NewGuid(),
								ModelState));
					}

					(IDictionary<string, int>, long) result =
						_documentManager.DocumentTypeCounts(request);

					DocumentTypesCountResponse response
						= new DocumentTypesCountResponse(
							SuccessMessage,
							(int)HttpStatusCode.OK,
							request.CorrelationGuid,
							result.Item1);

					_logger.LogInformation(
						$"Response for {methodName}",
						new Dictionary<string, object> { { "Response", response } });

					return Ok(response);
				}
				catch (OnBaseExceptionBase e)
				{
					_logger.LogError(
						e,
						e.Message);

					return Content(
						HttpStatusCode.BadRequest,
						new ExceptionResponse<int>(
							e.Message,
							(int)e.ErrorCode,
							request.CorrelationGuid,
							0
						));
				}
				catch (Exception e)
				{
					_logger.LogError(
						e,
						e.Message);

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

		/// <summary>
		///    Get the total records that match the parameters
		/// </summary>
		/// <param name="request">The request.</param>
		/// <returns>
		///    The total records matching the request parameter
		/// </returns>
		/// <exception cref="Exception"></exception>
		[HttpGet]
		[Route("TotalRecordCount")]
		public IHttpActionResult TotalRecordCount(
			[FromUri]
			CountRequest request)
		{
			throw new NotImplementedException();
			string methodName = $"{nameof(DocumentController)}.{nameof(TotalRecordCount)}";

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

					if (!_documentManager.IsValid(
							request,
							ModelState))
					{
						return Content(
							HttpStatusCode.BadRequest,
							new ValidationProblemResponse(
								request.CorrelationGuid,
								ModelState));
					}


					long result = _documentManager.DocumentCount(request);

					TotalDocumentCountResponse response = new TotalDocumentCountResponse(
						SuccessMessage,
						(int)HttpStatusCode.OK,
						request.CorrelationGuid,
						result);

					_logger.LogInformation(
						$"Response for {methodName}",
						new Dictionary<string, object> { { "Response", response } });

					return Ok(response);
				}
				catch (Exception e)
					when (e is InvalidKeywordException || e is InvalidDocumentTypeException)
				{
					_logger.LogError(
						e,
						e.Message);

					return Content(
						HttpStatusCode.BadRequest,
						new ExceptionResponse<int>(
							e.Message,
							(int)ErrorCode.InvalidKeyword,
							request.CorrelationGuid,
							0
						));
				}
				catch (Exception e)
				{
					_logger.LogError(
						e,
						e.Message);

					return
						Content(
							HttpStatusCode.InternalServerError,
							new ProblemResponse(request.CorrelationGuid));
				}
				finally
				{
					_logger.LogInformation($"Finished {methodName}");
				}
			}
		}
	}
}