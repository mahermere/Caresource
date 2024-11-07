//  ------------------------------------------------------------------------------------------------
//  <copyright>
//    Copyright (c) CareSource, 2020-2022.  All rights reserved.
// 
//    WC.Services.Logging
//    ILoggingManager.cs
//  </copyright>
//  ------------------------------------------------------------------------------------------------

namespace WC.Services.Logging.Managers.v1
{
	using WC.Services.Logging.Models.v1;

	public interface ILoggingManager
	{
		ILoggingResponse Log(
			Message message,
			LogLevel level,
			string token);


		bool ValidateMessage(Message message,LogLevel level);
	}
}