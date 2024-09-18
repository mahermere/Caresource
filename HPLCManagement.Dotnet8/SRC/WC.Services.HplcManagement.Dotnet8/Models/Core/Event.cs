// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2023. All rights reserved.
// 
//   WC.Services.HplcManagement
//   Event.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.HplcManagement.Dotnet8.Models.Core
{
	using System;
	using System.Text.Json.Serialization;

	public class Event
	{
		[JsonPropertyName("message")]
		public string Message { get; set; } = String.Empty;

		[JsonPropertyName("logLevel")]
		public string LogLevel { get; set; } =
			Microsoft.Extensions.Logging.LogLevel.Information.ToString();

		[JsonPropertyName("route")]
		public string Route { get; set; } = "HPlc Management";
	}
}