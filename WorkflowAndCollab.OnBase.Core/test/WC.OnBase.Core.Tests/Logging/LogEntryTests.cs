namespace WorkFlowAndCollab.Tools.Tests.Logging
{
	using System;
	using System.Diagnostics;
	using Microsoft.VisualStudio.TestTools.UnitTesting;
	using Shouldly;
	using WorkFlowAndCollab.Entities.Providers;
	using WorkFlowAndCollab.Tools.Configuration;
	using WorkFlowAndCollab.Tools.Logging;

	[TestClass()]
	public class LogEntryTests
	{
		[TestMethod()]
		public void LogExceptionEntryTest()
		{
			using (TraceLogger logger = TraceLogger.CreateFromCaller(
				new LoggingConfiguration
				{
					Level = TraceLevel.Verbose,
					Path = "c:\\logs\\LoggerTests"
				},
				Guid.NewGuid()))
			{
				logger.LogError(new Exception("Hello"));
			}

		}
	}
}