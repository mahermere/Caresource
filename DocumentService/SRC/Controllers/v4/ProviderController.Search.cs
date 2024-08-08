// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   WC.Services.Document
//   ProviderController.Search.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.Document.Controllers.v4
{
	using System;
	using System.Collections.Generic;
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
	///    Version 4 of the Document Controller to retrieve Provider document search's
	/// </summary>
	/// <seealso cref="System.Web.Http.ApiController" />
	public partial class ProviderController
	{
		/// <summary>
		///    Get the total records that match the parameters
		/// </summary>
		/// <param name="providerId"></param>
		/// <param name="request">The request.</param>
		/// <returns></returns>
		/// <exception cref="Exception"></exception>
		[HttpGet]
		[Route("{providerId}/DocumentTypesCount")]
		[Obsolete("Please use the V5 routes.")]
		public IHttpActionResult DocumentTypesCount(
			string providerId,
			[FromUri] DocumentTypesCountRequest request)
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

				IDictionary<string, int> docTypes = _providerManager.GetDocumentTypesCount(
					providerId,
					request);

				_logger.LogInformation($"{docTypes} document types match this query.");

				return Content(
					HttpStatusCode.OK,
					new DocumentTypesCountResponse(
						SuccessMessage,
						(int)HttpStatusCode.OK,
						request.CorrelationGuid,
						docTypes));
			}
			catch (OnBaseExceptionBase onBaseException)
			{
				_logger.LogError(
					onBaseException,
					onBaseException.Message);
				return Content(
					HttpStatusCode.OK,
					new DocumentTypesCountResponse(
						onBaseException.Message,
						(int)ResponseStatus.Error,
						request.CorrelationGuid,
						new Dictionary<string, int>()));
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
		///    Gets the provider documents.
		/// </summary>
		/// <param name="providerId">The provider identifier.</param>
		/// <param name="request">The request.</param>
		/// <returns></returns>
		[HttpGet]
		[Route("{providerId}")]
		[Obsolete("Please use the V5 routes.")]
		public IHttpActionResult GetProviderDocuments(
			string providerId,
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

				ISearchResults<DocumentHeader> documents = _providerManager.Search(
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