// ------------------------------------------------------------------------------------------------
//  <copyright>
//    Copyright (c) CareSource, 2020-2022.  All rights reserved.
// 
//    FXIAuthentication
//    InteractiveLogger.cs
//  </copyright>
//  ------------------------------------------------------------------------------------------------

namespace FXIAuthentication.Core.Diagnostics;

using FXIAuthentication.Core.Diagnostics.Models;
using Console = System.Console;

public class InteractiveLogger : ILogger
{
	private readonly InteractiveLoggerConfiguration _config;
	private readonly string _name;

	public InteractiveLogger(
		string name,
		InteractiveLoggerConfiguration config)
	{
		_name = name;
		_config = config;
	}

	public IDisposable BeginScope<TState>(
		TState state) => null;

	public bool IsEnabled(LogLevel logLevel)
		=> _config.IsEnabled && (int)_config.LogLevel.Default <= (int)logLevel;

	public void Log<TState>(
		LogLevel logLevel,
		EventId eventId,
		TState state,
		Exception exception,
		Func<TState, Exception, string> formatter)
	{
		if ((_config.EventId == 0 ||
		     _config.EventId == eventId.Id) && IsEnabled(logLevel))
		{
			ConsoleColor color = Console.ForegroundColor;
			Console.ForegroundColor = _config?.LevelColors?
				                          .FirstOrDefault(lc => lc.LogLevel == logLevel)
				                          ?
				                          .Color ??
			                          ConsoleColor.White;

			if (logLevel == LogLevel.Error ||
			    logLevel == LogLevel.Critical ||
			    logLevel == LogLevel.Warning)
			{
				Console.WriteLine(
					$"{logLevel.ToString()} :: {_name}[{eventId.Id}]{Environment.NewLine}{formatter(state, exception)}");
			}
			else
			{
				Console.WriteLine($"{formatter(state, exception)}");
			}

			Console.ForegroundColor = color;
		}
	}
}