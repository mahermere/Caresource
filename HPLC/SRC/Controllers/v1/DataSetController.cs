﻿// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2021. All rights reserved.
// 
//   WC.Services.Hplc
//   DataSetController.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.Hplc.Controllers.v1
{
	using System;
	using System.Collections.Generic;
	using System.Net;
	using System.Security.Authentication;
	using System.Web.Http;
	using CareSource.WC.OnBase.Core.Diagnostics.Interfaces;
	using Microsoft.Web.Http;
	using Swashbuckle.Swagger.Annotations;
	using WC.Services.Hplc.Managers.v1;
	using WC.Services.Hplc.Models.Core;

	/// <summary>
	///    Data and functions describing a CareSource.WC.Services.Hplc.Controllers.v1.DataSetController
	///    object.
	/// </summary>
	[RoutePrefix("dataset/api/v{version:apiVersion}")]
	[ApiVersion("1")]
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
		[HttpGet]
		[Route("AddressTypes")]
		public IHttpActionResult AddressTypes()
		{
			string dataSetName = "Address Type";
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
		[HttpGet]
		[Route("LocationActionTypes")]
		public IHttpActionResult LocationActionType()
		{
			string dataSetName = "Action Type - Provider Locations";
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
		[HttpGet]
		[Route("PhoneTypes")]
		public IHttpActionResult PhoneTypes()
		{
			string dataSetName = "Phone Type - Provider";
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
		public IHttpActionResult PrimaryStates()
		{
			string dataSetName = "State";
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
		[HttpGet]
		[Route("Products")]
		public IHttpActionResult Products()
		{
			string dataSetName = "API - Products";
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
		[HttpGet]
		[Route("ProviderTypes")]
		public IHttpActionResult ProviderTypes()
		{
			string dataSetName = "ProviderType";
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
		[HttpGet]
		[Route("RequestTypes")]
		public IHttpActionResult RequestTypes()
		{
			string dataSetName = "Provider Maintenance Request Types";
			Guid correlationGuid = Guid.NewGuid();
			return ActionResult(
				dataSetName,
				correlationGuid);
		}
	}
}