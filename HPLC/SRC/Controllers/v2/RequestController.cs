// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2021. All rights reserved.
// 
//   WC.Services.Hplc
//   RequestController.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.Hplc.Controllers.v2
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Net;
	using System.Security.Authentication;
	using System.Web.Http;
	using CareSource.WC.OnBase.Core.Diagnostics.Interfaces;
	using CareSource.WC.OnBase.Core.Http.Filters;
	using Microsoft.Web.Http;
	using Swashbuckle.Swagger.Annotations;
	using WC.Services.Hplc.Managers.v2;
	using WC.Services.Hplc.Models.Core;
	using WC.Services.Hplc.Models.v2;

	/// <summary>
	///    Data and functions describing a CareSource.WC.Services.Hplc.Controllers.v1.RequestController
	///    object.
	/// </summary>
	[OnBaseAuthorizeFilter]
	[RoutePrefix("request/api/v{version:apiVersion}")]
	[ApiVersion("2")]
	[SwaggerResponse(
		HttpStatusCode.OK,
		"When all is good.")]
	[SwaggerResponse(
		HttpStatusCode.Unauthorized,
		"When unable to authenticate using the basic Authentication.",
		typeof(ValidationProblemResponse))]
	[SwaggerResponse(
		HttpStatusCode.BadRequest,
		"When there is any validation error or json parsing error.",
		typeof(ValidationProblemResponse))]
	[SwaggerResponse(
		HttpStatusCode.InternalServerError,
		"When there is any unhandled exception.",
		typeof(ValidationProblemResponse))]
	public class RequestController : ApiController
	{
		private readonly ILogger _logger;
		private readonly IRequestManager _manager;

		/// <summary>
		///    Initializes a new instance of the <see cref="RequestController" /> class.
		/// </summary>
		/// <param name="manager">The manager.</param>
		/// <param name="logger">The logger.</param>
		public RequestController(
			IRequestManager manager,
			ILogger logger)
		{
			_manager = manager;
			_logger = logger;
		}

		/// <summary>
		///    Creates the request.
		/// </summary>
		/// <param name="request">The request.</param>
		/// <returns></returns>
		[HttpPost]
		[Route("")]
		public IHttpActionResult CreateRequest([FromBody] HplcServiceRequest request)
		{
			_logger.LogDebug(
				nameof(CreateRequest),
				new Dictionary<string, object> { { nameof(request), request } });

			Guid correlationGuid = request.CorrelationGuid;

			try
			{
				if (!_manager.ValidateRequest(
					request,
					ModelState))
				{
					return Content(
						HttpStatusCode.BadRequest,
						new ValidationProblemResponse(
							ModelState,
							request?.CorrelationGuid ?? Guid.NewGuid()));
				}

				long newId = _manager.CreateRequest(request);

				return CreatedAtRoute(
					"GetRequest",
					new
					{
						version = "2",
						requestId = newId
					},
					newId);
			}
			catch (Exception ex)
				when (ex is KeyNotFoundException
					|| ex is ArgumentException)
			{
				ModelState.AddModelError(
					"Request",
					ex.Message);

				_logger.LogError(
					$"ex.Message::{ex.Message}",
					ex,
					new Dictionary<string, object> { { nameof(correlationGuid), correlationGuid } });

				new Dictionary<string, object>
				{
					{ nameof(request.CorrelationGuid), request.CorrelationGuid }
				};

				return Content(
					HttpStatusCode.BadRequest,
					new ValidationProblemResponse(
						ModelState,
						correlationGuid));
			}
			catch (Exception ex)
				when (
					ex is UnauthorizedAccessException
					|| ex is AuthenticationException)
			{
				_logger.LogError(
					$"ex.Message::{ex.Message}",
					ex,
					new Dictionary<string, object> { { nameof(correlationGuid), correlationGuid } });

				return Content(
					HttpStatusCode.Unauthorized,
					ex.Message);
			}
			catch (Exception ex)
			{
				_logger.LogError(
					$"ex.Message::{ex.Message}",
					ex,
					new Dictionary<string, object> { { nameof(correlationGuid), correlationGuid } });

				return Content(
					HttpStatusCode.InternalServerError,
					$"An unexpected error occurred, please see the logs and reference Id: '{request.CorrelationGuid}'");
			}
		}

		/// <summary>
		///    Gets the request.
		/// </summary>
		/// <param name="requestId">The request identifier.</param>
		/// <returns></returns>
		/// <response code="500">If there is an uncaught server exception.</response>
		[HttpGet]
		[Route("{requestId}")]
		public IHttpActionResult GetRequest(long requestId)
		{
			Guid correlationGuid = Guid.NewGuid();

			try
			{
				_logger.LogDebug(
					nameof(GetRequest),
					new Dictionary<string, object> { { nameof(requestId), requestId } });

				Request obj = _manager.GetRequest(requestId);
				return Ok(obj);
			}
			catch (Exception ex)
				when (ex is KeyNotFoundException
					|| ex is ArgumentException)
			{
				ModelState.AddModelError(
					"Request",
					ex.Message);

				_logger.LogError(
					$"ex.Message::{ex.Message}",
					ex,
					new Dictionary<string, object> { { nameof(correlationGuid), correlationGuid } });

				return Content(
					HttpStatusCode.BadRequest,
					new ValidationProblemResponse(
						ModelState,
						correlationGuid));
			}
			catch (Exception ex)
				when (
					ex is UnauthorizedAccessException
					|| ex is AuthenticationException)
			{
				_logger.LogError(
					$"ex.Message::{ex.Message}",
					ex,
					new Dictionary<string, object> { { nameof(correlationGuid), correlationGuid } });

				return Content(
					HttpStatusCode.Unauthorized,
					ex.Message);
			}
			catch (Exception ex)
			{
				_logger.LogError(
					$"ex.Message::{ex.Message}",
					ex,
					new Dictionary<string, object> { { nameof(correlationGuid), correlationGuid } });

				return Content(
					HttpStatusCode.InternalServerError,
					$"An unexpected error occurred, please see the logs and reference Id: '{correlationGuid}'");
			}
		}


		/// <summary>
		///    Searches the DB for the request with the request parameters
		/// </summary>
		/// <param name="request">The request.</param>
		/// <returns></returns>
		[HttpGet]
		[Route("search")]
		public IHttpActionResult Search([FromUri] SearchRequest request)
		{
			Guid correlationGuid = request.CorrelationGuid;

			try
			{
				_logger.LogDebug(
					nameof(Search),
					new Dictionary<string, object> { { nameof(request), request } });

				IEnumerable<Request> requests = _manager.Search(request);

				return Content(
					HttpStatusCode.OK,
					!requests.SafeAny()
						? Array.Empty<object>()
						: requests.ToArray());
			}
			catch (Exception ex)
				when (ex is ArgumentException)
			{
				ModelState.AddModelError(
					"Request",
					ex.Message);

				_logger.LogError(
					$"ex.Message::{ex.Message}",
					ex,
					new Dictionary<string, object> { { nameof(correlationGuid), correlationGuid } });

				return Content(
					HttpStatusCode.BadRequest,
					new ValidationProblemResponse(
						ModelState,
						correlationGuid));
			}
			catch (Exception ex)
				when (ex is KeyNotFoundException)
			{
				ModelState.AddModelError(
					"Request",
					ex.Message);

				_logger.LogError(
					$"ex.Message::{ex.Message}",
					ex,
					new Dictionary<string, object> { { nameof(correlationGuid), correlationGuid } });

				return
					Content(
						HttpStatusCode.NotFound,
						new ValidationProblemResponse(
							ModelState,
							request.CorrelationGuid));
			}

			catch (Exception ex)
				when (
					ex is UnauthorizedAccessException
					|| ex is AuthenticationException)
			{
				_logger.LogError(
					$"ex.Message::{ex.Message}",
					ex,
					new Dictionary<string, object> { { nameof(correlationGuid), correlationGuid } });

				return Content(
					HttpStatusCode.Unauthorized,
					ex.Message);
			}
			catch (Exception ex)
			{
				_logger.LogError(
					$"ex.Message::{ex.Message}",
					ex,
					new Dictionary<string, object> { { nameof(correlationGuid), correlationGuid } });

				return Content(
					HttpStatusCode.InternalServerError,
					$"An unexpected error occurred, please see the logs and reference Id: '{request.CorrelationGuid}'");
				;
			}
		}

		/// <summary>
		///    Searches the Requests for a Specific Request Id and a specific Provider NPI
		/// </summary>
		/// <param name="applicationNumber">The application number.</param>
		/// <param name="npi">The provider npi.</param>
		/// <returns></returns>
		/// <exception cref="ArgumentException">No Request was found matching the Id's provided</exception>
		[HttpGet]
		[Route("search/{applicationNumber}/{npi}")]
		[SwaggerResponse(
			HttpStatusCode.OK,
			"When all is good.")]
		[SwaggerResponse(
			HttpStatusCode.Unauthorized,
			"When unable to authenticate using the basic Authentication.",
			typeof(ValidationProblemResponse))]
		[SwaggerResponse(
			HttpStatusCode.BadRequest,
			"When there is any validation error or json parsing error.",
			typeof(ValidationProblemResponse))]
		[SwaggerResponse(
			HttpStatusCode.InternalServerError,
			"When there is any unhandled exception.",
			typeof(ValidationProblemResponse))]
		public IHttpActionResult SearchByRequestAndNpi(
			string applicationNumber,
			string npi)
		{
			Guid correlationGuid = Guid.NewGuid();

			try
			{
				_logger.LogDebug(
					nameof(SearchByRequestAndNpi),
					new Dictionary<string, object>
					{
						{ nameof(correlationGuid), correlationGuid},
						{ nameof(applicationNumber), applicationNumber },
						{ nameof(npi), npi }
					});

				StatusResponse request = _manager.SearchByNpi(
					applicationNumber,
					npi);
				

				return request != null && request.Providers.SafeAny()
					? Ok(new { request })
					: throw new KeyNotFoundException("No providers were found matching the Id's provided");
			}
			catch (Exception ex)
				when (ex is KeyNotFoundException
					|| ex is ArgumentException)
			{
				ModelState.AddModelError(
					"Request",
					ex.Message);

				_logger.LogError(
					$"ex.Message::{ex.Message}",
					ex,
					new Dictionary<string, object> { { nameof(correlationGuid), correlationGuid } });

				return Content(
					HttpStatusCode.BadRequest,
					new ValidationProblemResponse(
						ModelState,
						correlationGuid));
			}
			catch (Exception ex)
				when (
					ex is UnauthorizedAccessException
					|| ex is AuthenticationException)
			{
				_logger.LogError(
					$"ex.Message::{ex.Message}",
					ex,
					new Dictionary<string, object> { { nameof(correlationGuid), correlationGuid } });

				return Content(
					HttpStatusCode.Unauthorized,
					ex.Message);
			}
			catch (Exception ex)
			{
				_logger.LogError(
					$"ex.Message::{ex.Message}",
					ex,
					new Dictionary<string, object> { { nameof(correlationGuid), correlationGuid } });

				return Content(
					HttpStatusCode.InternalServerError,
					$"An unexpected error occurred, please see the logs and reference Id: '{correlationGuid}'");
			}
		}

		/// <summary>
		///    Statuses the specified request identifier.
		/// </summary>
		/// <param name="requestId">The request identifier.</param>
		/// <returns></returns>
		[HttpGet]
		[Route("status/{requestId}")]
		[SwaggerResponse(
			HttpStatusCode.OK,
			"When all is good.")]
		[SwaggerResponse(
			HttpStatusCode.Unauthorized,
			"When unable to authenticate using the basic Authentication.",
			typeof(ValidationProblemResponse))]
		[SwaggerResponse(
			HttpStatusCode.BadRequest,
			"When there is any validation error or json parsing error.",
			typeof(ValidationProblemResponse))]
		[SwaggerResponse(
			HttpStatusCode.InternalServerError,
			"When there is any unhandled exception.",
			typeof(ValidationProblemResponse))]
		public IHttpActionResult Status(long requestId)
		{
			Guid correlationGuid = Guid.NewGuid();

			try
			{
				_logger.LogDebug(
					nameof(Status),
					new Dictionary<string, object> { { nameof(requestId), requestId } });

				Request obj = _manager.GetRequest(requestId);
				return Ok(new { obj.Status });
			}
			catch (Exception ex)
				when (ex is KeyNotFoundException
					|| ex is ArgumentException)
			{
				ModelState.AddModelError(
					"Request",
					ex.Message);

				_logger.LogError(
					$"ex.Message::{ex.Message}",
					ex,
					new Dictionary<string, object> { { nameof(correlationGuid), correlationGuid } });

				return Content(
					HttpStatusCode.BadRequest,
					new ValidationProblemResponse(
						ModelState,
						correlationGuid));
			}
			catch (Exception ex)
				when (
					ex is UnauthorizedAccessException
					|| ex is AuthenticationException)
			{
				_logger.LogError(
					$"ex.Message::{ex.Message}",
					ex,
					new Dictionary<string, object> { { nameof(correlationGuid), correlationGuid } });

				return Content(
					HttpStatusCode.Unauthorized,
					ex.Message);
			}
			catch (Exception ex)
			{
				_logger.LogError(
					$"ex.Message::{ex.Message}",
					ex,
					new Dictionary<string, object> { { nameof(correlationGuid), correlationGuid } });

				return Content(
					HttpStatusCode.InternalServerError,
					$"An unexpected error occurred, please see the logs and reference Id: '{correlationGuid}'");
			}
		}

		/// <summary>
		///    Updates the Request data for the supplied Id
		/// </summary>
		/// <param name="requestId">The ID of the request to be updated</param>
		/// <param name="request">New data for teh existing request</param>
		/// <returns></returns>
		[HttpPut]
		[Route("{requestId}")]
		[SwaggerResponse(
			HttpStatusCode.OK,
			"When all is good.")]
		[SwaggerResponse(
			HttpStatusCode.Unauthorized,
			"When unable to authenticate using the basic Authentication.",
			typeof(ValidationProblemResponse))]
		[SwaggerResponse(
			HttpStatusCode.BadRequest,
			"When there is any validation error or json parsing error.",
			typeof(ValidationProblemResponse))]
		[SwaggerResponse(
			HttpStatusCode.InternalServerError,
			"When there is any unhandled exception.",
			typeof(ValidationProblemResponse))]
		public IHttpActionResult UpdateRequest(
			long requestId,
			[FromBody] HplcServiceRequest request)
		{
			_logger.LogDebug(
				"Update Request",
				new Dictionary<string, object> { { nameof(requestId), requestId } });

			_manager.ValidateRequest(
				request,
				ModelState);

			return Ok();
		}
	}
}