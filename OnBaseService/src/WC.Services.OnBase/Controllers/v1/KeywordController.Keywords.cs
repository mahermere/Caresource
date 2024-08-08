// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   WC.Services.OnBase
//   KeywordController.Keywords.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.OnBase.Controllers.v1
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.Net;
	using System.Web.Http;
	using CareSource.WC.Services.Models;
	using CareSource.WC.Services.OnBase.Models.v1;

	public partial class KeywordsController
	{
		[HttpGet]
		[Route("")]
		public IHttpActionResult Keywords([FromUri] OnBaseRequest request)
		{
			try
			{
				_documentManager.ValidateRequest(
					request,
					ModelState);

				IEnumerable<Keyword> keywords =
					_documentManager.ListKeywords();

				Logger.LogInfo("Successfully loaded Keywords");

				return Ok(
					new BaseResponse<IEnumerable<Keyword>>(
						"Successfully loaded Keywords",
						(int)HttpStatusCode.OK,
						request.CorrelationGuid,
						keywords
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
		public IHttpActionResult ListKeywordsByDocumentTypeId(
			long documentTypeId,
			[FromUri] OnBaseRequest request)
		{
			try
			{
				_documentManager.ValidateRequest(
					request,
					ModelState);

				IEnumerable<Keyword> keywords =
					_documentManager.ListKeywords(documentTypeId);

				Logger.LogInfo("Successfully loaded Keywords");

				return Ok(
					new BaseResponse<IEnumerable<Keyword>>(
						$"Successfully loaded Keywords for Document Type Id:[{documentTypeId}]",
						(int)HttpStatusCode.OK,
						request.CorrelationGuid,
						keywords
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