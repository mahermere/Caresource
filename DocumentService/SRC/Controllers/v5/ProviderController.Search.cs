// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2022. All rights reserved.
// 
//   WC.Services.Document
//   ProviderController.Search.cs
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
	///    Version 5 of the Document Controller to retrieve Provider document search's
	/// </summary>
	/// <seealso cref="System.Web.Http.ApiController" />
	public partial class ProviderController
	{
		/// <summary>
		///    Gets the provider documents.
		/// </summary>
		/// <param name="providerId">The provider identifier.</param>
		/// <param name="request">The request.</param>
		/// <returns></returns>
		[HttpGet]
		[Route("{providerId}")]
		public IHttpActionResult ProviderDocuments(
			string providerId,
			[FromUri]
			SearchRequest request)
		{
			using (_logger.BeginScope(
						new Dictionary<string, string>
						{
							{ GlobalConstants.CorrelationGuid, request.CorrelationGuid.ToString() },
							{ GlobalConstants.Service, GlobalConstants.ServiceName }
						}))
			{
				string methodName = $"{nameof(ProviderController)}.{nameof(ProviderDocuments)}";

				try
				{
					_logger.LogInformation(
						$"Starting {methodName}",
						new Dictionary<string, object> { { "Request", request } },
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

					ISearchResult<DocumentHeader> documents = _providerManager.Search(request);

					SearchResponse response = new SearchResponse(
						ResponseStatus.Success,
						"Success",
						ErrorCode.Success,
						request.CorrelationGuid,
						documents.TotalRecordCount,
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

		/// <summary>
		///    Gets the provider documents.
		/// </summary>
		/// <param name="providerTin">The provider identifier.</param>
		/// <param name="request">The request.</param>
		/// <returns></returns>
		[HttpGet]
		[Route("tin/{providerTin}")]
		public IHttpActionResult ProviderDocumentsByTin(
			string providerTin,
			[FromUri]
			SearchRequest request)
		{
			using (_logger.BeginScope(
						new Dictionary<string, string>
						{
							{ GlobalConstants.CorrelationGuid, request.CorrelationGuid.ToString() },
							{ GlobalConstants.Service, GlobalConstants.ServiceName }
						}))
			{
				string methodName = $"{nameof(ProviderController)}.{nameof(ProviderDocumentsByTin)}";

				try
				{
					_logger.LogInformation(
						$"Starting {methodName}",
						new Dictionary<string, object> { { "Request", request } },
						Request);

					_providerManager.ProviderTin = providerTin;
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

					ISearchResult<DocumentHeader> documents = _providerManager.Search(request);

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