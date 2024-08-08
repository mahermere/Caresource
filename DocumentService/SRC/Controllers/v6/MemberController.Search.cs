// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2022. All rights reserved.
// 
//   WC.Services.Document
//   MemberController.Search.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.Document.Controllers.v6
{
	using System;
	using System.Collections.Generic;
	using System.Net;
	using System.Web.Http;
	using CareSource.WC.Entities.Documents;
	using CareSource.WC.Entities.Exceptions;
	using CareSource.WC.Entities.Responses;
	using CareSource.WC.OnBase.Core.ExtensionMethods;
	using CareSource.WC.Services.Document.Models.v6;
	using DocumentHeader = CareSource.WC.Services.Document.Models.v6.DocumentHeader;

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
		[HttpGet]
		[Route("")]
		public IHttpActionResult Search(
			string memberId,
			[FromUri]
			SearchRequest request)
		{
			throw new NotImplementedException();
			using (_logger.BeginScope(
						new Dictionary<string, string>
						{
							{ GlobalConstants.CorrelationGuid, request.CorrelationGuid.ToString() },
							{ GlobalConstants.Service, GlobalConstants.ServiceName }
						}))
			{
				string methodName = $"{nameof(MemberController)}.{nameof(Search)}";


				try
				{
					_logger.LogInformation(
						$"Starting {methodName}",
						new Dictionary<string, object> { { "Request", request } },
						Request);

					_memberManager.MemberId = memberId;

					if (!_memberManager.IsValid(
							request,
							ModelState))
					{
						return Content(
							HttpStatusCode.BadRequest,
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

					_logger.LogInformation(
						$"Response for {methodName}",
						new Dictionary<string, object>
						{
							{ "Response", response },
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
	}
}