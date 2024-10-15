using Hyland.Public;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace WC.Services.OnBase.Dotnet8.Authorization
{
    public class OnBaseAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private const string BASICAUTH = "basic";
        private const string BEARERAUTH = "bearer";
        public OnBaseAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock
            ) : base(options, logger, encoder, clock)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var authHeader = Request.Headers["Authorization"].ToString();
            if (authHeader != null && authHeader.StartsWith("basic", StringComparison.OrdinalIgnoreCase))
            {
                string onbaseToken = "";
                string onbaseAuthType = "";
                string secretKey = "";
                if (!TryRetrieveToken(out onbaseToken, out onbaseAuthType, out secretKey))
                {
                    Response.StatusCode = 401;
                    Response.Headers.Add("WWW-Authenticate", "Basic realm=\"cloudonbase.com\"");
                    return Task.FromResult(AuthenticateResult.Fail("Invalid Authorization Header"));
                }
                else
                {
                    var onbaseIdentity = new ClaimsIdentity(
                        new Claim[]
                        {
                         new Claim("sessionKey", onbaseToken),
                         new Claim("authType", onbaseAuthType),
                         new Claim("secretKey", secretKey),
                        }, onbaseToken);
                    ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(onbaseIdentity);
                    return Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(claimsPrincipal, Scheme.Name)));
                }

            }
            else
            {
                Response.StatusCode = 401;
                Response.Headers.Add("WWW-Authenticate", "Basic realm=\"cloudonbase.com\"");
                return Task.FromResult(AuthenticateResult.Fail("Invalid Authorization Header"));
            }
        }

        private bool TryRetrieveToken(out string token, out string type, out string secretKey)
        {
            token = null;
            type = null;
            secretKey = null;
            if (!Request.Headers.ContainsKey("Authorization"))
            {
                return false;
            }
            if (!Request.Headers.ContainsKey("secret-key"))
            {
                return false;
            }
            Microsoft.Extensions.Primitives.StringValues secretKeys;
            Request.Headers.TryGetValue("secret-key", out secretKeys);
            secretKey = secretKeys.First<string>();

            //request header{
            // content-type: application/json
            // authorization: basic rsapkal:MUSTEXIST
            //}
            string authzHeader = "";
            Microsoft.Extensions.Primitives.StringValues authzHeaders;
            Request.Headers.TryGetValue("Authorization", out authzHeaders);
            authzHeader = authzHeaders.First<string>();

            token = authzHeader.StartsWith("Bearer ", StringComparison.Ordinal) ? authzHeader.Split(' ')[1] : null;
            if (null == token)
            {
                token = authzHeader.StartsWith("Basic ", StringComparison.Ordinal) ? authzHeader.Split(' ')[1] : null;
                token = Base64Decode(token);
                if (null == token)
                {
                    return false;
                }
                else
                {
                    type = BASICAUTH;
                    string username = token.Split(':')[0];
                    string password = token.Split(':').Length > 1 ? token.Split(':')[1] : null;
                    token = username + ":" + password;
                    return true;
                }
            }
            else
            {
                type = BEARERAUTH;
                return true;
            }
        }

        public string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }
}
