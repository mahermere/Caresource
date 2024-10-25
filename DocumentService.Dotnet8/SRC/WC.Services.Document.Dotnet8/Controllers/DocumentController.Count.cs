// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2022. All rights reserved.
// 
//   WC.Services.Document
//   DocumentController.Count.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Service.Document.Dotnet8.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Net;
	using System.Text;
	using System.Net.Http;
    using CareSource.WC.Entities.Exceptions;
    using Microsoft.AspNetCore.Mvc;
    using Swashbuckle.AspNetCore.Annotations;
    using WC.Services.Document.Dotnet8;
    using WC.Services.Document.Dotnet8.Models.v6;
    using WC.Services.Document.Dotnet8.Models.v6.Requests;
    using WC.Services.Document.Dotnet8.Models.v6.Responses;
    using InvalidDocumentTypeException =
        CareSource.WC.Entities.Exceptions.InvalidDocumentTypeException;
    using InvalidKeywordException = CareSource.WC.Entities.Exceptions.InvalidKeywordException;
    using log4net;
	using Microsoft.AspNetCore.Mvc.ModelBinding;
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
		//[HttpGet]
		[HttpPost("DocumentTypesCount")]
		[SwaggerResponse(
			(int)HttpStatusCode.OK,
			"The total Documents matching the request parameter, along with a breakdown by document type",
			typeof(DocumentTypesCountResponse))]
		public IActionResult DocumentTypesCount(
			[FromBody]
			CountRequest request)
		{
			//throw new NotImplementedException();
			{
				string methodName = $"{nameof(DocumentController)}.{nameof(DocumentTypesCount)}";

				try
				{
                    _logger.Info($"Request for {methodName}. Request details: {request}");


                    if (!_documentManager.IsValid(
							request,
							ModelState))
					{
                        _logger.Error("Invalid Model State", new ValidationException());
                        _logger.Debug($"ModelState: {ModelState}");

                        return BadRequest(new ValidationProblemResponse(
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

                    _logger.Info($"Request for {methodName}. Request details: {request}");


                    return (IActionResult)Results.Ok(response);
				}
				catch (OnBaseExceptionBase e)
				{
					_logger.Error(e.Message, e);


                    var responses = Results.StatusCode((int)HttpStatusCode.BadRequest);
                    var exceptionResponse = new ExceptionResponse<int>(
                        e.Message,
                        (int)e.ErrorCode,
                        request.CorrelationGuid,
                        0
                    );
                    return (Microsoft.AspNetCore.Mvc.IActionResult)Results.Ok(exceptionResponse);
                }
				catch (Exception e)
				{
					_logger.Error(e.Message, e);

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

		/// <summary>
		///    Get the total records that match the parameters
		/// </summary>
		/// <param name="request">The request.</param>
		/// <returns>
		///    The total records matching the request parameter
		/// </returns>
		/// <exception cref="Exception"></exception>
		//[HttpGet]
		[HttpPost("TotalRecordCount")]
		public IActionResult TotalRecordCount(
			[FromBody]
			CountRequest request)
		{
			//throw new NotImplementedException();
			string methodName = $"{nameof(DocumentController)}.{nameof(TotalRecordCount)}";

			
			{
				try
				{
                    _logger.Info($"Request for {methodName}. Request details: {request}");

                    if (!_documentManager.IsValid(
							request,
							ModelState))
					{
						return BadRequest(
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

                    _logger.Info($"Request for {methodName}. Request details: {response}");


                    return Ok(response);
				}
				catch (Exception e)
					when (e is InvalidKeywordException || e is InvalidDocumentTypeException)
				{
					_logger.Error(e.Message, e);
						
					return BadRequest(
						new ExceptionResponse<int>(
							e.Message,
							(int)ErrorCode.InvalidKeyword,
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