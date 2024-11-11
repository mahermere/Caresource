// ------------------------------------------------------------------------------------------------
//  <copyright>
//    Copyright (c) CareSource, 2020-2022.  All rights reserved.
// 
//    FXIAuthentication
//    TransactionContext.cs
//  </copyright>
//  ------------------------------------------------------------------------------------------------

namespace FXIAuthentication.Core.Transaction.Models;

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