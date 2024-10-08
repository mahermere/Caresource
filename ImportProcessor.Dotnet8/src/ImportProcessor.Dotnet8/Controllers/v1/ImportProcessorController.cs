using WC.Services.ImportProcessor.Dotnet8.Managers.v1.Interfaces;
using WC.Services.ImportProcessor.Dotnet8.Models.v1;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WC.Services.ImportProcessor.Dotnet8.Adapters.v1.Interfaces;

namespace WC.Services.ImportProcessor.Dotnet8.Controllers.v1
{
    [Route("api/[controller]")]
    [ApiController]
    public partial class ImportProcessorController : ControllerBase
    {
        private readonly log4net.ILog _logger;
        private readonly IImportProcessorManager<ImportProcessorResponse> _manager;
 
        private readonly IConfiguration _configuration;
        private readonly Guid correlationGuid = Guid.NewGuid();
        public ImportProcessorController(IImportProcessorManager<ImportProcessorResponse> manager, log4net.ILog logger,
                       IConfiguration configuration)
        {
            _manager = manager;
            _logger = logger;
            _configuration = configuration;
        }

    }
}
