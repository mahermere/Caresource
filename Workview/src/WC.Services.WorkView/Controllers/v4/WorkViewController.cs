// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   Workview
//   WorkViewController.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.WorkView.Controllers.v4
{
	using System;
	using System.Collections.Generic;
	using System.Net;
	using System.Web.Http;
	using System.Web.Http.Description;
	using CareSource.WC.Entities.WorkView;
	using CareSource.WC.Entities.Workview.v2;
	using CareSource.WC.OnBase.Core.Http.Filters;
	using CareSource.WC.Services.WorkView.Managers.v4;
	using Microsoft.Web.Http;
	using WorkViewObject = CareSource.WC.Entities.Workview.v2.WorkviewObject;
	using Microsoft.Extensions.Logging;
	using CareSource.WC.OnBase.Core.ExtensionMethods;


	[OnBaseAuthorizeFilter]
	[ApiVersion("4")]
	[RoutePrefix("api/v{version:apiVersion}")]
	public class WorkViewController : ApiController
	{
		private readonly ILogger _logger;
		private readonly IWorkViewApplicationManager _workViewApplicationManager;
		private readonly Guid CorrelationGuid = Guid.NewGuid();

		public WorkViewController(
			IWorkViewApplicationManager workViewApplicationManager,
			ILogger logger)
		{
			_workViewApplicationManager = workViewApplicationManager;
			_logger = logger;
		}

		/// <summary>
		///    Retrieve all WorkviewObjects that match a given filter.
		/// </summary>
		/// <param name="request">Attributes to filter all WorkviewObjects</param>
		/// <returns></returns>
		/// <response code="200">Returns a list of found WorkviewObjects.</response>
		/// <response code="400">If given filter is invalid.</response>
		/// <response code="500">If there is an uncaught server exception.</response>
		[Route("Search")]
		[HttpGet]
		[ResponseType(typeof(WorkViewObject))]
		public IHttpActionResult Get([FromUri] WorkviewObjectGetRequest request)
		{
			string methodName = $"{nameof(WorkViewController)}.{nameof(Get)}";

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
						new Dictionary<string, object> { { "Correlation Guid", CorrelationGuid } });

					if (!_workViewApplicationManager.ValidateRequest(
						    request,
						    ModelState))
					{
						return BadRequest(ModelState);
					}

					IEnumerable<WorkViewObject> workviewObject =
						_workViewApplicationManager.FindWorkviewObjects(request);

					_logger.LogDebug(
						"Object found",
						new Dictionary<string, object> { { "WorkView Objects", workviewObject } });

					return Ok(workviewObject);
				}
				catch (Exception e)
				{
					_logger.LogError(
						e,
						$"Unsuccessful Get for Guid: {CorrelationGuid}.");

					return Content(HttpStatusCode.InternalServerError,
						$"See log entries for this Correlation Guid: {CorrelationGuid}");
				}
				finally
				{
					_logger.LogInformation($"Finished {methodName}");
				}
			}
		}

		/// <summary>
		///    Retrieve a single WorkviewObject for the given id.
		/// </summary>
		/// <param name="id">Unique Id for WorkviewObject</param>
		/// <param name="request">The request.</param>
		/// <returns></returns>
		/// <response code="200">Returns found WorkviewObject.</response>
		/// <response code="400">If given id is not found or invalid.</response>
		/// <response code="500">If there is an uncaught server exception.</response>
		[Route("{id}", Name = "Get")]
		[HttpGet]
		[ResponseType(typeof(WorkViewObject))]
		public IHttpActionResult GetById(
			long id,
			[FromUri] WorkviewObjectGetRequest request)
		{
			string methodName = $"{nameof(WorkViewController)}.{nameof(GetById)}";

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
						new Dictionary<string, object> { { "Correlation Guid", CorrelationGuid } });

					WorkViewObject workviewObject = _workViewApplicationManager.GetWorkviewObject(
						id,
						request);

					_logger.LogDebug(
						"Object found",
						new Dictionary<string, object> { { "WorkView Object", workviewObject } });

					return Ok(workviewObject);
				}
				catch (Exception e)
				{
					_logger.LogError(
						e,
						$"Unsuccessful Get for Guid: {CorrelationGuid}.");

					return Content(HttpStatusCode.InternalServerError,
						$"See log entries for this Correlation Guid: {CorrelationGuid}");
				}
				finally
				{
					_logger.LogInformation($"Finished {methodName}");
				}
			}
		}

		/// <summary>
		///    Add new WorkviewObject to OnBase for a given Application and Class.
		/// </summary>
		/// <param name="request">New WorkviewObject</param>
		/// <returns></returns>
		/// <response code="200">Returns the newly created WorkviewObject.</response>
		/// <response code="400">If given WorkviewObject is invalid.</response>
		/// <response code="500">If there is an uncaught server exception.</response>
		[Route("batch")]
		[HttpPost]
		[ResponseType(typeof(WorkViewObject))]
		public IHttpActionResult Post(
			[FromBody] WorkviewObjectBatchRequest request)
		{
			string methodName = $"{nameof(WorkViewController)}.{nameof(Post)}";

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
						new Dictionary<string, object> { { "Correlation Guid", CorrelationGuid } });

					if (!_workViewApplicationManager.ValidateRequest(
								    request,
								    ModelState))
					{
						return BadRequest(ModelState);
					}

					IEnumerable<WorkViewObject> result =
						_workViewApplicationManager.CreateNewObjects(request);

					_logger.LogDebug(
						"Objects Created",
						new Dictionary<string, object> { { "Objects", result } });

					return Ok(result);
				}
				catch (Exception e)
				{
					_logger.LogError(
						e,
						$"Unsuccessful Post for Guid: {CorrelationGuid}.");

					return Content(HttpStatusCode.InternalServerError,
						$"See log entries for this Correlation Guid: {CorrelationGuid}");
				}
				finally
				{
					_logger.LogInformation($"Finished {methodName}");
				}
			}
		}

		[Route("")]
		[HttpPost]
		[ResponseType(typeof(WorkViewObject))]
		public IHttpActionResult PostSingle(
			[FromBody] WorkviewObjectPostRequest request)
		{
			string methodName = $"{nameof(WorkViewController)}.{nameof(PostSingle)}";

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
						new Dictionary<string, object> { { "Correlation Guid", CorrelationGuid } });

					if (!_workViewApplicationManager.ValidateRequest(
								    request,
								    ModelState))
					{
						return BadRequest(ModelState);
					}

					WorkViewObject result =
						_workViewApplicationManager.CreateNewObject(request);

					_logger.LogDebug(
						"Object Created",
						new Dictionary<string, object> { { "Object", result } });

					return CreatedAtRoute(
						"Get",
						new { result.Id },
						result.Id);
				}
				catch (Exception e)
				{
					_logger.LogError(
						e,
						$"Unsuccessful Post for Guid: {CorrelationGuid}.");

					return Content(HttpStatusCode.InternalServerError,
						$"See log entries for this Correlation Guid: {CorrelationGuid}");
				}
				finally
				{
					_logger.LogInformation($"Finished {methodName}");
				}
			}
		}

		[Route("update")]
		[HttpPut]
		[ResponseType(typeof(WorkViewObject))]
		public IHttpActionResult Put(
			[FromBody] WorkviewObjectPostRequest request)
		{
			string methodName = $"{nameof(WorkViewController)}.{nameof(Put)}";

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
						new Dictionary<string, object> { { "Correlation Guid", CorrelationGuid } });

					//if (!_workViewApplicationManager.ValidateRequest(
						//	request,
						//	ModelState))
						//{
						//	return BadRequest(ModelState);
						//}

					//IEnumerable<WorkViewObject> result =
					_workViewApplicationManager.UpdateWorkviewObject(request);

					//_logger.LogDebug(
					//	"ObjectUpdated",
					//	new Dictionary<string, object> { { "Objects", result } });

					return Ok("update");
				}
				catch (Exception e)
				{
					_logger.LogError(
						e,
						$"Unsuccessful Put for Guid: {CorrelationGuid}.");

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