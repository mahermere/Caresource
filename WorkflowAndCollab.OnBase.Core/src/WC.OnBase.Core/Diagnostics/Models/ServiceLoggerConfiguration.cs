// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2022. All rights reserved.
// 
//   Core
//   ServiceLoggerConfiguration.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.OnBase.Core.Diagnostics.Models
{
	public class ServiceLoggerConfiguration : BatchLoggerConfiguration
	{
		/// <summary>
		/// Gets or sets the service logger configuration URL
		/// </summary>
		public string Url { get; set; }

		/// <summary>
		/// Gets or sets the service logger configuration unique identifier
		/// </summary>
		public string Token { get; set; }
	}
}