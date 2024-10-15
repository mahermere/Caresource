// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   WC.Services.OnBase
//   DocumentController.ListKeywordTypeGroups.cs
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
		[Route("ListKeywords")]
		public IActionResult ListKeywords()
		{
			try
			{
				IEnumerable<KeywordTypeGroup> keywords =
					_documentManager.ListKeywordTypeGroups();

				_logger.Info("Successfully loaded Keyword Type Groups");

				return Ok(
					new BaseResponse<IEnumerable<KeywordTypeGroup>>(
						"Successfully loaded Keyword Type Groups",
						(int)HttpStatusCode.OK,
						keywords
					));
			}
			catch (Exception ex)
			{
                _logger.Error("Bad Request", ex);
                return BadRequest("Failed to load keyword Type Groups");
            }
		}
	}
}