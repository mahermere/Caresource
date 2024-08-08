// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   Workview
//   WorkViewController.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.WorkView.Controllers.v5
{
	using System;
	using System.Collections.Generic;
	using System.Web.Http;
	using System.Web.Http.Description;
	using CareSource.WC.OnBase.Core.Http.Filters;
	using CareSource.WC.Services.WorkView.Managers.v5;
	using CareSource.WC.Services.WorkView.Models.v5;
	using Microsoft.Web.Http;
	using Microsoft.Extensions.Logging;
	using CareSource.WC.OnBase.Core.ExtensionMethods;

	[OnBaseAuthorizeFilter]
	[ApiVersion("5")]
	[RoutePrefix("api/v{version:apiVersion}/retrieve")]
	public class RetrieveController : ApiController
	{
		private readonly ILogger _logger;
		private readonly IRetrieveManager _retrieveManager;
		private readonly Guid CorrelationGuid = Guid.NewGuid();

		public RetrieveController(
			IRetrieveManager retrieveManager,
			ILogger logger)
		{
			_retrieveManager = retrieveManager;
			_logger = logger;
		}

		[Route("{workViewApplicationName}/{className}/{objectId}", Name = "v5/retrieve")]
		[HttpGet]
		[ResponseType(typeof(WorkViewObject))]
		public IHttpActionResult GetById(
			[FromUri] string workViewApplicationName,
			[FromUri] string className,
			[FromUri] long objectId)
		{
			string methodName = $"{nameof(RetrieveController)}.{nameof(GetById)}";

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
							{ "Correlation Guid", CorrelationGuid }
						});

					_logger.LogInformation(
						$"Successfully retrieved Application/Class: {workViewApplicationName}/{className} " +
						$"and Object ID: {objectId}");

					return Ok(
						_retrieveManager.GetWorkviewObject(
							workViewApplicationName,
							className,
							objectId));
				}
				catch (Exception e)
				{
					_logger.LogError(
						e,
						e.Message,
						new Dictionary<string, object>()
						{
							{ nameof(workViewApplicationName), workViewApplicationName },
							{ nameof(className), className },
							{ nameof(objectId), objectId }
						});

					throw;
				}
				finally
				{
					_logger.LogInformation($"Finished {methodName}");
				}
			}
		}
	}
}