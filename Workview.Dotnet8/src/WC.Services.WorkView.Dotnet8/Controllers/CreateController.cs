using Hyland.Unity;
using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using WC.Services.WorkView.Dotnet8.Managers.v5;
using WC.Services.WorkView.Dotnet8.Models.v5;
using WC.Services.WorkView.Dotnet8.Repository;

namespace WC.Services.WorkView.Dotnet8.Controllers
{
    [Authorize(Policy = "OnBaseAuthorization")]
    [Route("api/[controller]")]
    public class CreateController : ControllerBase, IDisposable
    {
        private readonly log4net.ILog _logger;
        private readonly ICreateManager _workViewApplicationManager;
        private readonly IRepository _repo;
        public CreateController(ICreateManager workViewApplicationManager, log4net.ILog logger, IRepository repo)
        {
            _logger = logger;
            _repo = repo;
            _workViewApplicationManager = workViewApplicationManager;
        }

        [HttpPost]
        [ProducesResponseType(typeof(CreateResponse), 200)]
        [Route("Post/{workViewApplicationName}")]
        public IActionResult Post(string workViewApplicationName, [FromBody] CreateRequest request)
        {
            string methodName = $"{nameof(CreateController)}.{nameof(Post)}";

            try
            {
                _logger.Info($"Starting {methodName} - Correlation Guid: {request.CorrelationGuid}");

                if (!_workViewApplicationManager.ValidateRequest(
                        workViewApplicationName,
                        request,
                        ModelState))
                {
                    return BadRequest(ModelState);
                }

                List<WorkViewObject> wvos = _workViewApplicationManager.CreateNewObject(
                    workViewApplicationName,
                    request).ToList();

                _logger.Info(
                    $"Successfully created Object for Class: {wvos.First().ClassName} " +
                    $"with Object ID: {wvos.First().Id}"); 


                return StatusCode(
                (int)HttpStatusCode.Created,
                new CreateResponse {SourceApplication = workViewApplicationName, CurrentUser = wvos.First().CreatedBy, Data = wvos, ApplicationName = workViewApplicationName });

            }
            catch (Exception e)
            {
                _logger.Error(e.Message, e);

                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"See log entries for this Correlation Guid: {request.CorrelationGuid}");
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
