// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2022. All rights reserved.
// 
//   Core
//   ILoggerExtensions.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.OnBase.Core.ExtensionMethods
{
	using System;
	using System.Collections.Generic;
	using System.Net.Http;
	using System.Runtime.CompilerServices;
	using System.Web.Http;
	using Microsoft.Extensions.Logging;

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
			[CallerMemberName] string memberName = "",
			[CallerFilePath] string sourceFilePath = "",
			[CallerLineNumber] int sourceLineNumber = 0)
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
			[CallerMemberName] string memberName = "",
			[CallerFilePath] string sourceFilePath = "",
			[CallerLineNumber] int sourceLineNumber = 0)
			=> Log(logger,
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
			[CallerMemberName] string memberName = "",
			[CallerFilePath] string sourceFilePath = "",
			[CallerLineNumber] int sourceLineNumber = 0)
			=> Log(logger,
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
			[CallerMemberName] string memberName = "",
			[CallerFilePath] string sourceFilePath = "",
			[CallerLineNumber] int sourceLineNumber = 0)
			=> Log(logger,
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
				default,
				additionalLogData,
				exception,
				(
					s,
					e) => message);
		}

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
					"URI", httpRequest.RequestUri.AbsoluteUri);

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
}