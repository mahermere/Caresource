// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   WorkView
//   DataSetController.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.WorkView.Controllers.v4
{
	using System;
	using System.Collections.Generic;
	using System.Net;
	using System.Web.Http;
	using CareSource.WC.OnBase.Core.Http.Filters;
	using CareSource.WC.Services.WorkView.Managers.v4;
	using CareSource.WC.Services.WorkView.Models.v4;
	using Microsoft.Web.Http;
	using Newtonsoft.Json;
	using Microsoft.Extensions.Logging;
	using CareSource.WC.OnBase.Core.ExtensionMethods;

	[OnBaseAuthorizeFilter]
	[ApiVersion("4")]
	[RoutePrefix("api/v{version:apiVersion}/dataset")]
	public class DataSetController : ApiController
	{
		private readonly ILogger _logger;
		private readonly IDataSetManager _dataSetManager;

		public DataSetController(
			IDataSetManager dataSetManager,
			ILogger logger)
		{
			_dataSetManager = dataSetManager;
			_logger = logger;
		}

		[HttpGet]
		[Route("{dataSetName}")]
		public IHttpActionResult GetDataSet(
			string dataSetName,
			[FromUri] WorkViewRequest request)
		{
			string methodName = $"{nameof(DataSetController)}.{nameof(GetDataSet)}";

			using (_logger.BeginScope(
				       new Dictionary<string, string>
				       {
					       { GlobalConstants.CorrelationGuid, request.CorrelationGuid.ToString() },
					       { GlobalConstants.Service, GlobalConstants.ServiceName }
				       }
			       ))
			{
				try
				{
					_logger.LogInformation(
						$"Starting {methodName}",
						new Dictionary<string, object>
						{
							{ "Correlation Guid", request.CorrelationGuid }
						});

					_logger.LogDebug($"DataSetName:{dataSetName}");

					_logger.LogDebug($"Request:{JsonConvert.SerializeObject(request)}");

					DataSetRequest dsRequest = new DataSetRequest(request, dataSetName);

					_logger.LogDebug($"dsRequest:{JsonConvert.SerializeObject(dsRequest)}");

					_logger.LogInformation(
						$"Successful Import for Guid: {request.CorrelationGuid}.");

					return Ok(_dataSetManager.GetDataSetValues(dsRequest));
				}
				catch (Exception e)
				{
					_logger.LogError(
						e,
						$"Unsuccessful Import for Guid: {request.CorrelationGuid}.");

					return Content(HttpStatusCode.InternalServerError,
						$"See log entries for this Correlation Guid: {request.CorrelationGuid}");
				}
				finally
				{
					_logger.LogInformation($"Finished {methodName}");
				}
			}
		}
	}
}