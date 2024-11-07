//  ------------------------------------------------------------------------------------------------
//  <copyright>
//    Copyright (c) CareSource, 2020-2022.  All rights reserved.
// 
//    WC.Services.Logging
//    ILoggingResponse.cs
//  </copyright>
//  ------------------------------------------------------------------------------------------------

namespace WC.Services.Logging.Models.v1
{
	public interface ILoggingResponse
	{
		string Text { get; set; }

		int Code { get; set; }

		string CorrelationGuid { get; set; }

		string Message { get; set; }
	}
}