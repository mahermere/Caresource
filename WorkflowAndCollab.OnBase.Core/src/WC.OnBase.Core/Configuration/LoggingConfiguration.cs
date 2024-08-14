// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   CareSource.WC.OnBase.Core
//   LoggingConfiguration.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.OnBase.Core.Configuration
{
	using System;
	using System.Diagnostics;

	public class LoggingConfiguration
	{
		public TraceLevel Level { get; set; }
		public string Path { get; set; }

		public static implicit operator string(
			LoggingConfiguration config) => throw new NotImplementedException();
	}
}