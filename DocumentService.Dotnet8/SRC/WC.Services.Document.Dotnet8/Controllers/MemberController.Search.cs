// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2022. All rights reserved.
// 
//   WC.Services.Document
//   MemberController.Search.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Service.Document.Dotnet8.Controllers
{
	using System;
	using System.Collections.Generic;
	using System.Net;
	//using System.Web.Http;
	using CareSource.WC.Entities.Documents;
	using CareSource.WC.Entities.Exceptions;
	using CareSource.WC.Entities.Responses;
    using Microsoft.AspNetCore.Mvc;
    using WC.Services.Document.Dotnet8;
    //using CareSource.WC.OnBase.Core.ExtensionMethods;
    using WC.Services.Document.Dotnet8.Models.v6;
    using WC.Services.Document.Dotnet8.Models.v6.Interfaces;
    using WC.Services.Document.Dotnet8.Models.v6.Requests;
    using WC.Services.Document.Dotnet8.Models.v6.Responses;
    using DocumentHeader = WC.Services.Document.Dotnet8.Models.v6.DocumentHeader;

	/// <summary>
	///    Version 5 of the Document Controller to retrieve Provider document search's
	/// </summary>
	/// <seealso cref="System.Web.Http.ApiController" />
	public partial class MemberController
	{
		/// <summary>
		///    Gets the provider documents.
		/// </summary>
		/// <param name="memberId"></param>
		/// <param name="request">The request.</param>
		/// <returns></returns>
	//	[HttpGet]
		[HttpPost("Search/{memberId}")]
		public IActionResult Search(
			string memberId,
			[FromBody]
			SearchRequest request)
		{
			//throw new NotImplementedException();
			{
				string methodName = $"{nameof(MemberController)}.{nameof(Search)}";
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
								request.CorrelationGuid,
								ModelState));
					}

					ISearchResult<DocumentHeader> documents = _memberManager.Search(request);

					SearchResponse response = new SearchResponse(
						ResponseStatus.Success,
						"Success",
						ErrorCode.Success,
						request.CorrelationGuid,
						documents.SuccessRecordCount,
						documents.Documents);

                    _logger.Info($"Response  for {methodName}. response details: {response}");

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
	}
}