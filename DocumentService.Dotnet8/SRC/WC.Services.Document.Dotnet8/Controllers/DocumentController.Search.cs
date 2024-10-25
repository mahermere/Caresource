// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2022. All rights reserved.
// 
//   WC.Services.Document
//   DocumentController.Search.cs
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
    //using WC.Services.Document.MVC.Dotnet8.Models.v6.Interfaces;
    using WC.Services.Document.Dotnet8.Models.v6.Requests;
    using WC.Services.Document.Dotnet8.Models.v6.Responses;
    using DocumentHeader = Services.Document.Dotnet8.Models.v6.DocumentHeader;

   

    /// <summary>
    ///    Version of the Document Controller, used to handle search's
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    public partial class DocumentController
	{
		/// <summary>
		///    Lets you search for documents
		/// </summary>
		/// <param name="request">The request.</param>
		/// <returns>
		///    The total records matching the request parameter.
		///    The paged list of documents paged based on the paging argument of the request
		///    Sorted per the sort argument of the request. Containing any requested display columns
		/// </returns>
		/// <exception cref="Exception"></exception>
		//[HttpGet]
		[HttpPost("Search")]
		public IActionResult Search(
			[FromBody]
			SearchRequest request)
		{
			//throw new NotImplementedException();
			string methodName = $"{nameof(DocumentController)}.{nameof(Search)}";

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

					ISearchResult<DocumentHeader> documents = _documentManager.Search(request);

					SearchResponse response = new SearchResponse(
						ResponseStatus.Success,
						"Success",
						ErrorCode.Success,
						request.CorrelationGuid,
						documents.SuccessRecordCount,
						documents.Documents);

                    _logger.Info($"request for {methodName}. Request details: {request}");

                    return Content(
						HttpStatusCode.OK.ToString(),
						response.ToString());
				}
				catch (OnBaseExceptionBase e)
				{
					_logger.Error(e.Message, e);
					
					return BadRequest(
						new ExceptionResponse<int>(
							e.Message,
							(int)HttpStatusCode.BadRequest,
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