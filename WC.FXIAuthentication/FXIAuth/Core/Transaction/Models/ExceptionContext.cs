// ------------------------------------------------------------------------------------------------
//  <copyright>
//    Copyright (c) CareSource, 2020-2022.  All rights reserved.
// 
//    FXIAuthentication
//    ExceptionContext.cs
//  </copyright>
//  ------------------------------------------------------------------------------------------------

namespace FXIAuthentication.Core.Transaction.Models;

public class ExceptionContext
{
	public string ActivityGuid { get; set; }

	public string AdditionalInfo { get; set; }

	public string Category { get; set; }

	public string Description { get; set; }

	public DateTime? EndedOn { get; set; }

	public string ErrorCode { get; set; }

	public string ErrorData { get; set; }
	public string ErrorGuid { get; set; }

	public string ErrorLevel { get; set; }

	public string Label { get; set; }

	public string MachineName { get; set; }

	public string Message { get; set; }

	public string Problemcode { get; set; }

	public string Remedy { get; set; }

	public string StackTrace { get; set; }

	public DateTime? StartedOn { get; set; }

	public Status? Status { get; set; }
}