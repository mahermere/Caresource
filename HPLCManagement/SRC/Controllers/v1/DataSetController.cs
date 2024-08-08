// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2021. All rights reserved.
// 
//   WC.Services.Hplc
//   DataSetController.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace HplcManagement.Controllers.v1
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Net;
	using System.Security.Authentication;
	using System.Web.Http;
	using CareSource.WC.OnBase.Core.Http.Filters;
	using HplcManagement.Managers.v1;
	using HplcManagement.Models.Core;
	using HplcManagement.Models.v1;
	using Microsoft.Extensions.Logging;
	using Microsoft.Web.Http;
	using Swashbuckle.Swagger.Annotations;

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
	[RoutePrefix("api/v{version:apiVersion}/dataset")]
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
			Stopwatch sw = Stopwatch.StartNew();
			string methodName = $"{nameof(DataSetController)}.{nameof(ActionResult)}({dataSetName})";
			using (_logger.BeginScope(
						new Dictionary<string, string>
						{
							{ GlobalConstants.CorrelationGuid, correlationGuid.ToString() },
							{ GlobalConstants.Service, GlobalConstants.ServiceName },
							{ GlobalConstants.Route, $"{GlobalConstants.ServiceName}/dataset/api/v1/{dataSetName}" }
						}
					))
			{
				try
				{
					_logger.LogInformation($"Starting: {methodName}");
					IEnumerable<string> values = _manager.GetDataSet(dataSetName);

					return Ok(values);
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
				finally
				{
					sw.Stop();
					_logger.LogInformation($"Elapsed time(ms): {sw.ElapsedMilliseconds}");
					_logger.LogInformation($"Ending: {methodName}");
				}
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
		public IHttpActionResult AddressTypes()
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
		public IHttpActionResult LocationActionType()
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
		public IHttpActionResult PhoneTypes()
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
		public IHttpActionResult PrimaryStates()
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
		public IHttpActionResult Products()
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
		public IHttpActionResult ProviderTypes()
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
		public IHttpActionResult RequestTypes()
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
		public IHttpActionResult Languages()
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
		public IHttpActionResult AgreementTypes()
			=> ActionResult(
				Constants.DataSetNames.HealthPartnerAgreementType,
				Guid.NewGuid());
	}
}