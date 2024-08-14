// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   Core
//   InteractiveLoggerConfiguration.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Core.Diagnostics.Models
{
	public class FileLoggerConfiguration : BatchLoggerConfiguration
	{
		public string LoggingDirectory { get; set; }

		public string LoggingFileName { get; set; }

		public string DatePattern { get; set; } = "yyyy-MM-dd";

		public int? FileSizeLimit { get; set; } = 10 * 1024 * 1024;

		public int? RetainedFileCountLimit { get; set; } = 2;
	}
}