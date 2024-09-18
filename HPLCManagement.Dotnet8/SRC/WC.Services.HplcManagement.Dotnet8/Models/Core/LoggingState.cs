// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2023. All rights reserved.
// 
//   WC.Services.HplcManagement
//   LoggingState.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.HplcManagement.Dotnet8.Models.Core
{
	using System;
	using System.Collections.Generic;

	public sealed class LoggingState : IDisposable
	{
		public LoggingState(IDictionary<string, string> state)
			=> State = state;

		public string CorrelationGuid
			=> State.ContainsKey(GlobalConstants.CorrelationGuid)
				? State[GlobalConstants.CorrelationGuid]
				: Guid.NewGuid().ToString();

		public string Service
			=> State.ContainsKey(GlobalConstants.Service)
				? State[GlobalConstants.Service]
				: "Unknown";

		public string Route
			=> State.ContainsKey(GlobalConstants.Route)
				? State[GlobalConstants.Route]
				: "Unknown";

		public IDictionary<string, string> State { get; internal set; }

		public void Dispose()
			=> State = null;
	}
}