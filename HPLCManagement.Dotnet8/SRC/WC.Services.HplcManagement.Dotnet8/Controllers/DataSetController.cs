using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Diagnostics;
using System.Net;
using System.Security.Authentication;
using WC.Services.HplcManagement.Dotnet8.Managers.Interfaces;
using WC.Services.HplcManagement.Dotnet8.Models;
using WC.Services.HplcManagement.Dotnet8.Models.Core;
using WC.Services.HplcManagement.Dotnet8.Repository;

namespace WC.Services.HplcManagement.Dotnet8.Controllers
{
    /// <summary>
	///    Data and functions describing a CareSource.WC.Services.Hplc.Controllers.v1.DataSetController
	///    object.
	/// </summary>
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
    
    // [RoutePrefix("api/v{version:apiVersion}/dataset")]
    // [ApiVersion("1")]
    [Authorize(Policy = "OnBaseAuthorization")]
    [Route("api/[controller]")]
    [ApiController]
    public class DataSetController : ControllerBase, IDisposable
    {
        private readonly log4net.ILog _logger;
        private readonly IDataSetManager _manager;
        private readonly IRepository _repo;
        /// <summary>
        ///    Initializes a new instance of the <see cref="DataSetController" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="manager">The manager.</param>
        public DataSetController(IRepository repo, IHttpContextAccessor httpContextAccessor, IConfiguration configuration, log4net.ILog logger, IDataSetManager manager)
        {
             _repo = repo;
            _logger = logger;
            _manager = manager;
        }
        private IActionResult ActionResult(
            string dataSetName,
            Guid correlationGuid)
        {
            Stopwatch sw = Stopwatch.StartNew();
            string methodName = $"{nameof(DataSetController)}.{nameof(ActionResult)}({dataSetName})";  
            try
                {
                    _logger.Info($"Starting: {methodName}");
                    IEnumerable<string> values = _manager.GetDataSet(dataSetName);

                    return Ok(values);
                }
                catch (Exception ex)
                    when (ex is KeyNotFoundException)
                {
                    _logger.Error($"ex.Message::{ex.Message}", ex);
                    return BadRequest(ex.Message);
                }
                catch (Exception ex)
                    when (ex is UnauthorizedAccessException
                            || ex is AuthenticationException)
                {
                    _logger.Error($"ex.Message::{ex.Message}", ex);
                    return new UnauthorizedResult();
                }
                catch (Exception ex)
                {
                    //TODO: Verify this response
                 _logger.Error($"ex.Message::{ex.Message}", ex);
                   return new ObjectResult(new { Message = $"An unexpected error occurred, please see the logs and reference Id: '{correlationGuid}'" })
                    {
                        StatusCode = 500
                    };
                }
                finally
                {
                    sw.Stop();
                    _logger.Info($"Elapsed time(ms): {sw.ElapsedMilliseconds}");
                    _logger.Info($"Ending: {methodName}");
                }
            
        }
        [HttpGet]
        [Route("Test")]
        public IActionResult Test()
        {
            string Msg = "Testing DataSet Endpoint";
            // Guid correlationGuid = Guid.NewGuid();
            return Ok(Msg);
        }
        /// <summary>
        ///    Retrieves all values for the [ActionTypes] data set .
        /// </summary>
        /// <returns>
        ///    An IEnumerable of string with the data set values
        /// </returns>
        /// <response code="200">Returns an array of string values of the DataSet.</response>
        /// <response code="400">If given DataSet does not exist.</response>
        /// <response code="401">if credentials are in valid.</response>
        /// <response code="500">If there is an uncaught server exception.</response>
        [HttpGet]
        [Route("ActionTypes")]
        public IActionResult ActionTypes()
            => ActionResult(
                Constants.DataSetNames.ActionType,
                Guid.NewGuid());
        /// <summary>
        ///    Retrieves all values for the [States] data set .
        /// </summary>
        /// <returns>
        ///    An IEnumerable of string with the data set values
        /// </returns>
        /// <response code="200">Returns an array of string values of the DataSet.</response>
        /// <response code="400">If given DataSet does not exist.</response>
        /// <response code="401">if credentials are in valid.</response>
        /// <response code="500">If there is an uncaught server exception.</response>
        [HttpGet]
        [Route("AddressTypes")]
        public IActionResult AddressTypes()
            => ActionResult(
                Constants.DataSetNames.AddressType,
                Guid.NewGuid());

        /// <summary>
        ///    Retrieves all values for the [States] data set .
        /// </summary>
        /// <returns>
        ///    An IEnumerable of string with the data set values
        /// </returns>
        /// <response code="200">Returns an array of string values of the DataSet.</response>
        /// <response code="400">If given DataSet does not exist.</response>
        /// <response code="401">if credentials are in valid.</response>
        /// <response code="500">If there is an uncaught server exception.</response>
        [HttpGet]
        [Route("LocationActionTypes")]
        public IActionResult LocationActionType()
            => ActionResult(
                Constants.DataSetNames.ActionTypeProviderLocations,
                Guid.NewGuid());

        /// <summary>
        ///    Retrieves all values for the [States] data set .
        /// </summary>
        /// <returns>
        ///    An IEnumerable of string with the data set values
        /// </returns>
        /// <response code="200">Returns an array of string values of the DataSet.</response>
        /// <response code="400">If given DataSet does not exist.</response>
        /// <response code="401">if credentials are in valid.</response>
        /// <response code="500">If there is an uncaught server exception.</response>
        [HttpGet]
        [Route("PhoneTypes")]
        public IActionResult PhoneTypes()
            => ActionResult(
                Constants.DataSetNames.PhoneTypeProvider,
                Guid.NewGuid());

        /// <summary>
        ///    Retrieves all values for the [States] data set .
        /// </summary>
        /// <returns>
        ///    An IEnumerable of string with the data set values
        /// </returns>
        /// <response code="200">Returns an array of string values of the DataSet.</response>
        /// <response code="400">If given DataSet does not exist.</response>
        /// <response code="401">if credentials are in valid.</response>
        /// <response code="500">If there is an uncaught server exception.</response>
        [HttpGet]
        [Route("States")]
        public IActionResult PrimaryStates()
            => ActionResult(
                Constants.DataSetNames.State,
                Guid.NewGuid());

        /// <summary>
        ///    Retrieves all values for the [States] data set .
        /// </summary>
        /// <returns>
        ///    An IEnumerable of string with the data set values
        /// </returns>
        /// <response code="200">Returns an array of string values of the DataSet.</response>
        /// <response code="400">If given DataSet does not exist.</response>
        /// <response code="401">if credentials are in valid.</response>
        /// <response code="500">If there is an uncaught server exception.</response>
        [HttpGet]
        [Route("Products")]
        public IActionResult Products()
            => ActionResult(
                Constants.DataSetNames.ApiProducts,
                Guid.NewGuid());

        /// <summary>
        ///    Retrieves all values for the [States] data set .
        /// </summary>
        /// <returns>
        ///    An IEnumerable of string with the data set values
        /// </returns>
        /// <response code="200">Returns an array of string values of the DataSet.</response>
        /// <response code="400">If given DataSet does not exist.</response>
        /// <response code="401">if credentials are in valid.</response>
        /// <response code="500">If there is an uncaught server exception.</response>
        [HttpGet]
        [Route("ProviderTypes")]
        public IActionResult ProviderTypes()
            => ActionResult(
                Constants.DataSetNames.ProviderType,
                Guid.NewGuid());

        /// <summary>
        ///    Retrieves all values for the [States] data set .
        /// </summary>
        /// <returns>
        ///    An IEnumerable of string with the data set values
        /// </returns>
        /// <response code="200">Returns an array of string values of the DataSet.</response>
        /// <response code="400">If given DataSet does not exist.</response>
        /// <response code="401">if credentials are in valid.</response>
        /// <response code="500">If there is an uncaught server exception.</response>
        [HttpGet]
        [Route("RequestTypes")]
        public IActionResult RequestTypes()
            => ActionResult(
                Constants.DataSetNames.ProviderMaintenanceRequestTypes,
                Guid.NewGuid());

        /// <summary>
        ///    Retrieves all values for the [States] data set .
        /// </summary>
        /// <returns>
        ///    An IEnumerable of string with the data set values
        /// </returns>
        /// <response code="200">Returns an array of string values of the DataSet.</response>
        /// <response code="400">If given DataSet does not exist.</response>
        /// <response code="401">if credentials are in valid.</response>
        /// <response code="500">If there is an uncaught server exception.</response>
        [HttpGet]
        [Route("ProviderLanguages")]
        public IActionResult Languages()
            => ActionResult(
                Constants.DataSetNames.HplcProviderLanguage,
                Guid.NewGuid());

        /// <summary>
        ///    Retrieves all values for the [States] data set .
        /// </summary>
        /// <returns>
        ///    An IEnumerable of string with the data set values
        /// </returns>
        /// <response code="200">Returns an array of string values of the DataSet.</response>
        /// <response code="400">If given DataSet does not exist.</response>
        /// <response code="401">if credentials are in valid.</response>
        /// <response code="500">If there is an uncaught server exception.</response>
        [HttpGet]
        [Route("AgreementTypes")]
        public IActionResult AgreementTypes()
            => ActionResult(
                Constants.DataSetNames.HealthPartnerAgreementType,
                Guid.NewGuid());

        public void Dispose()
        {
            if (_repo.Application != null && _repo.Application.IsConnected)
            {
                _repo.Application.Disconnect();
                _repo.Application.Dispose();
            }
        }
    }
}

