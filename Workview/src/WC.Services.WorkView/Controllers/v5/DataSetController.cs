// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   WorkView
//   DataSetController.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.WorkView.Controllers.v5
{
	using System;
	using System.Collections.Generic;
	using System.Net;
	using System.Web.Http;
	using CareSource.WC.OnBase.Core.ExtensionMethods;
	using CareSource.WC.OnBase.Core.Http.Filters;
	using CareSource.WC.Services.WorkView.Managers.v5;
	using CareSource.WC.Services.WorkView.Models.v5;
	using Hyland.Unity.Collaboration;
	using Microsoft.Web.Http;
	using Newtonsoft.Json;
	using Microsoft.Extensions.Logging;


	[OnBaseAuthorizeFilter]
	[ApiVersion("5")]
	[RoutePrefix("api/v{version:apiVersion}/dataset")]
	public class DataSetController : ApiController
	{
		private readonly ILogger _logger;
		private readonly IDataSetManager _dataSetManager;
		private readonly Guid CorrelationGuid = Guid.NewGuid();

		public DataSetController(
			IDataSetManager dataSetManager,
			ILogger logger)
		{
			_dataSetManager = dataSetManager;
			_logger = logger;
		}

		[HttpGet]
		[Route("{workViewApplicationName}/{className}/{dataSetName}")]
		public IHttpActionResult GetDataSet(
			[FromUri] string workViewApplicationName,
			[FromUri] string className,
			[FromUri] string dataSetName)
		{
			string methodName = $"{nameof(DataSetController)}.{nameof(GetDataSet)}";
			using (_logger.BeginScope(
				       new Dictionary<string, string>
				       {
					       { GlobalConstants.CorrelationGuid, CorrelationGuid.ToString() },
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
							{ "WorkView Application", workViewApplicationName },
							{ "Class Name", className },
							{ "Dataset",dataSetName }
						});

					IEnumerable<string> response = _dataSetManager.GetDataSetValues(
						workViewApplicationName,
						className,
						dataSetName);

					_logger.LogInformation(
						$"Response for {methodName}.",
						new Dictionary<string, object>
						{
							{ "Response", response },
						});

					return Ok(response);
				}
				catch (Exception e)
				{
					_logger.LogError(
						e,
						$"Unsuccessful Import for Guid: {CorrelationGuid}.");

					return Content(HttpStatusCode.InternalServerError,
						$"See log entries for this Correlation Guid: {CorrelationGuid}");
				}
				finally
				{
					_logger.LogInformation($"Finished {methodName}");
				}
			}
		}
	}
}