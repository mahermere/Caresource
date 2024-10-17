using Microsoft.AspNetCore.Mvc;
using WC.Services.WorkView.Dotnet8.Models.v5;
using WC.Services.WorkView.Dotnet8.Managers.v5;
using log4net;
using Microsoft.AspNetCore.Authorization;
using System.Net;
using WC.Services.WorkView.Dotnet8.Repository;

namespace WC.Services.WorkView.Dotnet8.Controllers
{
    [Authorize(Policy = "OnBaseAuthorization")]
    [Route("api/[controller]")]
    public class RetrieveController : ControllerBase, IDisposable
    {
        private readonly log4net.ILog _logger;
        private readonly IRetrieveManager _retrieveManager;
        private readonly IRepository _repo;
        private readonly Guid CorrelationGuid = Guid.NewGuid();
        public RetrieveController(IRetrieveManager retrieveManager, log4net.ILog logger, IRepository repo)
        {
            _logger = logger;
            _repo = repo;
            _retrieveManager = retrieveManager;
        }

        [Route("GetById/{workViewApplicationName}/{className}/{objectId}")]
        [HttpGet]
        [ProducesResponseType(typeof(WorkViewObject), 200)]
        public IActionResult GetById(string workViewApplicationName, string className, long objectId)
        {
            string methodName = $"{nameof(RetrieveController)}.{nameof(GetById)}";

            try
            {
                _logger.Info($"Starting {methodName}. Correlation Guid: {CorrelationGuid}");

                _logger.Info($"Successfully retrieved Application/Class: {workViewApplicationName}/{className} and Object ID: {objectId}");

                return Ok(
                    _retrieveManager.GetWorkviewObject(
                        workViewApplicationName,
                        className,
                        objectId));
            }
            catch (Exception e)
            {
                _logger.Error($"Error occurred: {e.Message}. Application: {workViewApplicationName}, Class: {className}, Object ID: {objectId}", e);

                throw;
            }
            finally
            {
                _logger.Info($"Finished {methodName}");
            }

        }

        public void Dispose()
        {
            if (_repo.Application != null && _repo.Application.IsConnected)
            {
                _repo.Application.Disconnect();
                _repo.Application.Dispose();
            }
        }
    }
}
