// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   WC.Services.OnBase
//   WorkViewContoller.Applications.cs
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

	public partial class WorkViewController
	{
		private const string SuccessMessage = "Successfully retrieved Applications";

		[HttpGet]
		[Route("")]
		public IHttpActionResult ListApplications([FromUri] OnBaseRequest request)
		{
			try
			{
				_workviewManager.ValidateRequest(
					request,
					ModelState);

				IEnumerable<WVApplication> applications =
					_workviewManager.ListApplications();

				if (applications != null &&
				    applications.Any())
				{
					Logger.LogInfo(SuccessMessage);

					return Ok(
						new BaseResponse<IEnumerable<WVApplication>>(
							SuccessMessage,
							(int)HttpStatusCode.OK,
							request.CorrelationGuid,
							applications
						));
				}

				return Ok(
					new BaseResponse<string>(
						SuccessMessage,
						(int)HttpStatusCode.NoContent,
						request.CorrelationGuid,
						"No Applications found."
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
		[Route("Id/{applicationId}")]
		public IHttpActionResult GetApplicationById(
			long applicationId,
			[FromUri] OnBaseRequest request)
		{
			try
			{
				_workviewManager.ValidateRequest(
					request,
					ModelState);

				WVApplication application =
					_workviewManager.GetApplicationById(applicationId);

				if (application != null)
				{
					Logger.LogInfo(SuccessMessage);
					return Ok(
						new BaseResponse<WVApplication>(
							SuccessMessage,
							(int)HttpStatusCode.OK,
							request.CorrelationGuid,
							application
						));
				}

				return Content(
					(HttpStatusCode)207,
					new BaseResponse<string>(
						$"Application not found for Id:{applicationId}",
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

		[HttpGet]
		[Route("Name/{applicationName}")]
		public IHttpActionResult GetApplicationByName(
			string applicationName,
			[FromUri] OnBaseRequest request)
		{
			try
			{
				_workviewManager.ValidateRequest(
					request,
					ModelState);

				WVApplication application =
					_workviewManager.GetApplicationByName(applicationName);

				if (application != null)
				{
					Logger.LogInfo(SuccessMessage);
					return Ok(
						new BaseResponse<WVApplication>(
							SuccessMessage,
							(int)HttpStatusCode.OK,
							request.CorrelationGuid,
							application
						));
				}

				return Content((HttpStatusCode)207,
					new BaseResponse<string>(
						$"Application not found for:{applicationName}",
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