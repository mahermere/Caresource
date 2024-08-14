namespace CareSource.WC.Core.Extensions
{
	using System;
	using System.Collections.Generic;
	using Microsoft.Extensions.Logging;

	public static class ILoggerExtensions
	{
		public static void LogDebug(this ILogger logger
			, string message, IDictionary<string, object> additionalLogData)
		{
			logger.Log(LogLevel.Debug, default(EventId), additionalLogData, null,
				(s, e) => message);
		}

		public static void LogInformation(this ILogger logger
			, string message, IDictionary<string, object> additionalLogData)
		{
			logger.Log(LogLevel.Information, default(EventId), additionalLogData, null,
				(s, e) => message);
		}

		public static void LogWarning(this ILogger logger
			, string message, IDictionary<string, object> additionalLogData)
		{
			logger.Log(LogLevel.Warning, default(EventId), additionalLogData, null,
				(s, e) => message);
		}

		public static void LogError(this ILogger logger
			, Exception exception, string message
			, IDictionary<string, object> additionalLogData)
		{
			logger.Log(LogLevel.Error, default(EventId), additionalLogData, exception,
				(s, e) => message);
		}

		public static void LogCritical(this ILogger logger
			, Exception exception, string message
			, IDictionary<string, object> additionalLogData)
		{
			logger.Log(LogLevel.Critical, default(EventId), additionalLogData, exception,
				(s, e) => message);
		}

		public static void LogTrace(this ILogger logger
			, string message, IDictionary<string, object> additionalLogData)
		{
			logger.Log(LogLevel.Trace, default(EventId), additionalLogData, null,
				(s, e) => message);
		}
	}
}
