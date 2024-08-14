using CareSource.WC.OnBase.Core.Configuration.Interfaces;

namespace CareSource.WC.OnBase.Core.Connection
{
    using CareSource.WC.OnBase.Core.Connection.Interfaces;
    using CareSource.WC.OnBase.Core.Diagnostics.Interfaces;
    using Hyland.Unity;

    public class OnBaseConnectionAdapter : IApplicationConnectionAdapter<Application>
    {
        private readonly ILogger _logger;
        private readonly ISettingsAdapter _settingsAdapter;

        public OnBaseConnectionAdapter(ILogger logger, ISettingsAdapter settingsAdapter)
        {
            _logger = logger;
            _settingsAdapter = settingsAdapter;
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
            var onbaseUrl = _settingsAdapter.GetSetting("OnBase.Url");
            var onbaseDataSource = _settingsAdapter.GetSetting("OnBase.DataSource");
            var onbaseDomain = _settingsAdapter.GetSetting("OnBase.Domain");

            //Create a new Application Object
            DomainAuthenticationProperties domainAuthProps =
                Application.CreateDomainAuthenticationProperties(onbaseUrl, onbaseDataSource);
            domainAuthProps.Domain = onbaseDomain;

            _logger.LogInfo($"Connecting to OnBase Application Server with Domain '{onbaseDomain}'" +
                $", Url '{onbaseUrl}', and DataSource '{onbaseDataSource}'.");

            _application = Application.Connect(domainAuthProps);

            _logger.LogInfo($"Successfully connected to OnBase Application Server through Hyland Unity API.");

            return _application;
        }

        public Application Connect(string username, string password)
        {
            var onbaseUrl = _settingsAdapter.GetSetting("OnBase.Url");
            var onbaseDataSource = _settingsAdapter.GetSetting("OnBase.DataSource");

            //Create a new Application Object
            var authProps =
                Application.CreateOnBaseAuthenticationProperties(onbaseUrl, username, password, onbaseDataSource);

            _logger.LogInfo($"Connecting to OnBase Application Server with Username '{username}'" +
                $", Url '{onbaseUrl}', and DataSource '{onbaseDataSource}'.");

            _application = Application.Connect(authProps);

            _logger.LogInfo($"Successfully connected to OnBase Application Server through Hyland Unity API.");

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

                _logger.LogInfo($"Disconnect from OnBase Application Server through Hyland Unity API.");
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