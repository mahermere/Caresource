using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using System.Security.Authentication;
using WC.Services.HplcManagement.Dotnet8.Managers.Interfaces;
using WC.Services.HplcManagement.Dotnet8.Models;
using WC.Services.HplcManagement.Dotnet8.Models.Core;
using WC.Services.HplcManagement.Dotnet8.Repository;
using ValidationProblemDetails = WC.Services.HplcManagement.Dotnet8.Models.Core.ValidationProblemDetails;

namespace WC.Services.HplcManagement.Dotnet8.Controllers
{
    /// <summary>
	///    Data and functions describing a CareSource.WC.Services.Hplc.Controllers.v1.RequestController
	///    object.
	/// </summary>
	//[OnBaseAuthorizeFilter]
 //   [RoutePrefix("api/v{version:apiVersion}/request")]
  //  [ApiVersion("1")]
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
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RequestController : ControllerBase
    {
        private readonly log4net.ILog _logger;
        private readonly IRequestManager _manager;
        /// <summary>
		///    Initializes a new instance of the <see cref="RequestController" /> class.
		/// </summary>
		/// <param name="manager">The manager.</param>
		/// <param name="logger">The logger.</param>
        public RequestController(IRequestManager manager, IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration, log4net.ILog logger, IRepository repo)
        {
            _manager = manager;
            _logger = logger;
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
        public IActionResult CreateRequest(
            [FromBody] HplcServiceRequest request)
        {
            string methodName = $"{nameof(RequestController)}.{nameof(CreateRequest)}";

            _logger.DebugFormat(
                    "{0}.{1}: {2}",
                    GetType().Name,
                    nameof(CreateRequest),
                    request);

                Guid correlationGuid = request.CorrelationGuid;

                try
                {
                    if (!_manager.ValidateRequest(
                            request,
                            ModelState))
                    {
                        return BadRequest(
                            new ValidationProblemResponse(
                                ModelState,
                                request?.CorrelationGuid ?? Guid.NewGuid()));
                    }

                    long newId = _manager.CreateRequest(request);

                    return CreatedAtRoute(
                        nameof(GetRequest),
                        new
                        {
                            requestId = newId
                        },
                        newId);
                }
                catch (Exception ex)
                    when (ex is KeyNotFoundException
                            || ex is ArgumentException)
                {
                    _logger.Error($"ex.Message::{ex.Message}", ex);
                    return BadRequest(
                        new ValidationProblemResponse(
                            ModelState,
                            correlationGuid));
                }
                catch (Exception ex)
                    when (
                        ex is UnauthorizedAccessException
                        || ex is AuthenticationException)
                {
                    _logger.Error($"ex.Message::{ex.Message}", ex);
                    return Unauthorized(
                        ex.Message);
                }
                catch (Exception ex)
                {
                    _logger.Error($"ex.Message::{ex.Message}", ex);
                    return StatusCode(
                        StatusCodes.Status500InternalServerError,
                        $"An unexpected error occurred, please see the logs and reference Id: '{request.CorrelationGuid}'");
                }
            
        }

        /// <summary>
        ///    Gets the request.
        /// </summary>
        /// <param name="requestId">The request identifier.</param>
        /// <returns></returns>
        /// <response code="500">If there is an uncaught server exception.</response>
        [HttpGet()]
        [Route("{requestId}", Name = "GetRequest")]
        public IActionResult GetRequest(long requestId)
        {
            Guid correlationGuid = Guid.NewGuid();

            try
            {
                _logger.DebugFormat(
                    "{0}.{1}: {2}",
                    GetType().Name,
                    nameof(GetRequest),
                    requestId);

                Data obj = _manager.GetRequest(requestId);
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
                return BadRequest(
                    new ValidationProblemResponse(
                        ModelState,
                        correlationGuid));
            }
            catch (Exception ex)
                when (
                    ex is UnauthorizedAccessException
                    || ex is AuthenticationException)
            {
                _logger.Error($"ex.Message::{ex.Message}", ex);
                return Unauthorized(
                    ex.Message);
            }
            catch (Exception ex)
            {
                _logger.Error($"ex.Message::{ex.Message}", ex);
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
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
        public IActionResult Search([FromQuery] SearchRequest request)
        {
            Guid correlationGuid = request.CorrelationGuid;

            try
            {
                _logger.Debug($"{nameof(Search)}: {request}");

                IEnumerable<Data> requests = _manager.Search(request);

                return Ok(
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

                _logger.Error($"ex.Message::{ex.Message}", ex);

                return BadRequest(
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

                _logger.Error($"ex.Message::{ex.Message}", ex);
                return NotFound(
                    new ValidationProblemResponse(
                        ModelState,
                        request.CorrelationGuid));
            }

            catch (Exception ex)
                when (
                    ex is UnauthorizedAccessException
                    || ex is AuthenticationException)
            {
                _logger.Error($"ex.Message::{ex.Message}", ex);
                return Unauthorized(
                    ex.Message);
            }
            catch (Exception ex)
            {
                 _logger.Error($"ex.Message::{ex.Message}", ex);
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    $"An unexpected error occurred, please see the logs and reference Id: '{request.CorrelationGuid}'");
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
                    : NotFound("No providers were found matching the Id's provided");
            }
            catch (Exception ex)
                when (ex is KeyNotFoundException
                    || ex is ArgumentException)
            {
                ModelState.AddModelError(
                    "Request",
                    ex.Message);

                _logger.Error($"ex.Message::{ex.Message}, {correlationGuid}");
                return BadRequest(
                    new ValidationProblemResponse(
                        ModelState,
                        correlationGuid));
            }
            catch (Exception ex)
                when (
                    ex is UnauthorizedAccessException
                    || ex is AuthenticationException)
            {
                _logger.Error($"ex.Message::{ex.Message}, {correlationGuid}");

                return Unauthorized(
                    ex.Message);
            }
            catch (Exception ex)
            {
                _logger.Error($"ex.Message::{ex.Message}, {correlationGuid}");
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
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

        public IActionResult Status(long requestId)
        {
            Guid correlationGuid = Guid.NewGuid();

            try
            {
                _logger.Debug($"{nameof(Status)}: {requestId}");

                Data obj = _manager.GetRequest(requestId);
                return Ok(new { Status = obj.Properties[Constants.Request.Status] });
            }
            catch (Exception ex)
                when (ex is KeyNotFoundException
                    || ex is ArgumentException)
            {
                ModelState.AddModelError(
                    "Request",
                    ex.Message);

                _logger.Error($"ex.Message::{ex.Message}, {correlationGuid}");

                return BadRequest(
                    new ValidationProblemResponse(
                        ModelState,
                        correlationGuid));
            }
            catch (Exception ex)
                when (
                    ex is UnauthorizedAccessException
                    || ex is AuthenticationException)
            {
                _logger.Error($"ex.Message::{ex.Message}, {correlationGuid}");

                return Unauthorized(
                    ex.Message);
            }
            catch (Exception ex)
            {
                _logger.Error($"ex.Message::{ex.Message}, {correlationGuid}");

                return StatusCode(
                    StatusCodes.Status500InternalServerError,
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdateRequest(
            long requestId,
            [FromBody] HplcServiceRequest request)
        {
             _logger.Debug($"{nameof(UpdateRequest)}: {requestId}");
            if (!_manager.ValidateRequest(
                    request,
                    ModelState))
            {
                return BadRequest(
                    new ValidationProblemDetails(
                        ModelState));
            }

            return Ok(new NotImplementedException());
        }
    }
}
