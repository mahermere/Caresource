using System;
using System.Collections.Generic;
using System.Diagnostics;
using CareSource.WC.OnBase.Core.Diagnostics.Interfaces;
using CareSource.WC.OnBase.Core.Transaction.Interfaces;
using log4net;

namespace CareSource.WC.OnBase.Core.Diagnostics
{
	using CareSource.WC.OnBase.Core.Helpers.Interfaces;

	public class Log4NetLogger : ILogger
    {
        private readonly ILog _logger;
        private readonly ITransactionContextManager _transactionContextManager;
        private readonly IJsonSerializerHelper _jsonSerializerHelper;

        public Log4NetLogger(ITransactionContextManager transactionContextManager
            , IJsonSerializerHelper jsonSerializerHelper)
        {
            _logger = LogManager.GetLogger(typeof(Log4NetLogger));
            _transactionContextManager = transactionContextManager;
            _jsonSerializerHelper = jsonSerializerHelper;
        }

        public void LogDebug(string message, IDictionary<string, object> additionalLogData = null)
        {
            WriteLogLineToFile(message, new StackFrame(1), LogLevels.Debug, DateTime.Now, additionalLogData);
        }

        public void LogInfo(string message, IDictionary<string, object> additionalLogData = null)
        {
            WriteLogLineToFile(message, new StackFrame(1), LogLevels.Information, DateTime.Now, additionalLogData);
        }

        public void LogWarning(string message, IDictionary<string, object> additionalLogData = null)
        {
            WriteLogLineToFile(message, new StackFrame(1), LogLevels.Warning, DateTime.Now, additionalLogData);
        }

        public void LogError(string message, Exception exception = null, IDictionary<string, object> additionalLogData = null)
        {
            WriteLogLineToFile(message, new StackFrame(1), LogLevels.Error, DateTime.Now, additionalLogData, exception);
        }

        public void LogMessage(string message, LogLevels logLevel, IDictionary<string, object> additionalLogData = null)
        {
            WriteLogLineToFile(message, new StackFrame(1), logLevel, DateTime.Now, additionalLogData);
        }

        private void WriteLogLineToFile(string message, StackFrame callingFrame, LogLevels loglevel
            , DateTime logDate, IDictionary<string, object> additionalLogData, Exception exception = null)
        {
            var loggingContext = _transactionContextManager.CopyCurrentContext();

            if (loggingContext?.SecurityContext?.Password != null)
                loggingContext.SecurityContext.Password = null;

            var callingMethod = callingFrame?.GetMethod()?.Name;
            var callingClass = callingFrame?.GetMethod()?.DeclaringType?.Name;

            var logMessage = _jsonSerializerHelper.ToJson(new
            {
                Message = message,
                Method = callingMethod,
                Class = callingClass,
                LogLevel = Enum.GetName(typeof(LogLevels), loglevel),
                LogDate = logDate,
                Exception = exception,
                AdditionalLogData = additionalLogData,
                TransactionContext = loggingContext
            });

            switch (loglevel)
            {
                case LogLevels.Error:
                    _logger.Error(logMessage);
                    break;
                case LogLevels.Information:
                    _logger.Info(logMessage);
                    break;
                case LogLevels.Warning:
                    _logger.Warn(logMessage);
                    break;
                default:
                    _logger.Debug(logMessage);
                    break;
            }
        }
    }
}
