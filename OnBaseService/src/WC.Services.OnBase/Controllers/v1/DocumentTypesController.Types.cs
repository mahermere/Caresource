// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   WC.Services.OnBase
//   DocumentController.Types.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.OnBase.Controllers.v1
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.Linq;
	using System.Net;
	using System.Web.Http;
	using CareSource.WC.Services.Models;
	using CareSource.WC.Services.OnBase.Models.v1;

	public partial class DocumentTypesController
	{
		private const string SuccessMessage = "Successfully retrieved Document Types";

		[HttpGet]
		[Route("")]
		public IHttpActionResult ListDocumentTypes([FromUri] OnBaseRequest request)
		{
			try
			{
				_documentManager.ValidateRequest(
					request,
					ModelState);

				IEnumerable<DocumentType> docTypes =
					_documentManager.ListDocumentTypes();

				if (docTypes != null
				    && docTypes.Any())
				{
					Logger.LogInfo(SuccessMessage);
					return Ok(
						new BaseResponse<IEnumerable<DocumentType>>(
							SuccessMessage,
							(int)HttpStatusCode.OK,
							request.CorrelationGuid,
							docTypes
						));
				}

				return Ok(
					new BaseResponse<string>(
						SuccessMessage,
						(int)HttpStatusCode.NoContent,
						request.CorrelationGuid,
						"No Document Types found."
					));
			}
			catch (ValidationException validationException)
			{
				return HandleValidationErrors(
					request,
					validationException);

			}
			catch (Exception exception)
			{
				return HandleUnknownErrors(
					request,
					exception);
			}
		}

		[HttpGet]
		[Route("{documentTypeId}")]
		public IHttpActionResult GetDocumentType(
			long documentTypeId,
			[FromUri] OnBaseRequest request)
		{
			try
			{
				_documentManager.ValidateRequest(
					request,
					ModelState);

				DocumentType docType =
					_documentManager.GetDocumentType(documentTypeId);

				if (docType != null)
				{
					Logger.LogInfo(SuccessMessage);
					return Ok(
						new BaseResponse<DocumentType>(
							SuccessMessage,
							(int)HttpStatusCode.OK,
							request.CorrelationGuid,
							docType
						));
				}

				return Content((HttpStatusCode)207,
					new BaseResponse<string>(
						$"Document Type not found for Id:{documentTypeId}",
						207,
						request.CorrelationGuid,
						null
					));
			}
			catch (ValidationException validationException)
			{
				return HandleValidationErrors(
					request,
					validationException);

			}
			catch (Exception exception)
			{
				return HandleUnknownErrors(
					request,
					exception);
			}
		}

		[HttpGet()]
		[Route("search/{keywordName}")]
		public IHttpActionResult SearchByKeyword(
			string keywordName,
			[FromUri] OnBaseRequest request)
		{
			try
			{
				_documentManager.ValidateRequest(
					request,
					ModelState);

				IEnumerable<DocumentType> docTypes =
					_documentManager.SearchByKeyword(keywordName);

				if (docTypes != null && docTypes.Any())
				{
					Logger.LogInfo(SuccessMessage);
					return Ok(
						new BaseResponse<IEnumerable<DocumentType>>(
							SuccessMessage,
							(int)HttpStatusCode.OK,
							request.CorrelationGuid,
							docTypes
						));
				}

				return Content((HttpStatusCode)207,
					new BaseResponse<string>(
						$"No Documents found containing Keyword:'{keywordName}'",
						207,
						request.CorrelationGuid,
						null
					));
			}
			catch (ValidationException validationException)
			{
				return HandleValidationErrors(
					request,
					validationException);

			}
			catch (Exception exception)
			{
				return HandleUnknownErrors(
					request,
					exception);
			}
		}
	}
}