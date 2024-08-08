// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2023. All rights reserved.
// 
//   WC.Services.HplcManagement
//   OnBaseConnectionAdapter.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace HplcManagement
{
	using CareSource.WC.OnBase.Core.Configuration.Interfaces;
	using CareSource.WC.OnBase.Core.Connection.Interfaces;
	using CareSource.WC.OnBase.Core.ExtensionMethods;
	using HplcManagement.Models;
	using Hyland.Unity;
	using Microsoft.Extensions.Logging;
	using LoggerExtensions = Microsoft.Extensions.Logging.LoggerExtensions;

	public class OnBaseConnectionAdapter : IApplicationConnectionAdapter<Application>
	{
		public OnBaseConnectionAdapter(
			ILogger logger,
			ISettingsAdapter settingsAdapter,
			IHttpRequestResolver requestResolver)
		{
			_logger = logger;
			_settingsAdapter = settingsAdapter;
			_requestResolver = requestResolver;
		}

		private readonly ILogger _logger;
		private readonly ISettingsAdapter _settingsAdapter;

		protected Application _application;
		private readonly IHttpRequestResolver _requestResolver;

		public Application Application
		{
			get
			{
				if (_application == null)
				{
					if (!_requestResolver.GetBasicAuth().IsNullOrWhiteSpace())
					{
						Connect(
							_requestResolver.BasicAuthUserName(),
							_requestResolver.BasicAuthPassword());
					}
					else
					{
						Connect();
					}
				}

				return _application;
			}
		}

		public bool IsConnected
			=> _application != null && _application.IsConnected;

		/// <summary>
		///    Used to connect to the application server.
		/// </summary>
		public Application Connect()
		{
			string onbaseUrl = _settingsAdapter.GetSetting("OnBase.Url");
			string onbaseDataSource = _settingsAdapter.GetSetting("OnBase.DataSource");
			string onbaseDomain = _settingsAdapter.GetSetting("OnBase.Domain");

			//Create a new Application Object
			DomainAuthenticationProperties domainAuthProps =
				Application.CreateDomainAuthenticationProperties(
					onbaseUrl,
					onbaseDataSource);

			domainAuthProps.Domain = onbaseDomain;

			LoggerExtensions.LogInformation(
				_logger,
				$"Connecting to OnBase Application Server with Domain '{onbaseDomain}'"
				+ $", Url '{onbaseUrl}', and DataSource '{onbaseDataSource}'.");

			_application = Application.Connect(domainAuthProps);

			LoggerExtensions.LogInformation(
				_logger,
				"Successfully connected to OnBase Application Server through Hyland Unity API.");

			return _application;
		}

		public Application Connect(
			string username,
			string password)
		{
			string onbaseUrl = _settingsAdapter.GetSetting("OnBase.Url");
			string onbaseDataSource = _settingsAdapter.GetSetting("OnBase.DataSource");

			//Create a new Application Object
			OnBaseAuthenticationProperties authProps =
				Application.CreateOnBaseAuthenticationProperties(
					onbaseUrl,
					username,
					password,
					onbaseDataSource);

			LoggerExtensions.LogInformation(
				_logger,
				$"Connecting to OnBase Application Server with Username '{username}'"
				+ $", Url '{onbaseUrl}', and DataSource '{onbaseDataSource}'.");

			_application = Application.Connect(authProps);

			LoggerExtensions.LogInformation(
				_logger,
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

				LoggerExtensions.LogInformation(
					_logger,
					"Disconnect from OnBase Application Server through Hyland Unity API.");
			}
		}

		public void Dispose()
			=> Disconnect();

		~OnBaseConnectionAdapter()
			=> Disconnect();
	}
}