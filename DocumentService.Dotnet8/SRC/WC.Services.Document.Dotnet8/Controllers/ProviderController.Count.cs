// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2022. All rights reserved.
// 
//   WC.Services.Document
//   ProviderController.Count.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Service.Document.Dotnet8.Controllers
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.Net;
	using CareSource.WC.Entities.Exceptions;
	using CareSource.WC.Entities.Responses;
    using Microsoft.AspNetCore.Mvc;
    using WC.Services.Document.Dotnet8;
    using WC.Services.Document.Dotnet8.Models.v6;
    using WC.Services.Document.Dotnet8.Models.v6.Requests;
    using WC.Services.Document.Dotnet8.Models.v6.Responses;

    //using CareSource.WC.OnBase.Core.ExtensionMethods;


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
	//	[HttpGet]
		[HttpPost("DocumentTypesCount/{providerId}")]
		public IActionResult DocumentTypesCount(
			string providerId,
			[FromBody]
			CountRequest request)
		{
			//throw  new NotImplementedException();
			{
				string methodName = $"{nameof(ProviderController)}.{nameof(DocumentTypesCount)}";

				try
				{
                    _logger.Info($"Starting for {methodName}. Request details: {request}");

                    _providerManager.ProviderId = providerId;
					if (!_providerManager.IsValid(
							request,
							ModelState))
					{

						return BadRequest(
							new Services.Document.Dotnet8.Models.v6.ValidationProblemResponse(
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

                    _logger.Info($"Response for {methodName}. Response details: {response}");

                    return Ok(response);
				}
				catch (OnBaseExceptionBase e)
				{
					_logger.Error(e.Message, e);
						
					return Ok(
						new DocumentTypesCountResponse(
							e.Message,
							(int)ResponseStatus.Error,
							request.CorrelationGuid,
							new Dictionary<string, int>()));
				}
				catch (Exception e)
				{
                    _logger.Error(e.Message, e);

                    return BadRequest(
						new TotalDocumentCountResponse(
							"An error occurred, please see logging and reference the Correlation Guid",
							(int)ResponseStatus.Error,
							request.CorrelationGuid,
							0));
				}
				finally
				{
					_logger.Info($"Finished {methodName}");
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
	//	[HttpGet]
		[HttpPost("TotalRecordCount/{providerId}")]
		public IActionResult TotalRecordCount(
			string providerId,
			[FromBody]
			CountRequest request)
		{
			//throw new NotImplementedException();
			{
				string methodName = $"{nameof(ProviderController)}.{nameof(TotalRecordCount)}";
				try
				{
                    _logger.Info($"Starting for {methodName}. Request details: {request}");

                    _providerManager.ProviderId = providerId;

					if (!_providerManager.IsValid(
							request,
							ModelState))
					{
						return BadRequest(
							new Services.Document.Dotnet8.Models.v6.ValidationProblemResponse(
								request?.CorrelationGuid ?? Guid.NewGuid(),
								ModelState));
					}

					long result = _providerManager.DocumentCount(request);

					TotalDocumentCountResponse response = new TotalDocumentCountResponse(
						SuccessMessage,
						(int)HttpStatusCode.OK,
						request.CorrelationGuid,
						result);

                    _logger.Info($"Response for {methodName}. Response details: {response}");

                    return Ok(
						response);
				}
				catch (OnBaseExceptionBase e)
				{
					_logger.Error(e.Message, e);
						
					return BadRequest(
						new ExceptionResponse<int>(
							e.Message,
							(int)e.ErrorCode,
							request.CorrelationGuid,
							0
						));
				}
				catch (Exception e)
				{
                    _logger.Error(e.Message, e);

                    return
						Content(
							HttpStatusCode.InternalServerError.ToString(),
							new ProblemResponse(request.CorrelationGuid).ToString());
				}
				finally
				{
					_logger.Info($"Finished {methodName}");
				}
			}
		}
	}
}