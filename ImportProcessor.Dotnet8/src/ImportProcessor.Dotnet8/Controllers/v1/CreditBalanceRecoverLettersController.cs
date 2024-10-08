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
        [Route("CBRL")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public IActionResult CBRL()
        {
            string methodName = $"{nameof(ImportProcessorController)}.{nameof(CBRL)}";



            try
            {
                _logger.Info($"Starting {methodName}, Correlation Guid: {correlationGuid}");

                ImportProcessorResponse importProcessorResponse =
                    _manager.CreateOnBaseObjects(
                        "CBRL",
                        correlationGuid);

                _logger.Info(
                    $"Successful Import for Guid: {importProcessorResponse.correlationGuid}.");
                _logger.Info("Finished: CBRL Import.");

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
                _logger.Error(
                    $"Unsuccessful Import for Guid: {correlationGuid}.",
                    exception);

                return BadRequest(
                    new ImportProcessorResponse(
                        ResponseStatus.Error,
                        ErrorCode.UnknownError,
                        exception.Message,
                        correlationGuid
                    ));
            }
            finally
            {
                _logger.Info($"Finished {methodName}");
            }

        }
    }
}
