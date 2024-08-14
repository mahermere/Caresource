// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   Core
//   InteractiveLoggerProvider.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Core.Diagnostics
{
	using System.Collections.Concurrent;
	using CareSource.WC.Core.Diagnostics.Models;
	using Microsoft.Extensions.Logging;

	public class InteractiveLoggerProvider : ILoggerProvider
	{
		private InteractiveLoggerConfiguration _config;

		private readonly ConcurrentDictionary<string, InteractiveLogger> _loggers =
			new ConcurrentDictionary<string, InteractiveLogger>();

		public InteractiveLoggerProvider(InteractiveLoggerConfiguration config)
		{
			_config = config;
		}

		public ILogger CreateLogger(string categoryName)
			=> _loggers.GetOrAdd(categoryName, name =>
				new InteractiveLogger(name, _config));

		public void Dispose()
		{
			_loggers.Clear();
		}
	}
}