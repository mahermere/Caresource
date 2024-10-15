// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   WC.Services.OnBase
//   DocumentController.TypeGroups.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.OnBase.Dotnet8.Controllers.v1
{
    using Microsoft.AspNetCore.Mvc;
    using Swashbuckle.AspNetCore.Annotations;
    using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.Linq;
	using System.Net;
    using WC.Services.OnBase.Dotnet8.Models;
    using WC.Services.OnBase.Dotnet8.Models.Base;
    using WC.Services.OnBase.Dotnet8.Models.v1;
    using WC.Services.OnBase.Dotnet8.OnBase;

    public partial class DocumentTypesController
	{
		[HttpGet]
		[Route("groups")]
		[SwaggerResponse((int)HttpStatusCode.BadRequest, "There is an issue that did not pass validation", typeof(ValidationProblemResponse))]
		[SwaggerResponse((int)HttpStatusCode.InternalServerError, "Any unhandled exception.", typeof(BaseResponse<string>))]
		public IActionResult ListDocumentTypeGroups()
		{
			try
			{

				IEnumerable<DocumentTypeGroup> groups =
					_documentManager.ListDocumentTypeGroups();

				if (groups != null
				    && groups.Any())
				{
					return Ok(
						new BaseResponse<IEnumerable<BaseModel>>(
							"Successfully loaded Document Type Groups",
							(int)HttpStatusCode.OK,
							groups
						));
				}

				return Ok(
					new BaseResponse<string>(
						"No Document Type Groups found",
						(int)HttpStatusCode.NoContent,
						"No Document Type Groups found."
					));
			}
			catch (Exception ex)
			{
                _logger.Error("Bad Request", ex);
				return BadRequest("Bad Request: Failed to load Document Type Groups");

            }
		}

		[HttpGet]
		[Route("group/{documentTypeGroupId}")]
		public IActionResult ListDocumentTypes(long documentTypeGroupId)
		{
			try
			{
				DocumentTypeGroup docTypes = _documentManager.GetDocumentTypeGroup(documentTypeGroupId);

				_logger.Info(SuccessMessage);

				if (docTypes != null)
				{
					return Ok(
						new BaseResponse<DocumentTypeGroup>(
							SuccessMessage,
							(int)HttpStatusCode.OK,
							docTypes
						));
				}

				return Ok(
					new BaseResponse<string>(
						SuccessMessage,
						(int)HttpStatusCode.NoContent,
						$"Document Type Group not found for Id [{documentTypeGroupId}]."
					));
			}
            catch (Exception ex)
            {
                _logger.Error("Bad Request", ex);
                return BadRequest("Bad Request: Failed to list Document Types");

            }
        }
	}
}