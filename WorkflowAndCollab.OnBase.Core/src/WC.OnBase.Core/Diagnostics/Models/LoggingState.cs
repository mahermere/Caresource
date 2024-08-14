//  ------------------------------------------------------------------------------------------------
//  <copyright>
//    Copyright (c) CareSource, 2020-2022.  All rights reserved.
// 
//    OnBase.Core
//    LoggingState.cs
//  </copyright>
//  ------------------------------------------------------------------------------------------------

namespace CareSource.WC.OnBase.Core.Diagnostics.Models
{
	using System;
	using System.Collections.Generic;

	public sealed class LoggingState : IDisposable
	{
		public string CorrelationGuid => State["Correlation Guid"];

		public string Service => State["Service"];

		public LoggingState(IDictionary<string, string> state)
		{
			State = state;
		}

		public IDictionary<string, string> State { get; internal set; }

		public void Dispose()
		{
			State = null;
		}
	}
}