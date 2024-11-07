//  ------------------------------------------------------------------------------------------------
//  <copyright>
//    Copyright (c) CareSource, 2020-2022.  All rights reserved.
// 
//    WC.Services.Logging
//    LoggingManager.cs
//  </copyright>
//  ------------------------------------------------------------------------------------------------

namespace WC.Services.Logging.Managers.v1
{
	using WC.Services.Logging.Adapters.v1;
	using WC.Services.Logging.Models.v1;

	public class LoggingManager : ILoggingManager
	{
		private readonly ILoggingAdapter _adapter;

		public LoggingManager(ILoggingAdapter adapter) => _adapter = adapter;


		public ILoggingResponse Log(
			Message message,
			LogLevel level,
			string token = "")
		{
			ValidateMessage
				(message,
					level);

			ILoggingResponse resp = _adapter.WriteLog(
				message,
				token);

			CompleteResult(
				message,
				resp);
			return resp;
		}

		public bool ValidateMessage(Message message, LogLevel level)
		{
			if (message.Fields.TryGetValue(
					Models.Constants.CorrelationGuid,
					out string correlationGuid))
			{
				if (string.IsNullOrWhiteSpace(correlationGuid))
				{
					message.Fields[Models.Constants.CorrelationGuid] = Guid.NewGuid().ToString();
				}
			}
			else
			{
				message.Fields.Add(Models.Constants.CorrelationGuid, Guid.NewGuid().ToString());
			}

			message.Event.LogLevel = level;

			return true;
		}

		private ILoggingResponse CompleteResult(
			Message message,
			ILoggingResponse result)
		{
			result.CorrelationGuid = message.Fields[Models.Constants.CorrelationGuid];

			if (result.Code == -1)
			{
				result.Message =
					"Log was accepted and will get moved to " +
					$"splunk:[{message.Fields[Models.Constants.CorrelationGuid]}].";
			}

			return result;
		}
	}
}