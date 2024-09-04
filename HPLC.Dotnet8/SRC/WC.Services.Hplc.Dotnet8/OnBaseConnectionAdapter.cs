﻿namespace WC.Services.Hplc.Dotnet8
{
	//using CareSource.WC.OnBase.Core.Configuration.Interfaces;
	////using CareSource.WC.OnBase.Core.Connection.Interfaces;
	//using CareSource.WC.OnBase.Core.Diagnostics.Interfaces;
	using Hyland.Unity;
    using WC.Services.Hplc.Dotnet8.Models;

    //using WC.Services.Hplc.Models;

    public class OnBaseConnectionAdapter //: IApplicationConnectionAdapter<Application>
	{
		private readonly IConfiguration _configuration;
		private readonly log4net.ILog _logger;
		public OnBaseConnectionAdapter(
			log4net.ILog logger,
			IConfiguration configuration,
   			IHttpRequestResolver requestResolver)
		{
			_logger = logger;
			_configuration = configuration;
			_requestResolver = requestResolver;
		}


		protected Application _application;
		private readonly IHttpRequestResolver _requestResolver;

		public Application Application
		{
			get
			{
				if (_application == null)
				{
					Connect(
						_requestResolver.BasicAuthUserName(),
						_requestResolver.BasicAuthPassword());
				}

				return _application;
			}
		}

		public bool IsConnected
			=> _application != null;

		/// <summary>
		///    Used to connect to the application server.
		/// </summary>
		public Application Connect()
		{
			string onbaseUrl = _configuration["OnBaseSettings:OnBase.Url"];
			string onbaseDataSource = _configuration["OnBaseSettings:OnBase.Datasource"];
			string UserName = _configuration["OnBaseSettings:OnBase.UserName"];
			string Password = _configuration["OnBaseSettings:OnBase.Password"];
			//Create a new Application Object
			AuthenticationProperties AuthProp = Application.CreateOnBaseAuthenticationProperties(onbaseUrl, UserName, Password, onbaseDataSource);

			_logger.Info($"Connecting to OnBase Application Server with Url '{onbaseUrl}' and DataSource '{onbaseDataSource}'.");
			_application = Application.Connect(AuthProp);

			_logger.Info(
				"Successfully connected to OnBase Application Server through Hyland Unity API.");

			return _application;
		}

		public Application Connect(
			string username,
			string password)
		{
			string onbaseUrl = _configuration["OnBaseSettings:OnBase.Url"];
			string onbaseDataSource = _configuration["OnBaseSettings:OnBase.Datasource"];

			//Create a new Application Object
			OnBaseAuthenticationProperties authProps =
				Application.CreateOnBaseAuthenticationProperties(
					onbaseUrl,
					username,
					password,
					onbaseDataSource);

			_logger.Info(
				string.Format(
				"Connecting to OnBase Application Server with Username '{0}', Url '{1}', and DataSource '{2}'.",
				username,
				onbaseUrl,
				onbaseDataSource
				)
			);
			_application = Application.Connect(authProps);

			_logger.Info(
				"Successfully connected to OnBase Application Server through Hyland Unity API.");

			return _application;
		}

		/// <summary>
		///    Used to disconnect from the application server.
		/// </summary>
		public void Disconnect()
		{
			if (IsConnected)
			{
				_application.Disconnect();
				_application = null;

				_logger.Info("Disconnect from OnBase Application Server through Hyland Unity API.");
			}
		}

		public void Dispose()
			=> Disconnect();

		~OnBaseConnectionAdapter()
			=> Disconnect();
	}
}