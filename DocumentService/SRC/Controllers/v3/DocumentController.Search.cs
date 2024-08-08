// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   WC.Services.Document
//   DocumentController.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.Document.Controllers.v3
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Net;
	using System.Threading.Tasks;
	using System.Web.Http;
	using CareSource.WC.Entities.Common.Interfaces;
	using CareSource.WC.Entities.Documents;
	using CareSource.WC.Entities.Exceptions;
	using CareSource.WC.Entities.Responses;
	using CareSource.WC.OnBase.Core.ExtensionMethods;
	using CareSource.WC.Services.Document.Models.v3;

	/// <summary>
	///    Version 3 of the Document Controller, used to handle search's
	/// </summary>
	/// <seealso cref="System.Web.Http.ApiController" />
	public partial class DocumentController
	{
		/// <summary>
		///    Searches the and get documents.
		/// </summary>
		/// <param name="request">The request.</param>
		/// <returns></returns>
		[HttpGet]
		[Route("search/retrieve")]
		[Obsolete("Please use the V5 routes.")]
		public async Task<IHttpActionResult> SearchAndGetDocuments(
			[FromUri]
			SearchRequest request)
		{
			string methodName = $"{nameof(DocumentController)}.{nameof(SearchAndGetDocuments)}";

				try
				{
					_logger.LogInformation(
						$"Starting {methodName}",
						new Dictionary<string, object> { { "Request", request } },
						Request);

					if (!VerifyRequest(request))
					{
						return
							Content(
								HttpStatusCode.BadRequest,
								new ValidationProblemResponse(
									request.CorrelationGuid,
									ModelState));
					}

					(IEnumerable<Document> documents, int count) documents
						= await _getDocumentManager.SearchAsync(request)
							.ConfigureAwait(false);

					_logger.LogInformation(
						"Successfully searched for Documents and returned " +
						$"{documents.count} documents.");

					return Content(
						HttpStatusCode.OK,
						new
						{
							Status = ResponseStatus.Success,
							Message = SuccessMessage,
							ErrorCode = ErrorCode.Success,
							request.CorrelationGuid,
							TotalRecords = documents.count,
							ResponseData = documents.documents.AsEnumerable()
						});
				}
				catch (OnBaseExceptionBase onBaseException)
				{
					_logger.LogError(
						onBaseException,
						onBaseException.Message);

					return Content(
						HttpStatusCode.OK,
						new
						{
							Status = ResponseStatus.Error,
							onBaseException.Message,
							onBaseException.ErrorCode,
							request.CorrelationGuid,
							TotalRecords = 0,
							ResponseData = (string)null
						});
				}
				catch (Exception exception)
				{
					_logger.LogError(
						exception,
						exception.Message);

					return
						Content(
							HttpStatusCode.InternalServerError,
							new BaseResponse<string>(
								"An unknown Error has occurred.",
								500,
								request.CorrelationGuid,
								null));
				}
				finally
				{
					_logger.LogInformation($"Finished {methodName}");
				}
			
		}

		/// <summary>
		///    Gets the member documents.
		/// </summary>
		/// <param name="request">The request.</param>
		/// <returns></returns>
		/// <exception cref="Exception"></exception>
		[HttpGet]
		[Route("search")]
		[Obsolete("Please use the V5 routes.")]
		public IHttpActionResult SearchDocuments(
			[FromUri]
			SearchRequest request)
		{
			using (_logger.BeginScope(
						new Dictionary<string, string>
						{
							{ GlobalConstants.CorrelationGuid, request.CorrelationGuid.ToString() },
							{ GlobalConstants.Service, GlobalConstants.ServiceName }
						}
					))
			{
				string methodName = $"{nameof(DocumentController)}.{nameof(SearchDocuments)}";

				try
				{
					_logger.LogInformation(
						$"Starting {methodName}",
						new Dictionary<string, object> { { "Request", request } },
						Request);

					if (!VerifyRequest(request))
					{
						throw new Exception(
							ModelState.Values.First(v => v.Errors.Any())
								.Errors.First()
								.ErrorMessage);
					}

					ISearchResults<DocumentHeader> documents = _documentManager.Search(request);

					_logger.LogInformation(
						"Successfully searched for Documents and returned " +
						$"{documents.TotalRecordCount} documents.");

					return Content(
						HttpStatusCode.OK,
						new ListDocumentsResponse(
							ResponseStatus.Success,
							SuccessMessage,
							ErrorCode.Success,
							request.CorrelationGuid,
							documents.TotalRecordCount,
							documents.Results));
				}
				catch (OnBaseExceptionBase onBaseException)
				{
					_logger.LogError(
						onBaseException,
						onBaseException.Message);

					return Content(
						HttpStatusCode.OK,
						new ListDocumentsResponse(
							ResponseStatus.Error,
							onBaseException.Message,
							onBaseException.ErrorCode,
							request.CorrelationGuid,
							0,
							null));
				}
				catch (Exception exception)
				{
					_logger.LogError(
						exception,
						exception.Message);

					return Content(
						HttpStatusCode.BadRequest,
						new ListDocumentsResponse(
							ResponseStatus.Error,
							exception.Message,
							ErrorCode.UnknownError,
							request.CorrelationGuid,
							0,
							null));
				}
				finally
				{
					_logger.LogInformation($"Finished {methodName}");
				}
			}
		}
	}
}