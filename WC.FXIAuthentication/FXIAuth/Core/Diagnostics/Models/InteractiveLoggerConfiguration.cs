// ------------------------------------------------------------------------------------------------
//  <copyright>
//    Copyright (c) CareSource, 2020-2022.  All rights reserved.
// 
//    FXIAuthentication
//    InteractiveLoggerConfiguration.cs
//  </copyright>
//  ------------------------------------------------------------------------------------------------

namespace FXIAuthentication.Core.Diagnostics.Models;

public class InteractiveLoggerConfiguration
{
	public int EventId { get; set; } = 0;

	public LogLevelConfiguration LogLevel { get; set; }

	public bool IsEnabled { get; set; } = true;

	public List<LevelColor> LevelColors { get; set; }
}