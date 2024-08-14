// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   Entities
//   TransactionContext.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Entities.Transactions
{
	public class TransactionContext
	{
		public BusinessContext BusinessContext { get; set; }
		public EventContext EventContext { get; set; }

		public string EventData { get; set; }

		public ExecutionContext ExecutionContext { get; set; }

		public ProcessContext ProcessContext { get; set; }

		public SecurityContext SecurityContext { get; set; }

		public Status? Status { get; set; }
	}
}