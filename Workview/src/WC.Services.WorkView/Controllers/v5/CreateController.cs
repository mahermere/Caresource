// ------------------------------------------------------------------------------------------------
//  <copyright>
//    Copyright (c) CareSource, 2020-2022.  All rights reserved.
// 
//    Workview
//    CreateController.cs
//  </copyright>
//  ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.WorkView.Controllers.v5
{
	using System.Collections.Generic;
	using System.Linq;
	using System.Web.Http;
	using System.Web.Http.Description;
	using CareSource.WC.OnBase.Core.Http.Filters;
	using CareSource.WC.Services.WorkView.Managers.v5;
	using CareSource.WC.Services.WorkView.Models.v5;
	using Microsoft.Web.Http;
	using CareSource.WC.OnBase.Core.ExtensionMethods;
	using Microsoft.Extensions.Logging;
	using System;
	using System.Net;

	[OnBaseAuthorizeFilter]
	[ApiVersion("5")]
	[RoutePrefix("api/v{version:apiVersion}/create")]
	public class CreateController : ApiController
	{
		private readonly ILogger _logger;
		private readonly ICreateManager _workViewApplicationManager;

		public CreateController(
			ICreateManager workViewApplicationManager,
			ILogger logger)
		{
			_workViewApplicationManager = workViewApplicationManager;
			_logger = logger;
		}

		[Route("{workViewApplicationName}")]
		[HttpPost]
		[ResponseType(typeof(CreateResponse))]
		public IHttpActionResult Post(
			[FromUri] string workViewApplicationName,
			[FromBody] CreateRequest request)
		{
			string methodName = $"{nameof(CreateController)}.{nameof(Post)}";

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


					if (!_workViewApplicationManager.ValidateRequest(
						    workViewApplicationName,
						    request,
						    ModelState))
					{
						return BadRequest(ModelState);
					}

					List<WorkViewObject> wvos = _workViewApplicationManager.CreateNewObject(
						workViewApplicationName,
						request).ToList();

					_logger.LogInformation(
						$"Successfully created Object for Class: {wvos.First().ClassName} "+
						$"with Object ID: {wvos.First().Id}");

					return CreatedAtRoute(
						"v5/retrieve",
						new
						{
							workViewApplicationName,
							className = wvos.First().ClassName,
							objectId = wvos.First().Id
						},
						new CreateResponse { Data = wvos, ApplicationName = workViewApplicationName });
				}
				catch (Exception e)
				{
					_logger.LogError(
						e,
						e.Message);

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