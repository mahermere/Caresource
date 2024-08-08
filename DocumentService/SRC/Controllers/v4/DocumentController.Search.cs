// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   WC.Services.Document
//   DocumentController.Search.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.Document.Controllers.v4
{
	using System;
	using System.Linq;
	using System.Net;
	using System.Web.Http;
	using CareSource.WC.Entities.Common.Interfaces;
	using CareSource.WC.Entities.Documents;
	using CareSource.WC.Entities.Exceptions;
	using CareSource.WC.Entities.Responses;
	using CareSource.WC.Services.Document.Models.v4;
	using CareSource.WC.OnBase.Core.ExtensionMethods;

	/// <summary>
	///    Version 4 of the Document Controller, used to handle search's
	/// </summary>
	/// <seealso cref="System.Web.Http.ApiController" />
	public partial class DocumentController
	{
		/// <summary>
		///    Lets you search for documents 
		/// </summary>
		/// <param name="request">The request.</param>
		/// <returns></returns>
		/// <exception cref="Exception"></exception>
		[HttpGet]
		[Route("search")]
		[Obsolete("Please use the V5 routes.")]
		public IHttpActionResult SearchDocuments(
			[FromUri] ListDocumentsRequest request)
		{
			try
			{
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
		}

		/// <summary>
		///   Get the total records that match the parameters
		/// </summary>
		/// <param name="request">The request.</param>
		/// <returns></returns>
		/// <exception cref="Exception"></exception>
		[HttpGet]
		[Route("TotalRecordCount")]
		[Obsolete("Please use the V5 routes.")]
		public IHttpActionResult TotalRecordCount(
			[FromUri] TotalDocumentCountRequest request)
		{
			try
			{
				if (!VerifyRequest(request))
				{
					throw new Exception(
						ModelState.Values.First(v => v.Errors.Any())
							.Errors.First()
							.ErrorMessage);
				}

				long count = _documentManager.GetTotalRecords(request);

				_logger.LogInformation($"{count} documents match this query.");

				return Content(
					HttpStatusCode.OK,
					new TotalDocumentCountResponse(
						SuccessMessage,
						(int)HttpStatusCode.OK,
						request.CorrelationGuid,
						count));
			}
			catch (OnBaseExceptionBase onBaseException)
			{
				_logger.LogError(
					onBaseException,
					onBaseException.Message);
				return Content(
					HttpStatusCode.OK,
					new TotalDocumentCountResponse(
						onBaseException.Message,
						(int)ResponseStatus.Error,
						request.CorrelationGuid,
						0));
			}
			catch (Exception exception)
			{
				_logger.LogError(
					exception,
					exception.Message);

				return Content(
					HttpStatusCode.BadRequest,
					new TotalDocumentCountResponse(
						"An error occurred, please see logging and reference the Correlation Guid",
						(int)ResponseStatus.Error,
						request.CorrelationGuid,
						0));
			}
		}


		/// <summary>
		///    Gets the document.
		/// </summary>
		/// <param name="documentId">The document identifier.</param>
		/// <param name="request">The request.</param>
		/// <returns>
		///    A base64 encoded string of the file bytes
		/// </returns>
		[HttpGet]
		[Route("{documentId}")]
		[Obsolete("Please use the V5 routes.")]
		public IHttpActionResult GetDocument(
			long documentId,
			[FromUri] GetDocumentRequest request)
		{
			try
			{
				VerifyRequest(request);

				Models.v4.Document document = _getDocumentManager.GetDocument(
					documentId,
					request);

				_logger.LogInformation($"Successfully retrieved Document [{documentId}].");

				return Content(
					HttpStatusCode.OK,
					new GetDocumentResponse(
						ResponseStatus.Success,
						SuccessMessage,
						ErrorCode.Success,
						request.CorrelationGuid,
						document));
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
		}
	}
}