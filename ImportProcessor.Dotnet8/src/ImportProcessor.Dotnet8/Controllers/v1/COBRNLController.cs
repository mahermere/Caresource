using CareSource.WC.Entities.Responses;
using WC.Services.ImportProcessor.Dotnet8.Models.v1;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using CareSource.WC.Entities.Exceptions;

namespace WC.Services.ImportProcessor.Dotnet8.Controllers.v1
{
    public partial class ImportProcessorController //: Controller
    {
        [HttpPost]
        [Route("COBRNL")]
        public IActionResult COBRNL()
        {
            string methodName = $"{nameof(ImportProcessorController)}.{nameof(COBRNL)}";

            try
            {
                _logger.Info($"Starting {methodName}, Correlation Guid: {correlationGuid}");

                ImportProcessorResponse importProcessorResponse =
                    _manager.CreateOnBaseObjects(
                        "COB RNL",
                        correlationGuid);

                _logger.Info(
                    $"Successful Import for Guid: {importProcessorResponse.correlationGuid}.");
                _logger.Info("Finished: RNL Import.");

                return Ok(new ImportProcessorResponse
                {
                    status = importProcessorResponse.status,
                    errorCode = importProcessorResponse.errorCode,
                    message = importProcessorResponse.message,
                    correlationGuid = importProcessorResponse.correlationGuid
                });
            }
            catch (Exception exception)
            {
                _logger.Error(
                    $"Unsuccessful Import for Guid: {correlationGuid}.",
                    exception);

                return BadRequest(new ImportProcessorResponse
                {
                    status = ResponseStatus.Error,
                    errorCode = ErrorCode.UnknownError,
                    message = exception.Message,
                    correlationGuid = correlationGuid
                });
            }
            finally
            {
                _logger.Info($"Finished {methodName}");
            }

        }
    }
}
