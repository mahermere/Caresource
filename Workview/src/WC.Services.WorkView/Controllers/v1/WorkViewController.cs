// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   Workview
//   WorkviewController.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.WorkView.Controllers.v1
{
	using System.Collections.Generic;
	using System.Web.Http;
	using System.Web.Http.Description;
	using CareSource.WC.Entities.WorkView;
	using CareSource.WC.OnBase.Core.Diagnostics.Interfaces;
	using CareSource.WC.OnBase.Core.Http.Filters;
	using CareSource.WC.Services.WorkView.Managers;
	using Microsoft.Web.Http;

	[OnBaseAuthorizeFilter]
	[ApiVersion("1")]
	[RoutePrefix("api/v{version:apiVersion}")]
	public class WorkViewController : ApiController
	{
		private readonly ILogger _logger;
		private readonly IWorkViewApplicationManager _WorkViewApplicationManager;

		public WorkViewController(
			IWorkViewApplicationManager WorkViewApplicationManager,
			ILogger logger)
		{
			_WorkViewApplicationManager = WorkViewApplicationManager;
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
		[ResponseType(typeof(WorkviewObject))]
		public IHttpActionResult Get([FromUri] WorkviewObjectRequest request)
		{
			IEnumerable<WorkviewObject> WorkviewObject =
				_WorkViewApplicationManager.FindWorkviewObjects(request);

			_logger.LogDebug(
				"Objects found",
				new Dictionary<string, object> { { "WorkView Objects", WorkviewObject } });

			return Ok(WorkviewObject);
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
		[Route(
			"{id}",
			Name = "")]
		[HttpGet]
		[ResponseType(typeof(WorkviewObject))]
		public IHttpActionResult GetById(
			long id,
			[FromUri] WorkviewObjectRequest request)
		{
			WorkviewObject WorkviewObject = _WorkViewApplicationManager.GetWorkviewObject(
				id,
				request);

			_logger.LogDebug(
				"Object found",
				new Dictionary<string, object> { { "WorkView Object", WorkviewObject } });

			return Ok(WorkviewObject);
		}

		/// <summary>
		///    Add new WorkviewObject to OnBase for a given Application and Class.
		/// </summary>
		/// <param name="request">New WorkviewObject</param>
		/// <returns></returns>
		/// <response code="201">Returns the newly created WorkviewObject.</response>
		/// <response code="400">If given WorkviewObject is invalid.</response>
		/// <response code="500">If there is an uncaught server exception.</response>
		[Route("")]
		[HttpPost]
		[ResponseType(typeof(WorkviewObject))]
		public IHttpActionResult Post(
			[FromBody] WorkviewObjectRequest request)
		{
			if (!_WorkViewApplicationManager.ValidateRequest(
				request,
				ModelState))
			{
				return BadRequest(ModelState);
			}

			WorkviewObject result = _WorkViewApplicationManager.CreateNewObject(request);

			_logger.LogDebug(
				"Object Created",
				new Dictionary<string, object> { { "Object", result } });

			return CreatedAtRoute(
				"GetById",
				new
				{
					id = result.Id
				},
				result);
		}
	}
}