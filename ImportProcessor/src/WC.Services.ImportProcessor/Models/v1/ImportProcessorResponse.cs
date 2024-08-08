// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2021. All rights reserved.
// 
//   ImportProcessor
//   ImportProcessorResponse.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace ImportProcessor.Models.v1
{
	using System;
	using CareSource.WC.Entities.Exceptions;
	using CareSource.WC.Entities.Responses;

	public class ImportProcessorResponse
	{
		public ImportProcessorResponse()
		{ }

		public ImportProcessorResponse(
			ResponseStatus status,
			ErrorCode errorCode,
			string message,
			Guid correlationGuid)
		{
			this.status = status;
			this.errorCode = errorCode;
			this.message = message;
			this.correlationGuid = correlationGuid;
		}

		public ResponseStatus status { get; set; }
		public ErrorCode errorCode { get; set; }
		public string message { get; set; }
		public Guid correlationGuid { get; set; }
	}
}