// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2022. All rights reserved.
// 
//   Core
//   BatchLoggerConfiguration.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.OnBase.Core.Diagnostics.Models
{
	public abstract class BatchLoggerConfiguration
	{
		public int EventId { get; set; } = 0;

		public double? FlushPeriod { get; set; } = 10;

		public int? BackgroundQueueSize { get; set; } = 5000;

		public int? BatchSize { get; set; } = 5000;

		public bool IsEnabled { get; set; } = true;

		public LogLevelConfiguration LogLevel { get; set; }
	}
}