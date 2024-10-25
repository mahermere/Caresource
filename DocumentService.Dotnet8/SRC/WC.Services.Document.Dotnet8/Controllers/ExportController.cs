// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2022. All rights reserved.
// 
//   WC.Services.Document
//   DocumentController.Count.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Service.Document.Dotnet8.Controllers
{
	using System.Net;
	using WC.Services.Document.Dotnet8.Models.v6;
	using Microsoft.Extensions.Logging;
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using Microsoft.AspNetCore.Mvc;
    using WC.Services.Document.Dotnet8.Managers.v6;
    using WC.Services.Document.Dotnet8;
    using WC.Services.Document.Dotnet8.Models.v6.Requests;
    using WC.Services.Document.Dotnet8.Models.v6.Interfaces;
    using Microsoft.AspNetCore.Authorization;
    using WC.Services.Document.Dotnet8.Managers.v6.Interfaces;
    using log4net;
    using Newtonsoft.Json;

    /// <summary>
    ///    Version 6 of the Export Controller
    /// </summary>
    [Authorize(Policy = "OnBaseAuthorization")]
	[Route("api/[controller]")]
    [ApiController]
    public class ExportController : ControllerBase
	{
        private readonly log4net.ILog _logger;
        private readonly IExportDocumentManager _manager;

		public ExportController(
            log4net.ILog logger,
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
	//	[HttpPost]
		[HttpPost("Export")]
		public IActionResult Export(
			[FromBody]
			ExportDocumentRequest request)
		{
			Stopwatch sw = Stopwatch.StartNew();
			{
				
				string methodName = $"{nameof(ExportController)}.{nameof(Export)}";

				try
				{
					_logger.Info($"Starting {methodName} - Request: {request}");
										
					ISearchResult<DocumentHeader> results = _manager.ExportDocuments(request);

					_logger.Info($"{methodName} results  Results : {results}");
					 
					return Ok(results);

				}
				catch (Exception e)
				{
					_logger.Error(e.Message, e);
						
					return BadRequest(
						new ExceptionResponse<string>(
							"An unexpected error has occurred",
							(int)HttpStatusCode.InternalServerError,
							request.CorrelationGuid,
							"Please logs and reference the correlation GUID"));
				}
				finally
				{
					_logger.Info($"{methodName} elapsed time: {sw.Elapsed.TotalMilliseconds}ms.");
					_logger.Info($"Finished: {methodName}");
					sw.Stop();
				}
			}
		}

		[HttpPost]
		[Route("Search")]
		public IActionResult Search(
			[FromBody]
			ExportDocumentRequest request)
		{
			Stopwatch sw = Stopwatch.StartNew();
			{
				string methodName = $"{nameof(ExportController)}.{nameof(Search)}";

				try
				{
                    // Logging the start of the method with request details
                    _logger.Info($"Starting: {methodName} - Request: {JsonConvert.SerializeObject(request)}");

                    //var results = _manager.SearchDocuments(request);
                    ISearchResult<DocumentHeader> results = _manager.SearchDocuments(request);
                    // Logging the result of the method
                    _logger.Info($"Results for {methodName}: {JsonConvert.SerializeObject(results)}");

                    return Ok(results);

                }
				catch (Exception e)
				{
					_logger.Error(e.Message, e);

					return BadRequest(
						new ExceptionResponse<string>(
							"An unexpected error has occurred",
							(int)HttpStatusCode.InternalServerError,
							request.CorrelationGuid,
							"Please logs and reference the correlation GUID"));
					
				}
				finally
				{
					_logger.Info($"{methodName} elapsed time: {sw.Elapsed.TotalMilliseconds}ms.");
					_logger.Info($"Finished: {methodName}");
					sw.Stop();
				}
			}
		}
	}
}