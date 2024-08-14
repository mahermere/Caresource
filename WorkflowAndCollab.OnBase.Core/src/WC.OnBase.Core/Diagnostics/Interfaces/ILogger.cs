//  ------------------------------------------------------------------------------------------------
//  <copyright>
//    Copyright (c) CareSource, 2020-2022.  All rights reserved.
// 
//    OnBase.Core
//    ILogger.cs
//  </copyright>
//  ------------------------------------------------------------------------------------------------

namespace CareSource.WC.OnBase.Core.Diagnostics.Interfaces
{
	using System;
	using System.Collections.Generic;

	public interface ILogger
	{
		void LogDebug(string message,
			IDictionary<string, object> additionalLogData = null);

		void LogError(string message,
			Exception exception = null,
			IDictionary<string, object> additionalLogData = null);

		void LogInfo(string message,
			IDictionary<string, object> additionalLogData = null);

		void LogMessage(string message,
			LogLevels logLevel,
			IDictionary<string, object> additionalLogData = null);

		void LogWarning(string message,
			IDictionary<string, object> additionalLogData = null);
	}
}