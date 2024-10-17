using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using WC.Services.WorkView.Dotnet8.Managers.v5;
using WC.Services.WorkView.Dotnet8.Repository;

namespace WC.Services.WorkView.Dotnet8.Controllers
{
    [Authorize(Policy = "OnBaseAuthorization")]
    [Route("api/[controller]")]
    public class DataSetController : ControllerBase, IDisposable
    {
        private readonly log4net.ILog _logger;
        private readonly IDataSetManager _dataSetManager;
        private readonly IRepository _repo;
        private readonly Guid CorrelationGuid = Guid.NewGuid();
        public DataSetController(IDataSetManager dataSetManager, log4net.ILog logger, IRepository repo)
        {
            _logger = logger;
            _repo = repo;
            _dataSetManager = dataSetManager;
        }

        [HttpGet]
        [Route("GetDataSet/{workViewApplicationName}/{className}/{dataSetName}")]
        public IActionResult GetDataSet(string workViewApplicationName, string className, string dataSetName)
        {
            string methodName = $"{nameof(DataSetController)}.{nameof(GetDataSet)}";

            try
            {
                _logger.Info($"Starting {methodName}. WorkView Application: {workViewApplicationName}, Class Name: {className}, Dataset: {dataSetName}");


                IEnumerable<string> response = _dataSetManager.GetDataSetValues(
                    workViewApplicationName,
                    className,
                    dataSetName);

                _logger.Info($"Response for {methodName}. Response: {response}");
                return Ok(response);
            }
            catch (Exception e)
            {
                _logger.Error($"Unsuccessful Import for Guid: {CorrelationGuid}.", e);

                return StatusCode(
                (int)HttpStatusCode.InternalServerError,
                $"See log entries for this Correlation Guid: {CorrelationGuid}");

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
