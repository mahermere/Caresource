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
	[RoutePrefix("api/v{version:apiVersion}/update")]
	public class UpdateController : ApiController
	{
		private readonly ILogger _logger;
		private readonly IUpdateManager _manager;

		public UpdateController(
			IUpdateManager manager,
			ILogger logger)
		{
			_manager = manager;
			_logger = logger;
		}

		[Route("{workViewApplicationName}/{ClassName}/{ObjectId}")]
		[HttpPut]
		[ResponseType(typeof(UpdateResult))]
		public IHttpActionResult Update(
			[FromUri] string workViewApplicationName,
			[FromBody] UpdateRequest request)
		{
			string methodName = $"{nameof(UpdateController)}.{nameof(Update)}";

			using (_logger.BeginScope(
				       new Dictionary<string, string>
				       {
					       { GlobalConstants.CorrelationGuid, correlationGuid.ToString() },
					       { GlobalConstants.Service, GlobalConstants.ServiceName }
				       }
			       ))
			{
				_logger.LogInformation(
					$"Begin {nameof(Update)} {workViewApplicationName}",
					new Dictionary<string, object>() { { "data", request } });

				try
				{
					if (!_manager.ValidateRequest(
						    workViewApplicationName,
						    request,
						    ModelState))
					{
						return BadRequest(ModelState);
					}

					IEnumerable<WorkViewObject> results = _manager.Search(
						workViewApplicationName,
						request);

					_logger.LogInformation(
						$"Successfully updated Application: {workViewApplicationName}");

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

					throw;
				}
				finally
				{
					_logger.LogInformation(
						$"Finished {nameof(Update)} {workViewApplicationName}");
				}
			}
		}
	}
}