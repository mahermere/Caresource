// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2022. All rights reserved.
// 
//   WC.Services.Document
//   ProviderController.Count.cs
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
	using CareSource.WC.Entities.Responses;
	using CareSource.WC.OnBase.Core.ExtensionMethods;
	using CareSource.WC.Services.Document.Models.v6;

	public partial class ProviderController
	{
		/// <summary>
		///    The total Documents matching the request parameter, along with a breakdown by
		///    document type
		/// </summary>
		/// <param name="providerId"></param>
		/// <param name="request">The request.</param>
		/// <returns></returns>
		/// <exception cref="Exception"></exception>
		[HttpGet]
		[Route("{providerId}/DocumentTypesCount")]
		public IHttpActionResult DocumentTypesCount(
			string providerId,
			[FromUri]
			CountRequest request)
		{
			throw  new NotImplementedException();
			using (_logger.BeginScope(
						new Dictionary<string, string>
						{
							{ GlobalConstants.CorrelationGuid, request.CorrelationGuid.ToString() },
							{ GlobalConstants.Service, GlobalConstants.ServiceName }
						}
					))
			{
				string methodName = $"{nameof(ProviderController)}.{nameof(DocumentTypesCount)}";

				try
				{
					_logger.LogInformation(
						$"Starting {methodName}",
						new Dictionary<string, object>
						{
							{ "Request", request }
						},
						Request);

					_providerManager.ProviderId = providerId;
					if (!_providerManager.IsValid(
							request,
							ModelState))
					{

						return Content(
							HttpStatusCode.BadRequest,
							new ValidationProblemResponse(
								request.CorrelationGuid,
								ModelState));
					}

					(IDictionary<string, int>, long) docTypes =
						_providerManager.DocumentTypeCounts(request);

					DocumentTypesCountResponse response = new DocumentTypesCountResponse(
						SuccessMessage,
						(int)HttpStatusCode.OK,
						request.CorrelationGuid,
						docTypes.Item1);

					_logger.LogInformation(
						$"Response for {methodName}",
						new Dictionary<string, object>
						{
							{
								"Response", response
							}
						});

					return Content(
						HttpStatusCode.OK,
						response);
				}
				catch (OnBaseExceptionBase e)
				{
					_logger.LogError(
						e,
						e.Message);

					return Content(
						HttpStatusCode.OK,
						new DocumentTypesCountResponse(
							e.Message,
							(int)ResponseStatus.Error,
							request.CorrelationGuid,
							new Dictionary<string, int>()));
				}
				catch (Exception e)
				{
					_logger.LogError(
						e,
						e.Message);

					return Content(
						HttpStatusCode.BadRequest,
						new TotalDocumentCountResponse(
							"An error occurred, please see logging and reference the Correlation Guid",
							(int)ResponseStatus.Error,
							request.CorrelationGuid,
							0));
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
		/// <param name="providerId"></param>
		/// <param name="request">The request.</param>
		/// <returns>
		///    The total records matching the request parameter
		/// </returns>
		/// <exception cref="Exception"></exception>
		[HttpGet]
		[Route("{providerId}/TotalRecordCount")]
		public IHttpActionResult TotalRecordCount(
			string providerId,
			[FromUri]
			CountRequest request)
		{
			throw new NotImplementedException();
			using (_logger.BeginScope(
				new Dictionary<string, string>
				{
					{ GlobalConstants.CorrelationGuid, request.CorrelationGuid.ToString() },
					{ GlobalConstants.Service, GlobalConstants.ServiceName }
				}))
			{
				string methodName = $"{nameof(ProviderController)}.{nameof(TotalRecordCount)}";
				try
				{
					_logger.LogInformation(
					$"Starting {methodName}",
					new Dictionary<string, object>
					{
						{ "Request", request }
					},
					Request);

					_providerManager.ProviderId = providerId;

					if (!_providerManager.IsValid(
							request,
							ModelState))
					{
						return Content(
							HttpStatusCode.BadRequest,
							new ValidationProblemResponse(
								request?.CorrelationGuid ?? Guid.NewGuid(),
								ModelState));
					}

					long result = _providerManager.DocumentCount(request);

					TotalDocumentCountResponse response = new TotalDocumentCountResponse(
						SuccessMessage,
						(int)HttpStatusCode.OK,
						request.CorrelationGuid,
						result);

					_logger.LogInformation(
						$"Response for {methodName}",
						new Dictionary<string, object>
						{
							{ "Response", response }
						});

					return Ok(
						response);
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