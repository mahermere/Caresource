// ------------------------------------------------------------------------------------------------
//  <copyright>
//    Copyright (c) CareSource, 2020-2022.  All rights reserved.
// 
//    FXIAuthentication
//    InteractiveLoggerProvider.cs
//  </copyright>
//  ------------------------------------------------------------------------------------------------

namespace FXIAuthentication.Core.Diagnostics;

using System.Collections.Concurrent;
using FXIAuthentication.Core.Diagnostics.Models;

public class InteractiveLoggerProvider : ILoggerProvider
{
	private readonly InteractiveLoggerConfiguration _config;
	private readonly ConcurrentDictionary<string, InteractiveLogger> _loggers = new();

	public InteractiveLoggerProvider(InteractiveLoggerConfiguration config) => _config = config;

	public ILogger CreateLogger(string categoryName)
		=> _loggers.GetOrAdd(categoryName, name =>
			new InteractiveLogger(name, _config));

	public void Dispose() => _loggers.Clear();
}