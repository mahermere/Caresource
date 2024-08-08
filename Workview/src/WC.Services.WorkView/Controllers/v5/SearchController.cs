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
	using System.Linq;
	using System.Net;
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
	[RoutePrefix("api/v{version:apiVersion}/search")]
	public class SearchController : ApiController
	{
		private readonly ILogger _logger;
		private readonly ISearchManager _workViewApplicationManager;

		public SearchController(
			ISearchManager workViewApplicationManager,
			ILogger logger)
		{
			_workViewApplicationManager = workViewApplicationManager;
			_logger = logger;
		}

		[Route("{workViewApplicationName}")]
		[HttpGet]
		[ResponseType(typeof(SearchResult))]
		public IHttpActionResult Search(
			[FromUri] string workViewApplicationName,
			[FromUri] SearchRequest request)
		{
			string methodName = $"{nameof(SearchController)}.{nameof(Search)}";

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
						$"Begin {nameof(Search)} {workViewApplicationName}",
						new Dictionary<string, object>() { { "data", request } });

					if (!_workViewApplicationManager.ValidateRequest(
						    workViewApplicationName,
						    request,
						    ModelState))
					{
						return BadRequest(ModelState);
					}

					IEnumerable<WorkViewObject> results = _workViewApplicationManager.Search(
						workViewApplicationName,
						request);

					_logger.LogInformation(
						$"Successfully retrieved results for Application: {workViewApplicationName}");

					return Ok(
						new SearchResult()
						{
							ClassName = request.ClassName,
							CorrelationGuid = request.CorrelationGuid,
							CurrentUser = request.CurrentUser,
							RequestDateTime = request.RequestDateTime,
							Results = results,
							SourceApplication = request.SourceApplication,
							TotalRecords = results.LongCount()
						});
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
					_logger.LogInformation(
						$"Finished {nameof(Search)} {workViewApplicationName}");
				}
			}
		}
	}
}