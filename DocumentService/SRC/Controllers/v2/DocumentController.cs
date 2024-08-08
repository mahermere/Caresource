// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   WC.Services.Document
//   DocumentController.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.Document.Controllers.v2
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
	using CareSource.WC.Entities.Requests.Base;
	using CareSource.WC.Entities.Responses;
	using Microsoft.Extensions.Logging;
	using CareSource.WC.OnBase.Core.ExtensionMethods;
	using CareSource.WC.OnBase.Core.Http.Filters;
	using CareSource.WC.Services.Document.Managers;
	using CareSource.WC.Services.Document.Models;
	using Microsoft.Web.Http;
	using Swashbuckle.Swagger.Annotations;

	/// <summary>
	///    Version 2 of the Document Controller
	/// </summary>
	[OnBaseAuthorizeFilter]
	[ApiVersion("2", Deprecated = true)]
	[RoutePrefix("api/v{version:apiVersion}")]
	[Obsolete("Please use the V5 routes.")]
	public class DocumentController : ApiController
	{
		/// <inheritdoc />
		/// <summary>
		///    Initializes a new instance of the
		///    <see cref="T:CareSource.WC.Services.Document.Controllers.v1.DocumentController" /> class.
		/// </summary>
		/// <param name="documentManager"></param>
		/// <param name="memberDocumentManager">The member document manager.</param>
		/// <param name="providerDocumentManager">The provider document manager.</param>
		/// <param name="getDocumentManager"></param>
		/// <param name="keywordManager"></param>
		/// <param name="logger"></param>
		public DocumentController(
			IDocumentManager<DocumentHeader> documentManager,
			IMemberDocumentManager<DocumentHeader> memberDocumentManager,
			IProviderDocumentManager<DocumentHeader> providerDocumentManager,
			IGetDocumentManager<Document> getDocumentManager,
			IKeywordManager keywordManager,
			ILogger logger)
		{
			_logger = logger;

			_memberDocumentManager = memberDocumentManager;
			_providerDocumentManager = providerDocumentManager;
			_getDocumentManager = getDocumentManager;
			_documentManager = documentManager;
			_keywordManager = keywordManager;
		}

		/// <summary>
		///    Success Message Document Controller option
		/// </summary>
		private const string SuccessMessage = "Success";

		/// <summary>
		///    Gets the document manager of the document controller class.
		/// </summary>
		private readonly IDocumentManager<DocumentHeader> _documentManager;

		/// <summary>
		///    Get Document Manager Document Controller option
		/// </summary>
		private readonly IGetDocumentManager<Document> _getDocumentManager;

		/// <summary>
		///    Logger Document Controller option
		/// </summary>
		private readonly ILogger _logger;

		/// <summary>
		///    Gets the member document manager of the document controller class.
		/// </summary>
		private readonly IMemberDocumentManager<DocumentHeader> _memberDocumentManager;

		/// <summary>
		///    Gets the provider document manager of the document controller class.
		/// </summary>
		private readonly IProviderDocumentManager<DocumentHeader> _providerDocumentManager;

		private readonly IKeywordManager _keywordManager;

		private void ValidateModelState()
		{
			if (!ModelState.IsValid)
			{
				throw new Exception(
					ModelState.Values.First(v => v.Errors.Any())
						.Errors.First()
						.ErrorMessage);
			}

			_logger.LogInformation("Successfully validated model state!");
		}

		private void VerifyRequest(
			BaseRequest request)
		{
			if (request.CorrelationGuid.Equals(Guid.Empty))
			{
				request.CorrelationGuid = Guid.NewGuid();
			}

			ValidateModelState();
		}

		/// <summary>
		///    Gets the document.
		/// </summary>
		/// <param name="documentId">The document identifier.</param>
		/// <param name="request">The request.</param>
		/// <returns>
		///    A base64 encoded string of the file bytes
		/// </returns>
		//[Obsolete("Please use the V5 routes.")]
		//[SwaggerOperation("Deprecated, Use '/document/api/v5/{documentId}'")]
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

				Document document = _getDocumentManager.GetDocument(
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
					onBaseException.Message,
					onBaseException);
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
					exception.Message,
					exception);
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
		///    Gets the member documents based on the provided member Id.
		/// </summary>
		/// <param name="memberId">The member identifier.</param>
		/// <param name="request">The request.</param>
		/// <returns>
		/// </returns>
		[HttpGet]
		[Route("member/{memberId}")]
		[Obsolete("Please use the V5 routes.")]
		[SwaggerOperation("Deprecated, Use '/document/api/v5/member/{memberId}'")]
		[SwaggerResponse(
			HttpStatusCode.OK,
			"Successfully searched for Documents, this may or may nor return any.",
			typeof(ListDocumentsResponse))]
		public IHttpActionResult GetMemberDocuments(
			string memberId,
			[FromUri] ListDocumentsRequest request)
		{
			try
			{
				VerifyRequest(request);

				ISearchResults<DocumentHeader> documents = _memberDocumentManager.Search(
					memberId,
					request);

				_logger.LogInformation(
					$"Successfully searched for Member Documents for Member [{memberId}] and returned'"
					+ $" {documents.TotalRecordCount} documents.");

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
					onBaseException.Message,
					onBaseException);
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
					exception.Message,
					exception);
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
		///    Gets the provider documents.
		/// </summary>
		/// <param name="providerId">The provider identifier.</param>
		/// <param name="request">The request.</param>
		/// <returns></returns>
		[HttpGet]
		[Route("provider/{providerId}")]
		[Obsolete("Please use the V5 routes.")]
		[SwaggerOperation("Deprecated, Use '/document/api/v5/provider/{providerId}'")]
		public IHttpActionResult GetProviderDocuments(
			string providerId,
			[FromUri] ListDocumentsRequest request)
		{
			try
			{
				VerifyRequest(request);

				ISearchResults<DocumentHeader> documents = _providerDocumentManager.Search(
					providerId,
					request);

				_logger.LogInformation(
					$"Successfully searched for Provider Documents for Provider [{providerId}] "
					+ $"and returned {documents.TotalRecordCount} documents.");

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
					onBaseException.Message,
					onBaseException);

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
					exception.Message,
					exception);
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
		///    Gets the provider documents.
		/// </summary>
		/// <param name="tin">the provider Tax Id Number</param>
		/// <param name="request">The request.</param>
		/// <returns></returns>
		[HttpGet]
		[Route("provider/tin/{tin}")]
		[Obsolete("Please use the V5 routes.")]
		[SwaggerOperation("Deprecated, Use '/document/api/v5/provider/tin/{tin}'")]
		public IHttpActionResult GetProviderDocumentsByTin(
			string tin,
			[FromUri] ListDocumentsRequest request)
		{
			try
			{
				VerifyRequest(request);

				ISearchResults<DocumentHeader> documents = _providerDocumentManager.SearchTin(
					tin,
					request);
				_logger.LogInformation(
					$"Successfully searched for Provider Documents for Provider TIN [{tin}] "
					+ $"and returned {documents.TotalRecordCount} documents.");
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
					onBaseException.Message,
					onBaseException);
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
					exception.Message,
					exception);
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
		///    Gets the member documents.
		/// </summary>
		/// <param name="request">The request.</param>
		/// <returns></returns>
		[HttpGet]
		[Route("search")]
		[Obsolete("Please use the V5 routes.")]
		[SwaggerOperation("Deprecated, Use '/document/api/v5/search'")]
		public IHttpActionResult SearchDocuments(
			[FromUri] ListDocumentsRequest request)
		{
			try
			{
				VerifyRequest(request);

				ISearchResults<DocumentHeader> documents = _documentManager.Search(request);

				_logger.LogInformation(
					"Successfully searched for Documents and returned "
					+ $"{documents.TotalRecordCount} documents.");

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
					onBaseException.Message,
					onBaseException);
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
					exception.Message,
					exception);

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
		/// Updates the keywords.
		/// </summary>
		/// <param name="request">Array of document and their keywords that need updating</param>
		/// <returns>
		/// </returns>
		[HttpPut]
		[Route("keywords/update")]
		[Obsolete("Please use the V5 routes.")]
		public IHttpActionResult UpdateKeywords(
			[FromBody] BatchUpdateKeywordsRequest request)
		{
			try
			{
				VerifyRequest(request);
				if (_keywordManager.ValidateRequest(
					request,
					ModelState))
				{
					(IEnumerable<long> successfulIds, IEnumerable<string> errors)
						results = _keywordManager.UpdateKeywords(request);

					string message = SuccessMessage;
					ResponseStatus status = ResponseStatus.Success;
					ErrorCode errorCode = ErrorCode.Success;

					if (results.errors.Any()
						&& results.successfulIds.Any())
					{
						message = "Success with failures response data";
						status = ResponseStatus.Failure;
						errorCode = ErrorCode.UnknownError;
					}
					else if (results.errors.Any()
						&& !results.successfulIds.Any())
					{
						message = "Error, see response data";
						status = ResponseStatus.Error;
						errorCode = ErrorCode.UnknownError;
					}

					return Ok(new KeywordUpdateResponse(
						status,
						message,
						errorCode,
						request.CorrelationGuid,
						new KeywordUpdateResult
						{
							SuccessfulIds = results.successfulIds,
							Failures = results.errors
						}));
				}


				return BadRequest(ModelState);
			}
			catch (Exception e)
			{
				return Content(
					HttpStatusCode.BadRequest,
					new KeywordUpdateResponse(
					ResponseStatus.Error,
					e.Message,
					ErrorCode.UnknownError,
					request.CorrelationGuid,
					null));
			}
		}

		[HttpGet]
		[Route("search/retrieve")]
		[Obsolete("Please use the V5 routes.")]
		public async Task<IHttpActionResult> SearchAndGetDocuments(
			[FromUri] ListDocumentsRequest request)
		{
			try
			{
				VerifyRequest(request);

				(IEnumerable<Document> documents, int count) documents = await _getDocumentManager
					.SearchAsync(request)
					.ConfigureAwait(false);

				_logger.LogInformation(
					"Successfully searched for Documents and returned "
					+ $"{documents.count} documents.");

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
					onBaseException.Message,
					onBaseException);

				string response = null;

				return Content(
					HttpStatusCode.OK,
					new
					{
						Status = ResponseStatus.Error,
						Message = onBaseException.Message,
						ErrorCode = onBaseException.ErrorCode,
						request.CorrelationGuid,
						TotalRecords = 0,
						ResponseData = response
					});
			}
			catch (Exception exception)
			{
				_logger.LogError(
					exception.Message,
					exception);

				string response = null;

				return Content(
					HttpStatusCode.OK,
					new
					{
						Status = ResponseStatus.Error,
						Message = exception.Message,
						ErrorCode = ErrorCode.UnknownError,
						request.CorrelationGuid,
						TotalRecords = 0,
						ResponseData = response
					});
			}
		}
	}
}