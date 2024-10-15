// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   WC.Services.OnBase
//   DocumentController.Types.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.OnBase.Dotnet8.Controllers.v1
{
    using Microsoft.AspNetCore.Mvc;
    using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.Linq;
	using System.Net;
    using WC.Services.OnBase.Dotnet8.Models;
    using WC.Services.OnBase.Dotnet8.Models.v1;

    public partial class DocumentTypesController
	{
		private const string SuccessMessage = "Successfully retrieved Document Types";

		[HttpGet]
		[Route("ListDocumentTypes")]
		public IActionResult ListDocumentTypes()
		{
			try
			{
				
				IEnumerable<DocumentType> docTypes =
					_documentManager.ListDocumentTypes();

				if (docTypes != null
				    && docTypes.Any())
				{
					_logger.Info(SuccessMessage);
					return Ok(
						new BaseResponse<IEnumerable<DocumentType>>(
							SuccessMessage,
							(int)HttpStatusCode.OK,
							docTypes
						));
				}

				return Ok(
					new BaseResponse<string>(
						SuccessMessage,
						(int)HttpStatusCode.NoContent,
						"No Document Types found."
					));
			}
            catch (Exception ex)
            {
                _logger.Error("Bad Request", ex);
                return BadRequest("Failed to list Document Types");
            }
        }

		[HttpGet]
		[Route("GetDocumentType/{documentTypeId}")]
		public IActionResult GetDocumentType(long documentTypeId)
		{
			try
			{
				DocumentType docType =
					_documentManager.GetDocumentType(documentTypeId);

				if (docType != null)
				{
					_logger.Info(SuccessMessage);
					return Ok(
						new BaseResponse<DocumentType>(
							SuccessMessage,
							(int)HttpStatusCode.OK,
							docType
						));
				}

				return Content(
					HttpStatusCode.MultiStatus.ToString(), 
					new BaseResponse<string>(
						$"Document Type not found for Id:{documentTypeId}",
						207,
						null
					).ToString());
			}
			catch (Exception ex)
			{
                _logger.Error("Bad Request", ex);
                return BadRequest("Failed to found Document Type");
            }
		}

		[HttpGet()]
		[Route("Search/{keywordName}")]
		public IActionResult SearchByKeyword(string keywordName)
		{
			try
			{
				
				IEnumerable<DocumentType> docTypes =
					_documentManager.SearchByKeyword(keywordName);

				if (docTypes != null && docTypes.Any())
				{
					_logger.Info(SuccessMessage);
					return Ok(
						new BaseResponse<IEnumerable<DocumentType>>(
							SuccessMessage,
							(int)HttpStatusCode.OK,
							docTypes
						));
				}

				return Content(
					HttpStatusCode.MultiStatus.ToString(),
					new BaseResponse<string>(
						$"No Documents found containing Keyword:'{keywordName}'",
						207,null).ToString());
			}
			catch (Exception ex)
			{
                _logger.Error("Bad Request", ex);
                return BadRequest("No Documents found containing Keyword:'" + keywordName + "'");
            }
		}
	}
}