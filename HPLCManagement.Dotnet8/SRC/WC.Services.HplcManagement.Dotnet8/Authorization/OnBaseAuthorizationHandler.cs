using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Security.Claims;

namespace WC.Services.HplcManagement.Dotnet8.Authorization
{
    public class OnBaseAuthorizationHandler : AuthorizationHandler<OnBaseAuthorizationRequirement>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;

        public OnBaseAuthorizationHandler(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                                       OnBaseAuthorizationRequirement requirement)
        {
            if (_httpContextAccessor.HttpContext == null || !_httpContextAccessor.HttpContext.Request.Headers.ContainsKey("authorization"))
            {
                return Task.CompletedTask;
            }
            else
            {
                if (!ValidateRequirement(requirement))
                {
                    return Task.CompletedTask;
                }
                else
                {
                    context.Succeed(requirement);
                }
            }
            return Task.CompletedTask;

        }

        private bool ValidateRequirement(OnBaseAuthorizationRequirement requirement)
        {
            string token = _httpContextAccessor.HttpContext.User.Claims.Where<Claim>(c => c.Type == "sessionKey").First<Claim>().Value;
            string type = _httpContextAccessor.HttpContext.User.Claims.Where<Claim>(c => c.Type == "authType").First<Claim>().Value;
            string secretKey = _httpContextAccessor.HttpContext.User.Claims.Where<Claim>(c => c.Type == "secretKey").First<Claim>().Value;
            if (secretKey != requirement.SecretKey)
            {
                return false;
            }

            return true;
        }
    }
}
