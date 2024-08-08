// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   WC.Services.Document
//   DocumentController.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.Document.Controllers.v1
{
	using System;
	using System.Globalization;
	using System.Linq;
	using System.Net;
	using System.Web.Http;
	using System.Web.Http.ValueProviders;
	using CareSource.WC.Entities.Common.Interfaces;
	using CareSource.WC.Entities.Documents;
	using CareSource.WC.Entities.Exceptions;
	using CareSource.WC.Entities.Responses;
	using Microsoft.Extensions.Logging;
	using CareSource.WC.OnBase.Core.ExtensionMethods;
	using CareSource.WC.Services.Document.Managers;
	using Microsoft.Web.Http;
	using Swashbuckle.Swagger.Annotations;

	/// <summary>
	/// Represents the data used to define a the document controller
	/// </summary>
	/// <seealso cref="System.Web.Http.ApiController" />
	[ApiVersion("1", Deprecated = true)]
	[RoutePrefix("api/v{version:apiVersion}")]
	[Obsolete("Please use the V5 routes.")]
	public class DocumentController : ApiController
	{
		/// <summary>
		///    Success Message Document Controller option
		/// </summary>
		private const string SuccessMessage = "Success";

		/// <inheritdoc />
		/// <summary>
		///    Initializes a new instance of the
		///    <see cref="T:CareSource.WC.Services.Document.Controllers.v1.DocumentController" /> class.
		/// </summary>
		/// <param name="memberDocumentManager">The member document manager.</param>
		/// <param name="providerDocumentManager">The provider document manager.</param>
		/// <param name="logger"></param>
		public DocumentController(
			IMemberDocumentManager<DocumentHeader> memberDocumentManager,
			IProviderDocumentManager<DocumentHeader> providerDocumentManager,
			ILogger logger)
		{
			_logger = logger;

			_memberDocumentManager = memberDocumentManager;
			_providerDocumentManager = providerDocumentManager;
		}

		/// <summary>
		/// Logger Document Controller option
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

		/// <summary>
		///    Gets the member documents.
		/// </summary>
		/// <param name="memberId">The member identifier.</param>
		/// <param name="request">The request.</param>
		/// <returns></returns>
		[HttpGet]
		[Route("member/{memberId}")]
		[Obsolete("Please use the V5 routes.")]
		[SwaggerOperation("Deprecated, Use '/document/api/v5/member{memberId}'")]
		public IHttpActionResult GetMemberDocuments(
			string memberId,
			[FromUri] ListDocumentsRequest request)
		{
			try
			{
				ValidateModelState(request);

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
				ValidateModelState(request);

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
				ValidateModelState(request);

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

		private void ValidateModelState(
			ListDocumentsRequest request)
		{
			if (request.CorrelationGuid.Equals(Guid.Empty))
			{
				request.CorrelationGuid = Guid.NewGuid();
			}

			if (request.UserId.IsNullOrWhiteSpace())
			{
				// This is added for this method because the code is being used in production and does
				// not provide the User Id.  I am setting the value and clearing the error.
				// Once Provider Portal starts passing the user ID this can be removed
				ModelState.SetModelValue(
					"request.UserId",
					new ValueProviderResult(
						$"{request.SourceApplication} user",
						"",
						CultureInfo.InvariantCulture));
				ModelState["request.UserId"]
					.Errors.Clear();

				request.UserId = $"{request.SourceApplication} user";
			}

			if (!ModelState.IsValid)
			{
				throw new Exception(
					ModelState.Values.First(v => v.Errors.Any())
						.Errors.First()
						.ErrorMessage);
			}

			_logger.LogInformation("Successfully validated model state!");
		}
	}
}