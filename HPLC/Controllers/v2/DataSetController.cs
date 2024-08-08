// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2021. All rights reserved.
// 
//   WC.Services.Hplc
//   DataSetController.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.Hplc.Controllers.v2
{
	using System;
	using System.Collections.Generic;
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
	using WC.Services.Hplc;

	/// <summary>
	///    Data and functions describing a CareSource.WC.Services.Hplc.Controllers.v1.DataSetController
	///    object.
	/// </summary>
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
	[OnBaseAuthorizeFilter]
	[RoutePrefix("dataset/api/v{version:apiVersion}")]
	[ApiVersion("2")]
	public class DataSetController : ApiController
	{
		private readonly ILogger _logger;
		private readonly IDataSetManager _manager;

		/// <summary>
		///    Initializes a new instance of the <see cref="DataSetController" /> class.
		/// </summary>
		/// <param name="logger">The logger.</param>
		/// <param name="manager">The manager.</param>
		public DataSetController(
			ILogger logger,
			IDataSetManager manager)
		{
			_logger = logger;
			_manager = manager;
		}

		private IHttpActionResult ActionResult(
			string dataSetName,
			Guid correlationGuid)
		{
			try
			{
				IEnumerable<string> primaryStates = _manager.GetDataSet(dataSetName);

				return Ok(primaryStates);
			}
			catch (Exception ex)
				when (ex is KeyNotFoundException)
			{
				_logger.LogError(
					$"ex.Message::{ex.Message}",
					ex,
					new Dictionary<string, object> { { nameof(correlationGuid), correlationGuid } });

				return BadRequest(ex.Message);
			}
			catch (Exception ex)
				when (ex is UnauthorizedAccessException
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
				//TODO: Verify this response
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
		public IHttpActionResult ActionTypes()
		{
			string dataSetName = "ActionType";
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
		[Route("AddressTypes")]
		public IHttpActionResult AddressTypes()
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
		public IHttpActionResult LocationActionType()
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
		public IHttpActionResult PhoneTypes()
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
		public IHttpActionResult PrimaryStates()
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
		public IHttpActionResult Products()
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
		public IHttpActionResult ProviderTypes()
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
		public IHttpActionResult RequestTypes()
		{
			string dataSetName = Constants.DataSetNames.ProviderMaintenanceRequestTypes;
			Guid correlationGuid = Guid.NewGuid();
			return ActionResult(
				dataSetName,
				correlationGuid);
		}

		[HttpGet]
		[Route("Languages")]
		public IHttpActionResult Languages()
		{
			string dataSetName = Constants.DataSetNames.HplcProviderLanguage;
			Guid correlationGuid = Guid.NewGuid();
			return ActionResult(
				dataSetName,
				correlationGuid);
		}

		[HttpGet]
		[Route("AgreementTypes")]
		public IHttpActionResult AgreementTypes()
		{
			string dataSetName = Constants.DataSetNames.HealthPartnerAgreementType;
			Guid correlationGuid = Guid.NewGuid();
			return ActionResult(
				dataSetName,
				correlationGuid);
		}
	}
}