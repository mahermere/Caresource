// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2021. All rights reserved.
// 
//   ImportProcessor
//   CreditBalanceRecoverLettersController.cs
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
		[Route("CBRL")]
		public IHttpActionResult CBRL()
		{
			string methodName = $"{nameof(ImportProcessorController)}.{nameof(CBRL)}";

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

					ImportProcessorResponse importProcessorResponse =
						_manager.CreateOnBaseObjects(
							"CBRL",
							correlationGuid);

					_logger.LogInformation(
						$"Successful Import for Guid: {importProcessorResponse.correlationGuid}.");
					_logger.LogInformation("Finished: CBRL Import.");

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