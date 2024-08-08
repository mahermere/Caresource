// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2022. All rights reserved.
// 
//   WC.Services.Document
//   DocumentController.Count.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.Document.Controllers.v6
{
	using System.Net;
	using System.Web.Http;
	using CareSource.WC.Services.Document.Managers.v6;
	using CareSource.WC.Services.Document.Models.v6;
	using Microsoft.Extensions.Logging;
	using Microsoft.Web.Http;
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using CareSource.WC.OnBase.Core.ExtensionMethods;
	using CareSource.WC.OnBase.Core.Http.Filters;

	/// <summary>
	///    Version 6 of the Export Controller
	/// </summary>
	[OnBaseAuthorizeFilter]
	[ApiVersion("6")]
	[RoutePrefix("api/v{version:apiVersion}/export")]
	public class ExportController : ApiController
	{
		private readonly ILogger _logger;
		private readonly IExportDocumentManager _manager;

		public ExportController(
			ILogger logger,
			IExportDocumentManager manager)
		{
			_logger = logger;
			_manager = manager;
		}
		/// <summary>
		///    Exports Documents to the specified path
		/// </summary>
		/// <param name="request">The request.</param>	
		/// <remarks>
		///   the total count of document will be returned
		/// </remarks>
		/// <exception cref="Exception"></exception>
		[HttpPost]
		[Route()]
		public IHttpActionResult Export(
			[FromBody]
			ExportDocumentRequest request)
		{
			Stopwatch sw = Stopwatch.StartNew();
			using(_logger.BeginScope(
						new Dictionary<string, string>
						{
							{ GlobalConstants.CorrelationGuid, request.CorrelationGuid.ToString() },
							{ GlobalConstants.Service, GlobalConstants.ServiceName }
						}
					))
			{
				
				string methodName = $"{nameof(ExportController)}.{nameof(Export)}";

				try
				{
					_logger.LogInformation(
						$"Starting {methodName}",
						new Dictionary<string, object> { { "Request", request } },
						Request);

					ISearchResult<DocumentHeader> results = _manager.ExportDocuments(request);

					_logger.LogInformation(
						$"{methodName} results",
						new Dictionary<string, object> { { "Results", results } },
						Request);

					return Ok(results);

				}
				catch (Exception e)
				{
					_logger.LogError(
						e,
						e.Message,
						new Dictionary<string, object>());

					return Content(
						HttpStatusCode.InternalServerError,
						new ExceptionResponse<string>(
							"An unexpected error has occurred",
							(int)HttpStatusCode.InternalServerError,
							request.CorrelationGuid,
							"Please logs and reference the correlation GUID"));
				}
				finally
				{
					_logger.LogInformation($"{methodName} elapsed time: {sw.Elapsed.TotalMilliseconds}ms.");
					_logger.LogInformation($"Finished: {methodName}");
					sw.Stop();
				}
			}
		}

		[HttpPost]
		[Route("Search")]
		public IHttpActionResult Search(
			[FromBody]
			ExportDocumentRequest request)
		{
			Stopwatch sw = Stopwatch.StartNew();
			using (_logger.BeginScope(
						new Dictionary<string, string>
						{
							{ GlobalConstants.CorrelationGuid, request.CorrelationGuid.ToString() },
							{ GlobalConstants.Service, GlobalConstants.ServiceName }
						}
					))
			{
				string methodName = $"{nameof(ExportController)}.{nameof(Search)}";

				try
				{
					_logger.LogInformation(
						$"Starting: {methodName}",
						new Dictionary<string, object> { { "Request", request } },
						Request);

					ISearchResult<DocumentHeader> results = _manager.SearchDocuments(request);

					_logger.LogInformation($"{methodName} results", results);

					return Ok(results);

				}
				catch (Exception e)
				{
					_logger.LogError(
						e,
						e.Message,
						new Dictionary<string, object>());

					return Content(
						HttpStatusCode.InternalServerError,
						new ExceptionResponse<string>(
							"An unexpected error has occurred",
							(int)HttpStatusCode.InternalServerError,
							request.CorrelationGuid,
							"Please logs and reference the correlation GUID"));
				}
				finally
				{
					_logger.LogInformation($"{methodName} elapsed time: {sw.Elapsed.TotalMilliseconds}ms.");
					_logger.LogInformation($"Finished: {methodName}");
					sw.Stop();
				}
			}
		}
	}
}