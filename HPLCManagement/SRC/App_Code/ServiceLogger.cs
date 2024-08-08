// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2022. All rights reserved.
// 
//   Core
//   ServiceLogger.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace HplcManagement
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Linq;
	using CareSource.WC.OnBase.Core.Diagnostics;
	using Microsoft.Extensions.Logging;
	using Newtonsoft.Json;
	using LoggingState = HplcManagement.Models.Core.LoggingState;
	using Message = HplcManagement.Models.Core.Message;

	public class ServiceLogger : ILogger
	{
	
		private readonly ServiceLoggerProvider _provider;

		private LoggingState _state = new LoggingState(
			new Dictionary<string, string>
			{
				{ GlobalConstants.CorrelationGuid, Guid.NewGuid().ToString() },
				{ GlobalConstants.Service, "Unknown - Scope was not set" }
			});

		public ServiceLogger(
			ServiceLoggerProvider provider)
			=> _provider = provider;

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
					Event = new HplcManagement.Models.Core.Event()
					{
						Message = message,
						LogLevel = Enum.GetName(
							typeof(LogLevel),
							logLevel),
						Route = _state.Route
					},
					Fields = ConvertAdditionalLogData(additionalLogData),
				};

				JsonSerializerSettings settings = new JsonSerializerSettings
				{
					NullValueHandling = NullValueHandling.Ignore,
					DefaultValueHandling = DefaultValueHandling.Ignore,
					DateFormatString = "s"
				};

				string logMessage = JsonConvert.SerializeObject(
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
				GlobalConstants.CorrelationGuid,
				_state.CorrelationGuid);
			additionalLogData.Add(
				GlobalConstants.Route,
				_state.Route);

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
					JsonConvert.SerializeObject(exception));
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