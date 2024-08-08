// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2021. All rights reserved.
// 
//   ImportProcessor
//   ExplanationOfBenefitsController.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace ImportProcessor.Controllers.v1
{
	using System;
	using System.Collections.Generic;
	using System.Net;
	using System.Web.Http;
	using CareSource.WC.Entities.Exceptions;
	using CareSource.WC.Entities.Responses;
	using ImportProcessor.Models.v1;
	using Microsoft.Extensions.Logging;

	public partial class ImportProcessorController
	{
		[HttpPost]
		[Route("EOB")]
		public IHttpActionResult EOB()
		{
			string methodName = $"{nameof(ImportProcessorController)}.{nameof(EOB)}";

			using (_logger.BeginScope(
						new Dictionary<string, string>
						{
							{ GlobalConstants.CorrelationGuid, correlationGuid.ToString() },
							{ GlobalConstants.Service, GlobalConstants.ServiceName }
						}
					))
			{

				try
				{
					_logger.LogInformation(
						$"Starting {methodName}",
						new Dictionary<string, object> { { "Correlation Guid", correlationGuid } });

					System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
					watch.Start();

					ImportProcessorResponse importProcessorResponse =
						_manager.CreateOnBaseObjects(
							"EOB",
							correlationGuid);

					watch.Stop();
					_logger.LogInformation(
						$"Total CreateDocumentObject Time: {watch.ElapsedMilliseconds}");

					_logger.LogInformation(
						$"Successful Import for Guid: {importProcessorResponse.correlationGuid}.");
					_logger.LogInformation("Finished: EOB Import.");

					return Ok(
						new ImportProcessorResponse(
							importProcessorResponse.status,
							importProcessorResponse.errorCode,
							importProcessorResponse.message,
							importProcessorResponse.correlationGuid
						));
				}
				catch (Exception exception)
				{
					_logger.LogError(
						$"Unsuccessful Import for Guid: {correlationGuid}.",
						exception);

					return Content(
						HttpStatusCode.BadRequest,
						new ImportProcessorResponse(
							ResponseStatus.Error,
							ErrorCode.UnknownError,
							exception.Message,
							correlationGuid
						));
				}
				finally
				{
					_logger.LogInformation($"Finished {methodName}");
				}
			}
		}
	}
}