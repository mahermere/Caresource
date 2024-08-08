// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2023. All rights reserved.
// 
//   WC.Services.Document
//   Class1.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Web.Http;
using CareSource.WC.Services.Document.Models.v6;
using Microsoft.Extensions.Logging;

namespace CareSource.WC.Services.Document
{
	using System;
	using System.Collections.Concurrent;
	using System.Collections.Generic;
	using System.Threading;
	using System.Threading.Tasks;
	using Microsoft.Extensions.Logging;
	using System.Diagnostics;
	using System.Linq;
	using System.Net.Http;
	using System.Net.Http.Headers;
	using System.Text;
	using CareSource.WC.OnBase.Core.Configuration.Interfaces;
	using CareSource.WC.OnBase.Core.Diagnostics.Models;
	using CareSource.WC.OnBase.Core.ExtensionMethods;
	using CareSource.WC.OnBase.Core.Helpers.Interfaces;
	using CareSource.WC.Services.Document.Models.v6;
	using Newtonsoft.Json;


	public class ServiceLogger : ILogger
	{
		public ServiceLogger(
			IJsonSerializerHelper jsonSerializerHelper,
			ServiceLoggerProvider provider)
		{
			_jsonSerializerHelper = jsonSerializerHelper;
			_provider = provider;
		}

		private readonly IJsonSerializerHelper _jsonSerializerHelper;
		private readonly ServiceLoggerProvider _provider;

		private LoggingState _state = new LoggingState(
			new Dictionary<string, string>
			{
				{
					"Correlation Guid", Guid.NewGuid()
						.ToString()
				},
				{ "Service", "Unknown - Scope was not set" }
			});

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

				IDictionary<string, object> additionalLogData = state as Dictionary<string, object>;

				SplitRequest(additionalLogData);
				PopulateException(
					ref additionalLogData,
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
			IDictionary<string, object> additionalLogData)
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
			ref IDictionary<string, object> additionalLogData,
			Exception exception)
		{
			additionalLogData = additionalLogData ?? new Dictionary<string, object>();
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

		private void SplitRequest(IDictionary<string, object> additionalLogData)
		{
			if (additionalLogData == null)
			{
				additionalLogData = new Dictionary<string, object>();
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

	[ProviderAlias("Service")]
	public class ServiceLoggerProvider : BatchingLoggerProvider
	{
		public ServiceLoggerProvider(
			IJsonSerializerHelper jsonSerializerHelper,
			ISettingsAdapter config)
			: base(config)
		{
			ServiceLoggerConfiguration settings = new ServiceLoggerConfiguration()
			{
				Token = config.GetSetting("Logging:Service:Token"),
				Url = config.GetSetting("Logging:Service:Url")
			};

			_jsonSerializerHelper = jsonSerializerHelper;

			_client = new HttpClient { BaseAddress = new Uri(settings.Url) };

			_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
				"Basic",
				$"Splunk:{settings.Token}".Base64Encode());
		}

		private readonly HttpClient _client;

		private readonly IJsonSerializerHelper _jsonSerializerHelper;

		private readonly ConcurrentDictionary<string, ServiceLogger> _loggers =
			new ConcurrentDictionary<string, ServiceLogger>();

		public override ILogger CreateLogger(string categoryName)
			=> _loggers.GetOrAdd(
				"Service",
				name =>
					new ServiceLogger(
						_jsonSerializerHelper,
						this));

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

		~ServiceLoggerProvider()
			=> _client.Dispose();
	}

	public abstract class BatchingLoggerProvider : ILoggerProvider
	{
		protected BatchingLoggerProvider(ISettingsAdapter config)
		{
			Configuration = new ServiceLoggerConfiguration();

			Configuration.BackgroundQueueSize = config.GetSetting(
				"Logging:Batch:BackGroundQueueSize",
				Configuration.BackgroundQueueSize);

			Configuration.BatchSize = config.GetSetting(
				"Logging:Batch:BatchSize",
				Configuration.BatchSize);

			Configuration.FlushPeriod = config.GetSetting(
				"Logging:Batch:BackGroundQueueSize",
				Configuration.BackgroundQueueSize);

			Configuration.EventId = config.GetSetting(
				"Logging:Batch:EventId",
				Configuration.EventId);

			Configuration.LogLevel = new LogLevelConfiguration();

			Configuration.LogLevel.Default =
				(LogLevel)Enum.Parse(
					typeof(LogLevel),
					config.GetSetting(
						"Logging:Batch:LogLevel:Default",
						Configuration.LogLevel.Default.ToString()));

			if (Configuration.BatchSize <= 0)
			{
				throw new ArgumentOutOfRangeException(
					nameof(Configuration.BatchSize),
					$"{nameof(Configuration.BatchSize)} must be a positive number.");
			}

			if (Configuration.FlushPeriod <= 0)
			{
				throw new ArgumentOutOfRangeException(
					nameof(Configuration.FlushPeriod),
					$"{nameof(Configuration.FlushPeriod)} must be longer than zero.");
			}

			if (Configuration.IsEnabled)
			{
				Start();
			}
		}

		private readonly List<Tuple<DateTimeOffset, string>> _currentBatch =
			new List<Tuple<DateTimeOffset, string>>();

		private CancellationTokenSource _cancellationTokenSource;
		private BlockingCollection<Tuple<DateTimeOffset, string>> _messageQueue;
		private Task _outputTask;

		public BatchLoggerConfiguration Configuration { get; }

		public void AddMessage(
			DateTimeOffset timestamp,
			string message)
		{
			if (!_messageQueue.IsAddingCompleted)
			{
				try
				{
					_messageQueue.Add(
						new Tuple<DateTimeOffset, string>(
							timestamp,
							message),
						_cancellationTokenSource.Token);
				}
				catch
				{
					//cancellation token canceled or CompleteAdding called
				}
			}
		}

		public abstract ILogger CreateLogger(string categoryName);

		public void Dispose()
		{
			if (Configuration.IsEnabled)
			{
				Stop();
			}
		}

		protected virtual Task IntervalAsync(
			TimeSpan interval,
			CancellationToken cancellationToken)
			=> Task.Delay(
				interval,
				cancellationToken);

		protected abstract Task WriteMessagesAsync(
			IEnumerable<Tuple<DateTimeOffset, string>> messages,
			CancellationToken token);

		private async Task ProcessLogQueue()
		{
			while (!_cancellationTokenSource.IsCancellationRequested)
			{
				int limit = Configuration.BatchSize ?? int.MaxValue;

				while (limit > 0 &&
						_messageQueue.TryTake(out Tuple<DateTimeOffset, string> message))
				{
					_currentBatch.Add(message);
					limit--;
				}

				if (_currentBatch.Count > 0)
				{
					try
					{
						await WriteMessagesAsync(
							_currentBatch,
							_cancellationTokenSource.Token);
					}
					catch
					{
						// ignored
					}

					_currentBatch.Clear();
				}

				await IntervalAsync(
					TimeSpan.FromMilliseconds(Configuration.FlushPeriod.Value),
					_cancellationTokenSource.Token);
			}
		}

		private void Start()
		{
			CreateLogger("Service");
			_messageQueue = Configuration.BackgroundQueueSize == null
				? new BlockingCollection<Tuple<DateTimeOffset, string>>(
					new ConcurrentQueue<Tuple<DateTimeOffset, string>>())
				: new BlockingCollection<Tuple<DateTimeOffset, string>>(
					new ConcurrentQueue<Tuple<DateTimeOffset, string>>(),
					Configuration.BackgroundQueueSize.Value);

			_cancellationTokenSource = new CancellationTokenSource();
			_outputTask = Task.Run(ProcessLogQueue);
		}

		private void Stop()
		{
			_cancellationTokenSource.Cancel();
			_messageQueue.CompleteAdding();

			try
			{
				_outputTask.Wait(TimeSpan.FromMinutes(Configuration.FlushPeriod.Value));
			}
			catch (TaskCanceledException) { }
			catch (AggregateException ex) when (ex.InnerExceptions.Count == 1 &&
															ex.InnerExceptions[0] is TaskCanceledException)
			{ }
		}
	}
}




public static class LoggerExtensions
{
	public static void LogCritical(
		this ILogger logger,
		Exception exception,
		string message,
		IDictionary<string, object> additionalLogData = null,
		HttpRequestMessage httpRequest = null,
		[CallerMemberName]
		string memberName = "",
		[CallerFilePath]
		string sourceFilePath = "",
		[CallerLineNumber]
		int sourceLineNumber = 0)
		=> Log(
			logger,
			LogLevel.Critical,
			message,
			exception,
			httpRequest,
			additionalLogData,
			memberName,
			sourceFilePath,
			sourceLineNumber);

	public static void LogDebug(
		this ILogger logger,
		string message,
		IDictionary<string, object> additionalLogData = null,
		HttpRequestMessage httpRequest = null,
		[CallerMemberName]
		string memberName = "",
		[CallerFilePath]
		string sourceFilePath = "",
		[CallerLineNumber]
		int sourceLineNumber = 0)
		=> Log(
			logger,
			LogLevel.Debug,
			message,
			null,
			httpRequest,
			additionalLogData,
			memberName,
			sourceFilePath,
			sourceLineNumber);

	public static void LogError(
		this ILogger logger,
		Exception exception,
		string message,
		IDictionary<string, object> additionalLogData = null,
		HttpRequestMessage httpRequest = null,
		[CallerMemberName]
		string memberName = "",
		[CallerFilePath]
		string sourceFilePath = "",
		[CallerLineNumber]
		int sourceLineNumber = 0)
		=> Log(
			logger,
			LogLevel.Error,
			message,
			exception,
			httpRequest,
			additionalLogData,
			memberName,
			sourceFilePath,
			sourceLineNumber);

	public static void LogInformation(
		this ILogger logger,
		string message,
		IDictionary<string, object> additionalLogData = null,
		HttpRequestMessage httpRequest = null,
		[CallerMemberName]
		string memberName = "",
		[CallerFilePath]
		string sourceFilePath = "",
		[CallerLineNumber]
		int sourceLineNumber = 0)
		=> Log(
			logger,
			LogLevel.Information,
			message,
			null,
			httpRequest,
			additionalLogData,
			memberName,
			sourceFilePath,
			sourceLineNumber);

	public static void LogTrace(
		this ILogger logger,
		string message,
		IDictionary<string, object> additionalLogData = null,
		HttpRequestMessage httpRequest = null,
		[CallerMemberName]
		string memberName = "",
		[CallerFilePath]
		string sourceFilePath = "",
		[CallerLineNumber]
		int sourceLineNumber = 0)
		=> Log(
			logger,
			LogLevel.Trace,
			message,
			null,
			httpRequest,
			additionalLogData,
			memberName,
			sourceFilePath,
			sourceLineNumber);

	public static void LogWarning(
		this ILogger logger,
		string message,
		IDictionary<string, object> additionalLogData = null,
		HttpRequestMessage httpRequest = null,
		[CallerMemberName]
		string memberName = "",
		[CallerFilePath]
		string sourceFilePath = "",
		[CallerLineNumber]
		int sourceLineNumber = 0)
		=> Log(
			logger,
			LogLevel.Warning,
			message,
			null,
			httpRequest,
			additionalLogData,
			memberName,
			sourceFilePath,
			sourceLineNumber);

	private static void Log(
		ILogger logger,
		LogLevel logLevel,
		string message,
		Exception exception,
		HttpRequestMessage httpRequest,
		IDictionary<string, object> additionalLogData,
		string memberName,
		string sourceFilePath,
		int sourceLineNumber)
	{
		additionalLogData = AddAdditionalLogData(
			additionalLogData,
			httpRequest,
			memberName,
			sourceFilePath,
			sourceLineNumber);

		logger.Log(
			logLevel,
			default(int),
			additionalLogData,
			exception,
			(
				s,
				e) => message);
	}

	public static void LogMethodStart(
		this ILogger logger,
		string methodName,
		IRequest request,
		HttpRequestMessage httpRequest = null,
		[CallerMemberName]
		string memberName = "",
		[CallerFilePath]
		string sourceFilePath = "",
		[CallerLineNumber]
		int sourceLineNumber = 0)
		=> Log(
			logger,
			LogLevel.Information,
			$"Starting {methodName} ",
			null,
			httpRequest,
			null,
			memberName,
			sourceFilePath,
			sourceLineNumber);

	public static void LogMethodFinish(
		this ILogger logger,
		string methodName,
		[CallerMemberName]
		string memberName = "",
		[CallerFilePath]
		string sourceFilePath = "",
		[CallerLineNumber]
		int sourceLineNumber = 0)
		=> Log(
			logger,
			LogLevel.Information,
			$"Finished {methodName} ",
			null,
			null,
			null,
			memberName,
			sourceFilePath,
			sourceLineNumber);


	private static IDictionary<string, object> AddAdditionalLogData(
		IDictionary<string, object> additionalLogData,
		HttpRequestMessage httpRequest,
		string memberName,
		string sourceFilePath,
		int sourceLineNumber)
	{
		additionalLogData = additionalLogData ?? new Dictionary<string, object>();

		additionalLogData.Add(
			"Caller Member Name",
			memberName);

		additionalLogData.Add(
			"Caller File Path",
			sourceFilePath);

		additionalLogData.Add(
			"Caller Line Number",
			sourceLineNumber);

		if (httpRequest != null)
		{
			additionalLogData.Add(
				"URI",
				httpRequest.RequestUri.AbsoluteUri);

			additionalLogData.Add(
				"Query String",
				httpRequest.RequestUri.Query);

			additionalLogData.Add(
				"API Version",
				httpRequest.GetRequestedApiVersion()
					?.MajorVersion);

			additionalLogData.Add(
				"Method",
				httpRequest.Method);
		}

		return additionalLogData;
	}
}