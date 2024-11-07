// ------------------------------------------------------------------------------------------------
//  <copyright>
//    Copyright (c) CareSource, 2020-2022.  All rights reserved.
// 
//    FXIAuthentication
//    FileLogger.cs
//  </copyright>
//  ------------------------------------------------------------------------------------------------

namespace FXIAuthentication.Core.Diagnostics;

using System.Diagnostics;
using FXIAuthentication.Core.Helpers;
using FXIAuthentication.Core.Transaction;
using FXIAuthentication.Core.Transaction.Models;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

public class FileLogger : ILogger
{
	private readonly string _category;
	private readonly IJsonSerializerHelper _jsonSerializerHelper;
	private readonly BatchingLoggerProvider _provider;
	private readonly ITransactionContextManager _transactionContextManager;

	public FileLogger(ITransactionContextManager transactionContextManager,
		IJsonSerializerHelper jsonSerializerHelper,
		BatchingLoggerProvider provider,
		string categoryName)
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
		Func<TState, Exception, string> formatter) =>
		Log(DateTimeOffset.Now, logLevel, eventId, state, exception, formatter);

	public void Log<TState>(DateTimeOffset timestamp,
		LogLevel logLevel,
		EventId eventId,
		TState state,
		Exception exception,
		Func<TState
			, Exception, string> formatter)
	{
		if ((_provider.Configuration.EventId == 0 ||
		     _provider.Configuration.EventId == eventId.Id) && IsEnabled(logLevel))
		{
			TransactionContext loggingContext = _transactionContextManager.CopyCurrentContext();

			if (loggingContext?.SecurityContext?.Password != null)
			{
				loggingContext.SecurityContext.Password = null;
			}

			StackFrame callingFrame = new(7);
			string? callingMethod = callingFrame.GetMethod()
				?.Name;
			string? callingClass = callingFrame.GetMethod()
				?.DeclaringType?.Name;

			string message = formatter(
				state,
				exception);

			Dictionary<string, object>? additionalLogData = state as Dictionary<string, object>;

			string logMessage = _jsonSerializerHelper.ToJson(
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