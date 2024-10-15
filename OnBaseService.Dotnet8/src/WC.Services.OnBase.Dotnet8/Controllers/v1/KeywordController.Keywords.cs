// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   WC.Services.OnBase
//   KeywordController.Keywords.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.OnBase.Dotnet8.Controllers.v1
{
    using Microsoft.AspNetCore.Mvc;
    using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.Net;
    using WC.Services.OnBase.Dotnet8.Models;
    using WC.Services.OnBase.Dotnet8.Models.v1;

    public partial class KeywordsController
	{
		[HttpGet]
		[Route("Keywords")]
		public IActionResult Keywords()
		{
			try
			{
				IEnumerable<Keyword> keywords =
					_documentManager.ListKeywords();

				_logger.Info("Successfully loaded Keywords");

				return Ok(
					new BaseResponse<IEnumerable<Keyword>>(
						"Successfully loaded Keywords",
						(int)HttpStatusCode.OK,
						keywords
					));
			}
			catch (Exception ex)
			{
                _logger.Error("Bad Request", ex);
                return BadRequest("Failed to list keywords");
            }
		}

		[HttpGet]
		[Route("ListKeywords/{documentTypeId}")]
		public IActionResult ListKeywordsByDocumentTypeId(long documentTypeId)
		{
			try
			{
				IEnumerable<Keyword> keywords =
					_documentManager.ListKeywords(documentTypeId);

				_logger.Info("Successfully loaded Keywords");

				return Ok(
					new BaseResponse<IEnumerable<Keyword>>(
						$"Successfully loaded Keywords for Document Type Id:[{documentTypeId}]",
						(int)HttpStatusCode.OK,
						keywords
					));
			}
			catch (Exception ex)
			{
                _logger.Error("Bad Request", ex);
                return BadRequest("Failed to list keyworeds for Document Type Id : "+documentTypeId);
			}
		}
	}
}