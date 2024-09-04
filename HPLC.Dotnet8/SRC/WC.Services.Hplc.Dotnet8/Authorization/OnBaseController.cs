using Hyland.Unity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
using WC.Services.Hplc.Dotnet8.Repository;

namespace WC.Services.Hplc.Dotnet8.Authorization
{
    public class OnBaseController : ControllerBase, IOnBaseController, IDisposable
    {
        private Application _app = null;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private const string BASICAUTH = "basic";
        private const string BEARERAUTH = "bearer";
        private readonly IRepository _repository;
        private OnBaseApplicationAbstractFactory _onbaseApplicationFactory = null;
        public Application Application => _app;
        

        public OnBaseController(IHttpContextAccessor httpContextAccessor, IConfiguration configuration, 
            IRepository repository)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _repository = repository;
            if (!SetupOnBaseConnection())
            {
                throw new UnauthorizedAccessException("Invalid OnBase Credentials");
            }
        }

        private bool SetupOnBaseConnection()
        {
            string token = _httpContextAccessor.HttpContext.User.Claims.Where<Claim>(c => c.Type == "sessionKey").First<Claim>().Value;
            string type = _httpContextAccessor.HttpContext.User.Claims.Where<Claim>(c => c.Type == "authType").First<Claim>().Value;
            if (type == BASICAUTH)
                _onbaseApplicationFactory = new OnBaseUserApplicationFactory(_configuration);
            
            else
                throw new UnauthorizedAccessException("Invalid authtype!");
            _app = _onbaseApplicationFactory.GetApplication(token);
            _repository.Application = _app;
            return true;
        }

        public void Dispose()
        {
            if (_app != null && _app.IsConnected)
            {
                _app.Disconnect();
                _app.Dispose();
            }
        }
    }
}
