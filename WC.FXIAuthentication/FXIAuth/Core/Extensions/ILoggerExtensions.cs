// ------------------------------------------------------------------------------------------------
//  <copyright>
//    Copyright (c) CareSource, 2020-2022.  All rights reserved.
// 
//    FXIAuthentication
//    ILoggerExtensions.cs
//  </copyright>
//  ------------------------------------------------------------------------------------------------

namespace FXIAuthentication.Core.Extensions;

public static class ILoggerExtensions
{
	public static void LogDebug(this ILogger logger,
		string message,
		IDictionary<string, object> additionalLogData) =>
		logger.Log(LogLevel.Debug, default, additionalLogData, null,
			(s,
				e) => message);

	public static void LogInformation(this ILogger logger,
		string message,
		IDictionary<string, object> additionalLogData) =>
		logger.Log(LogLevel.Information, default, additionalLogData, null,
			(s,
				e) => message);

	public static void LogWarning(this ILogger logger,
		string message,
		IDictionary<string, object> additionalLogData) =>
		logger.Log(LogLevel.Warning, default, additionalLogData, null,
			(s,
				e) => message);

	public static void LogError(this ILogger logger,
		Exception exception,
		string message,
		IDictionary<string, object> additionalLogData) =>
		logger.Log(LogLevel.Error, default, additionalLogData, exception,
			(s,
				e) => message);

	public static void LogCritical(this ILogger logger,
		Exception exception,
		string message,
		IDictionary<string, object> additionalLogData) =>
		logger.Log(LogLevel.Critical, default, additionalLogData, exception,
			(s,
				e) => message);

	public static void LogTrace(this ILogger logger,
		string message,
		IDictionary<string, object> additionalLogData) =>
		logger.Log(LogLevel.Trace, default, additionalLogData, null,
			(s,
				e) => message);
}