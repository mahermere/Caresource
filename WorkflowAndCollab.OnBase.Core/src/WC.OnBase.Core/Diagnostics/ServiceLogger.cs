// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2022. All rights reserved.
// 
//   Core
//   ServiceLogger.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.OnBase.Core.Diagnostics
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Linq;
	using System.Web.UI;
	using CareSource.WC.OnBase.Core.Diagnostics.Models;
	using CareSource.WC.OnBase.Core.Helpers.Interfaces;
	using Microsoft.Extensions.Logging;
	using Newtonsoft.Json;

	public class ServiceLogger : ILogger
	{
		private readonly IJsonSerializerHelper _jsonSerializerHelper;
		private readonly ServiceLoggerProvider _provider;

		private LoggingState _state = new LoggingState(
			new Dictionary<string, string>
			{
				{ "Correlation Guid", Guid.NewGuid().ToString() },
				{ "Service", "Unknown - Scope was not set" }
			});

		public ServiceLogger(
			IJsonSerializerHelper jsonSerializerHelper,
			ServiceLoggerProvider provider)
		{
			_jsonSerializerHelper = jsonSerializerHelper;
			_provider = provider;
		}

		public IDisposable BeginScope<TState>(TState state)
		{
				_state = state == null
					? new LoggingState(new Dictionary<string, string>())
					: new LoggingState((IDictionary<string, string>)state);

				return _state;
		}

		public bool IsEnabled(
			LogLevel logLevel)
			=> _provider.Configuration.IsEnabled &&
				(int)(_provider.Configuration
						?.LogLevel?.Default ??
					LogLevel.Debug) <=
				(int)logLevel;

		public void Log<TState>(
			LogLevel logLevel,
			EventId eventId,
			TState state,
			Exception exception,
			Func<TState, Exception, string> formatter)
			=> Log(
				DateTimeOffset.Now,
				logLevel,
				eventId,
				state,
				exception,
				formatter);

		public void Log<TState>(
			DateTimeOffset timestamp,
			LogLevel logLevel,
			EventId eventId,
			TState state,
			Exception exception,
			Func<TState, Exception, string> formatter)
		{
			if ((_provider.Configuration.EventId == 0 ||
					_provider.Configuration.EventId == eventId.Id) &&
				IsEnabled(logLevel))
			{
				Process process = Process.GetCurrentProcess();
				string message = formatter(
					state,
					exception);

				Dictionary<string, object> additionalLogData = state as Dictionary<string, object>;

				SplitRequest(additionalLogData);
				PopulateException(
					additionalLogData,
					exception);

				Message entry = new Message
				{
					Time = DateTimeOffset.Now.ToUniversalTime()
						.ToUnixTimeMilliseconds(),
					Host = Environment.MachineName,
					Source = _state.Service,
					SourceType = process.ProcessName,
					Event = new Event
					{
						Message = message,
						LogLevel = Enum.GetName(
							typeof(LogLevel),
							logLevel)
					},
					Fields = ConvertAdditionalLogData(additionalLogData),
				};

				JsonSerializerSettings settings = new JsonSerializerSettings
				{
					NullValueHandling = NullValueHandling.Ignore,
					DefaultValueHandling = DefaultValueHandling.Ignore,
					DateFormatString = "s"
				};

				string logMessage = _jsonSerializerHelper.ToJson(
					entry,
					settings);

				_provider.AddMessage(
					timestamp,
					logMessage + Environment.NewLine);
			}
		}

		private Dictionary<string, string> ConvertAdditionalLogData(
			Dictionary<string, object> additionalLogData)
		{
			additionalLogData = additionalLogData ?? new Dictionary<string, object>();

			additionalLogData.Remove("CorrelationGuid");
			additionalLogData.Remove("Correlation Guid");
			additionalLogData.Add(
				"Correlation Guid",
				_state.CorrelationGuid);

			return additionalLogData
				.ToDictionary(
					item => item.Key,
					item => JsonConvert.SerializeObject(item.Value));
		}

		private void PopulateException(
			IDictionary<string, object> additionalLogData,
			Exception exception)
		{
			if (exception != null)
			{
				additionalLogData.Add(
					"Stack Trace",
					new StackTrace().ToString());

				additionalLogData.Add(
					"Exception Type",
					exception.GetType()
						.Name);

				additionalLogData.Add(
					"Exception",
					_jsonSerializerHelper.ToJson(exception));
			}
		}

		private void SplitRequest(Dictionary<string, object> additionalLogData)
		{
			if (additionalLogData == null)
			{
				return;
			}

			additionalLogData.TryGetValue(
				"Request",
				out object request);

			if (request == null)
			{
				return;
			}

			IRequest details = (IRequest)request;

			if (!additionalLogData.ContainsKey("Source Application"))
			{
				additionalLogData.Add(
					"Source Application",
					details.SourceApplication);
			}

			if (!additionalLogData.ContainsKey("Current User"))
			{
				additionalLogData.Add(
					"Current User",
					details.UserId);
			}
		}
	}
}