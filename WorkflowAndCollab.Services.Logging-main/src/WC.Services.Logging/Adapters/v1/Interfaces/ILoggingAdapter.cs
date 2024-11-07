//  ------------------------------------------------------------------------------------------------
//  <copyright>
//    Copyright (c) CareSource, 2020-2022.  All rights reserved.
// 
//    WC.Services.Logging
//    ILoggingAdapter.cs
//  </copyright>
//  ------------------------------------------------------------------------------------------------

namespace WC.Services.Logging.Adapters.v1
{
	using WC.Services.Logging.Models.v1;

	public interface ILoggingAdapter
	{
		ILoggingResponse WriteLog(Message message, string token);

		bool IsEnabled(LogLevel logLevel);
	}
}