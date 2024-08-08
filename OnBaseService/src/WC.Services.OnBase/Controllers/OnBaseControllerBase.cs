// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   WC.Services.OnBase
//   OnBaseControllerBase.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.OnBase.Controllers
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.Net;
	using System.Web.Http;
	using CareSource.WC.Entities.Requests.Base;
	using CareSource.WC.OnBase.Core.Diagnostics.Interfaces;
	using CareSource.WC.OnBase.Core.Http.Filters;
	using CareSource.WC.Services.Models;

	[OnBaseAuthorizeFilter]
	public abstract class OnBaseControllerBase : ApiController
	{
		protected readonly ILogger Logger;

		/// <summary>
		///    Initializes a new instance of the <see cref="OnBaseControllerBase" /> class.
		/// </summary>
		/// <param name="logger">The logger.</param>
		protected OnBaseControllerBase(
			ILogger logger)
			=> Logger = logger;

		protected IHttpActionResult HandleUnknownErrors(
			BaseRequest request,
			Exception exception)
		{
			Logger.LogError(
				exception.Message,
				exception);

			return Content(
				HttpStatusCode.InternalServerError,
				new BaseResponse<string>(
					"That is weird, I tried to complete your request; Please see my logs for what " +
					"really happened.",
					(int)HttpStatusCode.InternalServerError,
					request.CorrelationGuid,
					null
				));
		}

		protected IHttpActionResult HandleValidationErrors(
			BaseRequest request,
			ValidationException exception)
		{
			Logger.LogError(
				"Bad Request",
				exception);

			return Content(
				HttpStatusCode.BadRequest,
				new ValidationProblemResponse(
					request.CorrelationGuid,
					ModelState));
		}
	}
}