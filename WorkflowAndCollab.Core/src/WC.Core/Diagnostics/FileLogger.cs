using System;
using Microsoft.Extensions.Logging;

namespace CareSource.WC.Core.Diagnostics
{
	using System.Collections.Generic;
	using System.Diagnostics;
	using CareSource.WC.Core.Helpers.Interfaces;
	using CareSource.WC.Core.Transaction.Interfaces;

	public class FileLogger : ILogger
	{
		private readonly ITransactionContextManager _transactionContextManager;
		private readonly IJsonSerializerHelper _jsonSerializerHelper;
		private readonly BatchingLoggerProvider _provider;
		private readonly string _category;

		public FileLogger(ITransactionContextManager transactionContextManager
			, IJsonSerializerHelper jsonSerializerHelper
			, BatchingLoggerProvider provider
			, string categoryName)
		{
			_transactionContextManager = transactionContextManager;
			_jsonSerializerHelper = jsonSerializerHelper;
			_provider = provider;
			_category = categoryName;
		}

		public IDisposable BeginScope<TState>(
			TState state) => null;

		public bool IsEnabled(LogLevel logLevel)
			=> _provider.Configuration.IsEnabled && (int)_provider.Configuration
				   .LogLevel.Default <= (int)logLevel;

		public void Log<TState>(
			LogLevel logLevel,
			EventId eventId,
			TState state,
			Exception exception,
			Func<TState, Exception, string> formatter)
		{
			Log(DateTimeOffset.Now, logLevel, eventId, state, exception, formatter);
		}

		public void Log<TState>(DateTimeOffset timestamp, LogLevel logLevel
			, EventId eventId, TState state, Exception exception, Func<TState
				, Exception, string> formatter)
		{
			if ((_provider.Configuration.EventId == 0 ||
			     _provider.Configuration.EventId == eventId.Id) && IsEnabled(logLevel))
			{
				var loggingContext = _transactionContextManager.CopyCurrentContext();

				if (loggingContext?.SecurityContext?.Password != null)
					loggingContext.SecurityContext.Password = null;

				var callingFrame = new StackFrame(7);
				var callingMethod = callingFrame.GetMethod()
					?.Name;
				var callingClass = callingFrame.GetMethod()
					?.DeclaringType?.Name;

				var message = formatter(
					state,
					exception);

				var additionalLogData = state as Dictionary<string, object>;

				var logMessage = _jsonSerializerHelper.ToJson(
					new
					{
						Message = message,
						Method = callingMethod,
						Class = callingClass,
						LogLevel = Enum.GetName(
							typeof(LogLevel),
							logLevel),
						LogDate = timestamp.DateTime,
						Exception = exception,
						AdditionalLogData = additionalLogData,
						TransactionContext = loggingContext
					});

				_provider.AddMessage(
					timestamp,
					logMessage + Environment.NewLine);
			}
		}
	}
}