// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   Entities
//   ActivityContext.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Entities.Transactions
{
	using System;

	public class ActivityContext
	{
		public string ActivityGuid { get; set; }

		public string ActivityType { get; set; }

		public DateTime? EndedOn { get; set; }

		public string Process { get; set; }
		public string ProcessId { get; set; }

		public int? RetryCount { get; set; }

		public DateTime? StartedOn { get; set; }

		public Status? Status { get; set; }
	}
}