using Microsoft.AspNetCore.Authorization;
using System.Text.Json.Nodes;

namespace WC.Services.Hplc.Dotnet8.Authorization
{
    public class OnBaseAuthorizationRequirement : IAuthorizationRequirement
    {
       // private IConfiguration _configuration;
        public string SecretKey { get; set; }
        public OnBaseAuthorizationRequirement(IConfiguration configuration)
        {
            this.SecretKey = configuration.GetSection("OnBaseSettings").GetSection("OnBase.Secret").Value;
        }
    }
}
