//  ------------------------------------------------------------------------------------------------
//  <copyright>
//    Copyright (c) CareSource, 2020-2022.  All rights reserved.
// 
//    OnBase.Core
//    ServiceLoggerProvider.cs
//  </copyright>
//  ------------------------------------------------------------------------------------------------

namespace HplcManagement
{
	using System;
	using System.Collections.Concurrent;
	using System.Collections.Generic;
	using System.Net.Http;
	using System.Net.Http.Headers;
	using System.Text;
	using System.Threading;
	using System.Threading.Tasks;
	using CareSource.WC.OnBase.Core.Configuration.Interfaces;
	using CareSource.WC.OnBase.Core.Diagnostics.Models;
	using CareSource.WC.OnBase.Core.ExtensionMethods;
	using Microsoft.Extensions.Logging;

	[ProviderAlias("Service")]
	public class ServiceLoggerProvider : BatchingLoggerProvider
	{
		private readonly ConcurrentDictionary<string, ServiceLogger> _loggers =
			new ConcurrentDictionary<string, ServiceLogger>();

		private readonly HttpClient _client;

		~ServiceLoggerProvider()
			=> _client.Dispose();

		public ServiceLoggerProvider(
			ISettingsAdapter config)
			: base(config)
		{
			ServiceLoggerConfiguration settings = new ServiceLoggerConfiguration()
			{
				Token = config.GetSetting("Logging:Service:Token"),
				Url = config.GetSetting("Logging:Service:Url")
			};

			_client = new HttpClient { BaseAddress = new Uri(settings.Url) };

			_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
				"Basic",
				$"Splunk:{settings.Token}".Base64Encode());
		}

		public override ILogger CreateLogger(string categoryName)
			=> _loggers.GetOrAdd(
				"Service",
				name =>
					new ServiceLogger(this));

		protected override async Task WriteMessagesAsync(
			IEnumerable<Tuple<DateTimeOffset, string>> messages,
			CancellationToken cancellationToken)
		{
			foreach (Tuple<DateTimeOffset, string> message in messages)
			{
				HttpContent content = new StringContent(
					message.Item2,
					Encoding.UTF8,
					"application/json");

				await _client.PostAsync(
					_client.BaseAddress,
					content,
					cancellationToken);
			}
		}
	}
}