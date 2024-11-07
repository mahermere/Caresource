// ------------------------------------------------------------------------------------------------
//  <copyright>
//    Copyright (c) CareSource, 2020-2022.  All rights reserved.
// 
//    FXIAuthentication
//    ExecutionContext.cs
//  </copyright>
//  ------------------------------------------------------------------------------------------------

namespace FXIAuthentication.Core.Transaction.Models;

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