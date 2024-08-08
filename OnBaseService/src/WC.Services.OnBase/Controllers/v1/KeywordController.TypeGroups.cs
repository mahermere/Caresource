// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   WC.Services.OnBase
//   DocumentController.ListKeywordTypeGroups.cs
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
		[Route("groups")]
		public IHttpActionResult ListKeywords([FromUri] OnBaseRequest request)
		{
			try
			{
				_documentManager.ValidateRequest(
					request,
					ModelState);

				IEnumerable<KeywordTypeGroup> keywords =
					_documentManager.ListKeywordTypeGroups();

				Logger.LogInfo("Successfully loaded Keyword Type Groups");

				return Ok(
					new BaseResponse<IEnumerable<KeywordTypeGroup>>(
						"Successfully loaded Keyword Type Groups",
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