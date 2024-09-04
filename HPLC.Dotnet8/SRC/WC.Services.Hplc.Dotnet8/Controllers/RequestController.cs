// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2021. All rights reserved.
// 
//   WC.Services.Hplc
//   RequestController.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WC.Services.Hplc.Dotnet8;

namespace WC.Services.Hplc.Dotnet8.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Swashbuckle.AspNetCore.Annotations;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Security.Authentication;
    using WC.Services.Hplc.Dotnet8.Authorization;
    using WC.Services.Hplc.Dotnet8.Managers.Interfaces;
    using WC.Services.Hplc.Dotnet8.Models;
    using WC.Services.Hplc.Dotnet8.Models.Core;
    using WC.Services.Hplc.Dotnet8.Repository;

    /// <summary>
    ///    Data and functions describing a CareSource.WC.Services.Hplc.Controllers.v1.RequestController
    ///    object.
    /// </summary>
    //[OnBaseAuthorizeFilter]
    //[RoutePrefix("request/api/v{version:apiVersion}")]
    //[ApiVersion("2")]
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    [SwaggerResponse(
        (int)HttpStatusCode.OK,
        "When all is good.")]
    [SwaggerResponse(
        (int)HttpStatusCode.Unauthorized,
        "When unable to authenticate using the basic Authentication.",
        typeof(ValidationProblemResponse))]
    [SwaggerResponse(
        (int)HttpStatusCode.BadRequest,
        "When there is any validation error or json parsing error.",
        typeof(ValidationProblemResponse))]
    [SwaggerResponse(
        (int)HttpStatusCode.InternalServerError,
        "When there is any unhandled exception.",
        typeof(ValidationProblemResponse))]
    
    public class RequestController : ControllerBase
    {
        private readonly log4net.ILog _logger;
        private readonly IRequestManager _manager;
        public RequestController(IRepository repo, IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration, log4net.ILog logger, IRequestManager manager) 
        {
            _logger = logger;
            _manager = manager;
        }

        [HttpGet]
        [Route("Test")]
        public IActionResult Test()
        {
            string Msg = "Testing Request Endpoints";
            // Guid correlationGuid = Guid.NewGuid();
            return Ok(Msg);
        }

        /// <summary>
		///    Creates the request.
		/// </summary>
		/// <param name="request">The request.</param>
		/// <returns></returns>
		[HttpPost]
        [Route("")]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Post))]
        public IActionResult CreateRequest([FromBody] HplcServiceRequest request)
        {
            _logger.Debug($"{nameof(CreateRequest)}: {request}");

            Guid correlationGuid = request.CorrelationGuid;

            if (!_manager.ValidateRequest(
                request,
                ModelState))
            {
                return BadRequest(new ValidationProblemDetails(ModelState)
                {
                    Instance = (request?.CorrelationGuid ?? Guid.NewGuid()).ToString()
                });
            }

            try
            {
                long newId = _manager.CreateRequest(request);

                return CreatedAtAction(
                    nameof(GetRequest),
                    new { version = "2", requestId = newId },
                    newId);
            }
            catch (Exception ex)
                when (ex is KeyNotFoundException
                    || ex is ArgumentException)
            {
                ModelState.AddModelError(
                    "Request",
                    ex.Message);

                _logger.Error($"ex.Message::{ex.Message}", ex);

                return BadRequest(new ValidationProblemDetails(ModelState)
                {
                    Instance = correlationGuid.ToString()
                });
            }
            catch (Exception ex)
                when (
                    ex is UnauthorizedAccessException
                    || ex is AuthenticationException)
            {
                _logger.Error($"ex.Message::{ex.Message}", ex);

                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.Error($"ex.Message::{ex.Message}", ex);

                return new ObjectResult(
                    new { Error = $"An unexpected error occurred, please see the logs and reference Id: '{request.CorrelationGuid}'" })
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
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
        public IActionResult GetRequest(long requestId)
        {
            Guid correlationGuid = Guid.NewGuid();

            try
            {
                _logger.Debug($"{nameof(GetRequest)} : {requestId}");

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

                _logger.Error($"ex.Message::{ex.Message}", ex);

                var validationProblemDetails = new ValidationProblemDetails(ModelState)
                {
                    Instance = correlationGuid.ToString()
                };
                return BadRequest(validationProblemDetails);
            }
            catch (Exception ex)
                when (
                    ex is UnauthorizedAccessException
                    || ex is AuthenticationException)
            {
                _logger.Error($"ex.Message::{ex.Message}", ex);

                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.Error($"ex.Message::{ex.Message}", ex);

                return new ObjectResult(
                    new { Error = $"An unexpected error occurred, please see the logs and reference Id: '{correlationGuid}'" })
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
        }


        /// <summary>
        ///    Searches the DB for the request with the request parameters
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("search")]
        public IActionResult Search([FromQuery] SearchRequest request)
        {
            Guid correlationGuid = request.CorrelationGuid;

            try
            {
                _logger.Debug($"{nameof(Search)}: {request}");

                IEnumerable<Request> requests = _manager.Search(request);

                return !requests.SafeAny()
                    ? new JsonResult(Array.Empty<object>())
                    : new JsonResult(requests);
            }
            catch (Exception ex)
                when (ex is ArgumentException)
            {
                ModelState.AddModelError(
                    "Request",
                    ex.Message);

                _logger.Error($"ex.Message::{ex.Message}", ex);

               var validationProblemDetails = new ValidationProblemDetails(ModelState);
                validationProblemDetails.Extensions.Add("correlationGuid", correlationGuid.ToString());
                return StatusCode((int)HttpStatusCode.BadRequest, validationProblemDetails);
            }
            catch (Exception ex)
                when (ex is KeyNotFoundException)
            {
                ModelState.AddModelError(
                    "Request",
                    ex.Message);

                _logger.Error($"ex.Message::{ex.Message}", ex);

                var validationProblemDetails = new ValidationProblemDetails(ModelState);
                validationProblemDetails.Instance = request.CorrelationGuid.ToString();
                return NotFound(validationProblemDetails);
            }

            catch (Exception ex)
                when (
                    ex is UnauthorizedAccessException
                    || ex is AuthenticationException)
            {
                _logger.Error($"ex.Message::{ex.Message}", ex);

                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.Error($"ex.Message::{ex.Message}", ex);

                return new ObjectResult(
                    new { Error = $"An unexpected error occurred, please see the logs and reference Id: '{request.CorrelationGuid}'" })
                { StatusCode = StatusCodes.Status500InternalServerError };
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
            (int)HttpStatusCode.OK,
            "When all is good.")]
        [SwaggerResponse(
            (int)HttpStatusCode.Unauthorized,
            "When unable to authenticate using the basic Authentication.",
            typeof(ValidationProblemResponse))]
        [SwaggerResponse(
            (int)HttpStatusCode.BadRequest,
            "When there is any validation error or json parsing error.",
            typeof(ValidationProblemResponse))]
        [SwaggerResponse(
            (int)HttpStatusCode.InternalServerError,
            "When there is any unhandled exception.",
            typeof(ValidationProblemResponse))]
        public IActionResult SearchByRequestAndNpi(
            string applicationNumber,
            string npi)
        {
            Guid correlationGuid = Guid.NewGuid();

            try
            {
                _logger.Debug($"{nameof(SearchByRequestAndNpi)} started with correlation guid '{correlationGuid}', application number '{applicationNumber}', and npi '{npi}'");

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

                //_logger.LogError(
                //    $"ex.Message::{ex.Message}",
                //    ex,
                //    new Dictionary<string, object> { { nameof(correlationGuid), correlationGuid } });
                _logger.Error($"ex.Message::{ex.Message}, {correlationGuid}");

                return BadRequest(new ValidationProblemDetails(ModelState)
                {
                    Instance = correlationGuid.ToString()
                });
            }
            catch (Exception ex)
                when (
                    ex is UnauthorizedAccessException
                    || ex is AuthenticationException)
            {
                //_logger.LogError(
                //    $"ex.Message::{ex.Message}",
                //    ex,
                //    new Dictionary<string, object> { { nameof(correlationGuid), correlationGuid } });
                _logger.Error($"ex.Message::{ex.Message}, {correlationGuid}");

                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                //_logger.LogError(
                //    $"ex.Message::{ex.Message}",
                //    ex,
                //    new Dictionary<string, object> { { nameof(correlationGuid), correlationGuid } });
                _logger.Error($"ex.Message::{ex.Message}, {correlationGuid}");

                return new ObjectResult(
                    new { Error = $"An unexpected error occurred, please see the logs and reference Id: '{correlationGuid}'" })
                { StatusCode = StatusCodes.Status500InternalServerError };
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
            (int)HttpStatusCode.OK,
            "When all is good.")]
        [SwaggerResponse(
            (int)HttpStatusCode.Unauthorized,
            "When unable to authenticate using the basic Authentication.",
            typeof(ValidationProblemResponse))]
        [SwaggerResponse(
            (int)HttpStatusCode.BadRequest,
            "When there is any validation error or json parsing error.",
            typeof(ValidationProblemResponse))]
        [SwaggerResponse(
            (int)HttpStatusCode.InternalServerError,
            "When there is any unhandled exception.",
            typeof(ValidationProblemResponse))]
        public IActionResult Status(long requestId)
        {
            Guid correlationGuid = Guid.NewGuid();

            try
            {
                _logger.Debug($"{nameof(Status)}: {requestId}");

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

                _logger.Error($"ex.Message::{ex.Message}", ex);

                var validationProblemDetails = new ValidationProblemDetails(ModelState);
                validationProblemDetails.Extensions.Add("correlationGuid", correlationGuid.ToString());
                return StatusCode((int)HttpStatusCode.BadRequest, validationProblemDetails);
            }
            catch (Exception ex)
                when (
                    ex is UnauthorizedAccessException
                    || ex is AuthenticationException)
            {
                _logger.Error($"ex.Message::{ex.Message}", ex);

                return Content(HttpStatusCode.Unauthorized.ToString(), ex.Message);
            }
            catch (Exception ex)
            {
                _logger.Error($"ex.Message::{ex.Message}", ex);

                return Content(HttpStatusCode.InternalServerError.ToString(),
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
            (int)HttpStatusCode.OK,
            "When all is good.")]
        [SwaggerResponse(
            (int)HttpStatusCode.Unauthorized,
            "When unable to authenticate using the basic Authentication.",
            typeof(ValidationProblemResponse))]
        [SwaggerResponse(
            (int)HttpStatusCode.BadRequest,
            "When there is any validation error or json parsing error.",
            typeof(ValidationProblemResponse))]
        [SwaggerResponse(
            (int)HttpStatusCode.InternalServerError,
            "When there is any unhandled exception.",
            typeof(ValidationProblemResponse))]
        public IActionResult UpdateRequest(
            long requestId,
            [FromBody] HplcServiceRequest request)
        {
            _logger.Debug($"{nameof(UpdateRequest)}: {requestId}");

            _manager.ValidateRequest(
                request,
                ModelState);

            return Ok();
        }
    }
}
