// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   Core
//   InteractiveLoggerConfiguration.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Core.Diagnostics.Models
{
	using System;
	using Microsoft.Extensions.Logging;

	public class LevelColor
	{
		public LogLevel LogLevel { get; set; }

		public ConsoleColor Color { get; set; }
	}
}