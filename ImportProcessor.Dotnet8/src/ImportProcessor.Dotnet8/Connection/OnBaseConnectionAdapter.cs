
namespace WC.Services.ImportProcessor.Dotnet8.Connection
{
    using WC.Services.ImportProcessor.Dotnet8.Connection.Interfaces;
    using Hyland.Unity;
    using System.Configuration;

    public class OnBaseConnectionAdapter : IApplicationConnectionAdapter<Application>
    {
        private readonly log4net.ILog _logger;
        private readonly IConfiguration _configuration;

        public OnBaseConnectionAdapter(log4net.ILog logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        protected Application _application;
        public Application Application
        {
            get
            {
                if (_application == null)
                {
                    Connect();
                }

                return _application;
            }
        }

        public bool IsConnected => _application != null;

        /// <summary>
        /// Used to connect to the application server.
        /// </summary>
        public Application Connect()
        {
            string onbaseUrl = _configuration["OnBaseSettings:OnBase.AppServerUrl"];
            string onbaseDataSource = _configuration["OnBaseSettings:OnBase.Datasource"];
            string UserName = _configuration["OnBaseSettings:OnBase.UserName"];
            string Password = _configuration["OnBaseSettings:OnBase.Password"];

            //Create a new Application Object
            AuthenticationProperties AuthProp = Application.CreateOnBaseAuthenticationProperties(onbaseUrl, UserName, Password, onbaseDataSource);
            
            _logger.Info($"Connecting to OnBase Application Server " + $", Url '{onbaseUrl}', and DataSource '{onbaseDataSource}'.");
            
            _application = Application.Connect(AuthProp);
            
            _logger.Info($"Successfully connected to OnBase Application Server through Hyland Unity API.");
            
            return _application;
        }

        public Application Connect(string username, string password)
        {
            string onbaseUrl = _configuration["OnBaseSettings:OnBase.Url"];
            string onbaseDataSource = _configuration["OnBaseSettings:OnBase.Datasource"];

            //Create a new Application Object
            var authProps = Application.CreateOnBaseAuthenticationProperties(onbaseUrl, username, password, onbaseDataSource);

            _logger.Info($"Connecting to OnBase Application Server with Username '{username}'" +
                $", Url '{onbaseUrl}', and DataSource '{onbaseDataSource}'.");

            _application = Application.Connect(authProps);

            _logger.Info($"Successfully connected to OnBase Application Server through Hyland Unity API.");

            return _application;
        }

        /// <summary>
        /// Used to disconnect from the application server.
        /// </summary>
        public void Disconnect()
        {
            if (IsConnected)
            {
                _application.Disconnect();
                _application = null;

                _logger.Info($"Disconnect from OnBase Application Server through Hyland Unity API.");
            }
        }

        ~OnBaseConnectionAdapter()
        {
            Disconnect();
        }

        public void Dispose()
        {
            Disconnect();
        }
    }
}