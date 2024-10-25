// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2022. All rights reserved.
// 
//   WC.Services.Document
//   MemberController.Count.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Service.Document.Dotnet8.Controllers
{
	using System;
	using System.Collections.Generic;
	using System.Net;
	//using System.Web.Http;
	using CareSource.WC.Entities.Exceptions;
	using CareSource.WC.Entities.Responses;
    using Microsoft.AspNetCore.Mvc;
    using WC.Services.Document.Dotnet8;
    //using CareSource.WC.OnBase.Core.ExtensionMethods;
    using WC.Services.Document.Dotnet8.Models.v6;
    using WC.Services.Document.Dotnet8.Models.v6.Requests;
    using WC.Services.Document.Dotnet8.Models.v6.Responses;

    public partial class MemberController
	{
		/// <summary>
		///    The total Documents matching the request parameter, along with a breakdown by
		///    document type
		/// </summary>
		/// <param name="memberId"></param>
		/// <param name="request">The request.</param>
		/// <returns></returns>
		/// <exception cref="Exception"></exception>
	//	[HttpGet]
		[HttpPost("DocumentTypesCount/{memberId}")]
		public IActionResult DocumentTypesCount(
			string memberId,
			[FromBody]
			CountRequest request)
		{
			//throw new NotImplementedException();
			{
				string methodName = $"{nameof(MemberController)}.{nameof(DocumentTypesCount)}";
				try
				{
                    _logger.Info($"Request for {methodName}. Request details: {request}");

                    _memberManager.MemberId = memberId;

					if (!_memberManager.IsValid(
							request,
							ModelState))
					{
						return BadRequest(
							new ValidationProblemResponse(
								request?.CorrelationGuid ?? Guid.NewGuid(),
								ModelState));
					}

					(IDictionary<string, int>, long) docTypes =
						_memberManager.DocumentTypeCounts(request);

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
		/// <param name="memberId"></param>
		/// <param name="request">The request.</param>
		/// <returns>
		///    The total records matching the request parameter
		/// </returns>
		/// <exception cref="Exception"></exception>
	//	[HttpGet]
		[HttpPost("TotalRecordCount/{memberId}")]
		public IActionResult TotalRecordCount(
			string memberId,
			[FromBody]
			CountRequest request)
		{
			//throw new NotImplementedException();
			{
				string methodName = $"{nameof(MemberController)}.{nameof(TotalRecordCount)}";
				try
                {
                    _logger.Info($"Starting for {methodName}. Request details: {request}");

                    _memberManager.MemberId = memberId;

				if (!_memberManager.IsValid(
						request,
						ModelState))
				{
					return BadRequest(
						new ValidationProblemResponse(
							request?.CorrelationGuid ?? Guid.NewGuid(),
							ModelState));
				}

				long result = _memberManager.DocumentCount(request);

					TotalDocumentCountResponse response = new TotalDocumentCountResponse(
						SuccessMessage,
						(int)HttpStatusCode.OK,
						request.CorrelationGuid,
						result);

                    _logger.Info($"Response for {methodName}. Response details: {response}");

                    return Ok(response);
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