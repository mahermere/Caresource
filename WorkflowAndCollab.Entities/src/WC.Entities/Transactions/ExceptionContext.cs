// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   Entities
//   ExceptionContext.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Entities.Transactions
{
	using System;

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
}