// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2022. All rights reserved.
// 
//   Core
//   LogLevelConfiguration.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.OnBase.Core.Diagnostics.Models
{
	using Microsoft.Extensions.Logging;

	public class LogLevelConfiguration
	{
		public LogLevel Default { get; set; } = LogLevel.Debug;
	}
}