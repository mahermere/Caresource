// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2022. All rights reserved.
// 
//   WC.Services.Document
//   DocumentController.Search.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.Document.Controllers.v5
{
	using System;
	using System.Collections.Generic;
	using System.Net;
	using System.Web.Http;
	using CareSource.WC.Entities.Documents;
	using CareSource.WC.Entities.Exceptions;
	using CareSource.WC.Entities.Responses;
	using CareSource.WC.OnBase.Core.ExtensionMethods;
	using CareSource.WC.Services.Document.Models.v5;

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
		[HttpGet]
		[Route("Search")]
		public IHttpActionResult Search(
			[FromUri]
			SearchRequest request)
		{
			string methodName = $"{nameof(DocumentController)}.{nameof(Search)}";

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

					ISearchResult<DocumentHeader> documents = _documentManager.Search(request);

					SearchResponse response = new SearchResponse(
						ResponseStatus.Success,
						"Success",
						ErrorCode.Success,
						request.CorrelationGuid,
						documents.TotalRecordCount,
						documents.Documents);

					_logger.LogInformation(
						$"Response for {methodName}",
						new Dictionary<string, object> { { "Response", response } });

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
						HttpStatusCode.BadRequest,
						new ExceptionResponse<int>(
							e.Message,
							(int)HttpStatusCode.BadRequest,
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
	}
}