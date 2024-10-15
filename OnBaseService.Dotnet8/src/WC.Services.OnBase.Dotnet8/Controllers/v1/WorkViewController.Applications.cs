// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   WC.Services.OnBase
//   WorkViewContoller.Applications.cs
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

    public partial class WorkViewController
	{
		private const string SuccessMessage = "Successfully retrieved Applications";

		[HttpGet]
		[Route("ListApplications")]
		public IActionResult ListApplications()
		{
			try
			{
				IEnumerable<WVApplication> applications =
					_workviewManager.ListApplications();

				if (applications != null &&
				    applications.Any())
				{
					_logger.Info(SuccessMessage);

					return Ok(
						new BaseResponse<IEnumerable<WVApplication>>(
							SuccessMessage,
							(int)HttpStatusCode.OK,
							applications
						));
				}

				return Ok(
					new BaseResponse<string>(
						SuccessMessage,
						(int)HttpStatusCode.NoContent,
						"No Applications found."
					));
			}
			catch (Exception exception)
			{
				return HandleUnknownErrors(exception);
			}
		}

		[HttpGet]
		[Route("Id/{applicationId}")]
		public IActionResult GetApplicationById(long applicationId)
		{
			try
			{
				WVApplication application =
					_workviewManager.GetApplicationById(applicationId);

				if (application != null)
				{
					_logger.Info(SuccessMessage);
					return Ok(
						new BaseResponse<WVApplication>(
							SuccessMessage,
							(int)HttpStatusCode.OK,
							application
						));
				}

				return Content(
					HttpStatusCode.MultiStatus.ToString(),
					new BaseResponse<string>(
						$"Application not found for Id:{applicationId}",
						207,null).ToString());
			}
			catch (Exception exception)
			{
				return HandleUnknownErrors(exception);
			}
		}

		[HttpGet]
		[Route("Name/{applicationName}")]
		public IActionResult GetApplicationByName(string applicationName)
		{
			try
			{
				WVApplication application =
					_workviewManager.GetApplicationByName(applicationName);

				if (application != null)
				{
					_logger.Info(SuccessMessage);
					return Ok(
						new BaseResponse<WVApplication>(
							SuccessMessage,
							(int)HttpStatusCode.OK,
							application
						));
				}

				return Content(HttpStatusCode.MultiStatus.ToString(),
					new BaseResponse<string>(
						$"Application not found for:{applicationName}",
						207,null).ToString());
			}
			catch (Exception ex)
			{
                _logger.Error("Bad Request", ex);
                return BadRequest("Failed to found application : "+applicationName);
            }
		}
	}
}