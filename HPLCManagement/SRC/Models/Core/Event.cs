// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2023. All rights reserved.
// 
//   WC.Services.HplcManagement
//   Event.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace HplcManagement.Models.Core
{
	using System;
	using Newtonsoft.Json;

	public class Event
	{
		[JsonProperty("message")]
		public string Message { get; set; } = String.Empty;

		[JsonProperty("logLevel")]
		public string LogLevel { get; set; } =
			Microsoft.Extensions.Logging.LogLevel.Information.ToString();

		[JsonProperty("route")]
		public string Route { get; set; } = "HPlc Management";
	}
}