using Hyland.Unity;

namespace WC.Services.OnBase.Dotnet8.Authorization
{
    public class OnBaseUserApplicationFactory : OnBaseApplicationAbstractFactory
    {
        private readonly IConfiguration _configuration;
        public OnBaseUserApplicationFactory(IConfiguration configuration)
        {
            _configuration  = configuration;
        }
        public override Application GetApplication()
        {
            string serverURL = _configuration.GetSection("OnBaseSettings").GetSection("APPSERVERURL").Value;
            string dataSource = _configuration.GetSection("OnBaseSettings").GetSection("DATASOURCE").Value;
            string username = _configuration.GetSection("OnBaseSettings").GetSection("USERNAME").Value;
            string password = _configuration.GetSection("OnBaseSettings").GetSection("PASSWORD").Value;
            AuthenticationProperties AuthProp = Application.CreateOnBaseAuthenticationProperties(serverURL, username, password, dataSource);
            Application application = Application.Connect(AuthProp);
            return application ;
        }

        public override Application GetApplication(string key)
        {
            string serverURL = _configuration.GetSection("OnBaseSettings").GetSection("OnBase.AppServerUrl").Value;
            string dataSource = _configuration.GetSection("OnBaseSettings").GetSection("OnBase.Datasource").Value;
            string username = key.Split(':')[0];
            string password = key.Split(':')[1];
            AuthenticationProperties AuthProp = Application.CreateOnBaseAuthenticationProperties(serverURL, username, password, dataSource);
            Application application = Application.Connect(AuthProp);
            return application;
        }

        // public Application Application = GetApplication();
    }
}
