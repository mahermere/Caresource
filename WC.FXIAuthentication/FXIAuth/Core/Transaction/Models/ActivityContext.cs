// ------------------------------------------------------------------------------------------------
//  <copyright>
//    Copyright (c) CareSource, 2020-2022.  All rights reserved.
// 
//    FXIAuthentication
//    ActivityContext.cs
//  </copyright>
//  ------------------------------------------------------------------------------------------------

namespace FXIAuthentication.Core.Transaction.Models;

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