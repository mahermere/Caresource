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
	using System.Collections.Generic;
	using Microsoft.Extensions.Logging;

	public class InteractiveLoggerConfiguration
	{
		public int EventId { get; set; } = 0;

		public LogLevelConfiguration LogLevel { get; set; }

		public bool IsEnabled { get; set; } = true;

		public List<LevelColor> LevelColors { get; set; }
	}
}