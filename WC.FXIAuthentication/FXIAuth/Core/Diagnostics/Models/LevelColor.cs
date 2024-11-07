// ------------------------------------------------------------------------------------------------
//  <copyright>
//    Copyright (c) CareSource, 2020-2022.  All rights reserved.
// 
//    FXIAuthentication
//    LevelColor.cs
//  </copyright>
//  ------------------------------------------------------------------------------------------------

namespace FXIAuthentication.Core.Diagnostics.Models;

public class LevelColor
{
	public LogLevel LogLevel { get; set; }

	public ConsoleColor Color { get; set; }
}