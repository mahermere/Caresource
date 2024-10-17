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
    public class SearchController : ControllerBase, IDisposable
    {
        private readonly log4net.ILog _logger;
        private readonly IRepository _repo;
        private readonly ISearchManager _workViewApplicationManager;
        public SearchController(ISearchManager workViewApplicationManager, log4net.ILog logger, IRepository repo)
        {
            _workViewApplicationManager = workViewApplicationManager;
            _logger = logger;
            _repo = repo;
        }

        [Route("Search/{workViewApplicationName}")]
        [HttpGet]
        [ProducesResponseType(typeof(SearchResult), 200)]
        public IActionResult Search(string workViewApplicationName, [FromBody] SearchRequest request)
        {
            string methodName = $"{nameof(SearchController)}.{nameof(Search)}";

            try
            {
                _logger.Info($"Begin {nameof(Search)} {workViewApplicationName}. Data: {request}");


                if (!_workViewApplicationManager.ValidateRequest(
                        workViewApplicationName,
                        request,
                        ModelState))
                {
                    return BadRequest(ModelState);
                }

                IEnumerable<WorkViewObject> results = _workViewApplicationManager.Search(
                    workViewApplicationName,
                    request);

                _logger.Info(
                    $"Successfully retrieved results for Application: {workViewApplicationName}");

                return Ok(
                    new SearchResult()
                    {
                        ClassName = request.ClassName,
                        CorrelationGuid = request.CorrelationGuid,
                        CurrentUser = request.CurrentUser,
                        RequestDateTime = request.RequestDateTime,
                        Results = results,
                        SourceApplication = request.SourceApplication,
                        TotalRecords = results.LongCount()
                    });
            }
            catch (Exception e)
            {
                _logger.Error(e.Message, e);

                // Then return the response as usual
                return Content(((int)HttpStatusCode.InternalServerError).ToString(),
                $"See log entries for this Correlation Guid: {request.CorrelationGuid}");

            }
            finally
            {
                _logger.Info(
                    $"Finished {nameof(Search)} {workViewApplicationName}");
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
