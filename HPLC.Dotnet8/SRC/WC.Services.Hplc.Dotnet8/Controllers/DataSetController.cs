// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2021. All rights reserved.
// 
//   WC.Services.Hplc
//   DataSetController.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WC.Services.Hplc.Dotnet8.Controllers
{
    using Swashbuckle.AspNetCore.Annotations;
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Security.Authentication;
    using WC.Services.Hplc.Dotnet8.Managers.Interfaces;
    using WC.Services.Hplc.Dotnet8.Models;
    using WC.Services.Hplc.Dotnet8.Models.Core;
    using log4net.Core;
    using Microsoft.AspNetCore.Authorization;
    using WC.Services.Hplc.Dotnet8.Authorization;
    using WC.Services.Hplc.Dotnet8.Repository;

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

    //[OnBaseAuthorizeFilter]
    // [RoutePrefix("dataset/api/v{version:apiVersion}")]
    [Authorize(Policy = "OnBaseAuthorization")]
    [ApiController]
    [Route("api/[controller]")]
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

        public DataSetController(IRepository repo, IHttpContextAccessor httpContextAccessor, IConfiguration configuration, log4net.ILog logger, 
        IDataSetManager manager) 
        {
            _logger = logger;
            _repo = repo;
           // _repo.Application = this.Application;
            _manager = manager;
        }

        private IActionResult ActionResult(string dataSetName, Guid correlationGuid)
        {
            try
            {
                IEnumerable<string> primaryStates = _manager.GetDataSet(dataSetName);

                return Ok(primaryStates);
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

                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                //TODO: Verify this response
              _logger.Error($"ex.Message::{ex.Message}", ex);

                return Problem(
                    detail: ex.Message,
                    statusCode: StatusCodes.Status500InternalServerError);
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
        {
            string dataSetName = "ActionType";
            Guid correlationGuid = Guid.NewGuid();
            return ActionResult(dataSetName, correlationGuid);
        }
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
        {
            string dataSetName = Constants.DataSetNames.AddressType;
            Guid correlationGuid = Guid.NewGuid();
            return ActionResult(
                dataSetName,
                correlationGuid);
        }

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
        {
            string dataSetName = Constants.DataSetNames.ActionTypeProviderLocations;
            Guid correlationGuid = Guid.NewGuid();
            return ActionResult(
                dataSetName,
                correlationGuid);
        }

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
        {
            string dataSetName = Constants.DataSetNames.PhoneTypeProvider;
            Guid correlationGuid = Guid.NewGuid();
            return ActionResult(
                dataSetName,
                correlationGuid);
        }

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
        {
            string dataSetName = Constants.DataSetNames.State;
            Guid correlationGuid = Guid.NewGuid();
            return ActionResult(
                dataSetName,
                correlationGuid);
        }

        /// <summary>
        ///    Retrieves all values for the [Products] data set .
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
        {
            string dataSetName = Constants.DataSetNames.ApiProducts;
            Guid correlationGuid = Guid.NewGuid();
            return ActionResult(
                dataSetName,
                correlationGuid);
        }

        /// <summary>
        ///    Retrieves all values for the [ProviderTypes] data set .
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
        {
            string dataSetName = Constants.DataSetNames.ProviderType;
            Guid correlationGuid = Guid.NewGuid();
            return ActionResult(
                dataSetName,
                correlationGuid);
        }

        /// <summary>
        ///    Retrieves all values for the [RequestTypes] data set .
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
        {
            string dataSetName = Constants.DataSetNames.ProviderMaintenanceRequestTypes;
            Guid correlationGuid = Guid.NewGuid();
            return ActionResult(
                dataSetName,
                correlationGuid);
        }

        [HttpGet]
        [Route("Languages")]
        public IActionResult Languages()
        {
            string dataSetName = Constants.DataSetNames.HplcProviderLanguage;
            Guid correlationGuid = Guid.NewGuid();
            return ActionResult(
                dataSetName,
                correlationGuid);
        }

        [HttpGet]
        [Route("AgreementTypes")]
        public IActionResult AgreementTypes()
        {
            string dataSetName = Constants.DataSetNames.HealthPartnerAgreementType;
            Guid correlationGuid = Guid.NewGuid();
            return ActionResult(
                dataSetName,
                correlationGuid);
        }

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
