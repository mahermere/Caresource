// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   WC.Services.OnBase
//   DocumentController.TypeGroups.cs
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
	using System.Web.Http.Description;
	using CareSource.WC.Services.Models;
	using CareSource.WC.Services.OnBase.Models.v1;
	using Swashbuckle.Swagger.Annotations;

	public partial class DocumentTypesController
	{
		[HttpGet]
		[Route("groups")]
		[ResponseType(typeof(BaseResponse<IEnumerable<DocumentTypeGroup>>))]
		[SwaggerResponse(HttpStatusCode.BadRequest, "There is an issue that did not pass validation", typeof(ValidationProblemResponse))]
		[SwaggerResponse(HttpStatusCode.InternalServerError, "Any unhandled exception.", typeof(BaseResponse<string>))]
		public IHttpActionResult ListDocumentTypeGroups([FromUri] OnBaseRequest request)
		{
			try
			{
				_documentManager.ValidateRequest(
					request,
					ModelState);

				IEnumerable<DocumentTypeGroup> groups =
					_documentManager.ListDocumentTypeGroups();

				if (groups != null
				    && groups.Any())
				{
					return Ok(
						new BaseResponse<IEnumerable<DocumentTypeGroup>>(
							"Successfully loaded Document Type Groups",
							(int)HttpStatusCode.OK,
							request.CorrelationGuid,
							groups
						));
				}

				return Ok(
					new BaseResponse<string>(
						"No Document Type Groups found",
						(int)HttpStatusCode.NoContent,
						request.CorrelationGuid,
						"No Document Type Groups found."
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
		[Route("group/{documentTypeGroupId}")]
		public IHttpActionResult ListDocumentTypes(
			long documentTypeGroupId,
			[FromUri] OnBaseRequest request)
		{
			try
			{
				_documentManager.ValidateRequest(
					request,
					ModelState);

				DocumentTypeGroup docTypes =
					_documentManager.GetDocumentTypeGroup(documentTypeGroupId);

				Logger.LogInfo(SuccessMessage);

				if (docTypes != null)
				{
					return Ok(
						new BaseResponse<DocumentTypeGroup>(
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
						$"Document Type Group not found for Id [{documentTypeGroupId}]."
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