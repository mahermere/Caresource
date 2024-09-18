using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Hyland.Unity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
using WC.Services.HplcManagement.Dotnet8.Repository;
using WC.Services.HplcManagement.Dotnet8.Authorization;

namespace WC.Services.HplcManagement.Dotnet8.Middlewares
{
    public class OnbaseSetup
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        private Application _app = null;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private const string BASICAUTH = "basic";
        private const string BEARERAUTH = "bearer";
        private IRepository _repository;
        private OnBaseApplicationAbstractFactory _onbaseApplicationFactory = null;


        public OnbaseSetup(RequestDelegate next, ILogger<OnbaseSetup> logger, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _next = next;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Your middleware code here
            _logger.LogInformation("OnbaseSetup middleware executed");

            // Get the repository object
            _repository = (IRepository)context.RequestServices.GetService(typeof(IRepository));

            //Check if the response status code is not 401 (Not Authorized)
            if (context.Response.StatusCode != 401)
            {
                //Setup repository with onbase application object
                string token = _httpContextAccessor.HttpContext.User.Claims.Where<Claim>(c => c.Type == "sessionKey").First<Claim>().Value;
                string type = _httpContextAccessor.HttpContext.User.Claims.Where<Claim>(c => c.Type == "authType").First<Claim>().Value;
                if (type == BASICAUTH)
                    _onbaseApplicationFactory = new OnBaseUserApplicationFactory(_configuration);
                else
                    throw new UnauthorizedAccessException("Invalid authtype!");
                _app = _onbaseApplicationFactory.GetApplication(token);
                _repository.Application = _app;
            }
            // Call the next middleware in the pipeline
            await _next(context);
        }
    }
}


