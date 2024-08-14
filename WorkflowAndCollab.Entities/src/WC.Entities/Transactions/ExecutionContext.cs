// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   Entities
//   ExecutionContext.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Entities.Transactions
{
	using System;

	public class ExecutionContext
	{
		public DateTime? EndedOn { get; set; }

		public string Environment { get; set; }

		public LogLevel? LogLevel { get; set; }
		public string Process { get; set; }

		public string PurgingSchedule { get; set; }

		public int? RetryCount { get; set; }

		public string Server { get; set; }

		public DateTime? StartedOn { get; set; }

		public string Synthetic { get; set; }
	}
}