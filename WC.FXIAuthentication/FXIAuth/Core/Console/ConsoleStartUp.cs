// ------------------------------------------------------------------------------------------------
//  <copyright>
//    Copyright (c) CareSource, 2020-2022.  All rights reserved.
// 
//    FXIAuthentication
//    ConsoleStartUp.cs
//  </copyright>
//  ------------------------------------------------------------------------------------------------

namespace FXIAuthentication.Core.Console;

using FXIAuthentication.Core.Configuration.Models;
using FXIAuthentication.Core.Console.Commands;
using FXIAuthentication.Core.Extensions;
using FXIAuthentication.Core.Helpers;
using FXIAuthentication.Core.Transaction;
using Console = System.Console;

public static class ConsoleStartUp
{
	public static int Run<TLaunchingType>(
		string[] args)
	{
		ServiceProvider serviceProvider = Configure();

		ITransactionContextManager transactionContextManager =
			serviceProvider.GetService<ITransactionContextManager>();
		ILogger<TLaunchingType> logger = serviceProvider.GetService<ILogger<TLaunchingType>>();
		ICareSourceEnvironmentHelper careSourceEnvironmentHelper =
			serviceProvider.GetService<ICareSourceEnvironmentHelper>();
		ICommandRunner commandRunner = serviceProvider.GetService<ICommandRunner>();

		if (commandRunner == null ||
		    logger == null ||
		    careSourceEnvironmentHelper == null)
		{
			logger.LogError(
				"FATAL ERROR: Could not successfully instantiate console application dependencies...");
			return -1;
		}

		int result = commandRunner.RunCommands(args);

		if (careSourceEnvironmentHelper.GetCareSourceEnvironment() == Environment.DevelopLocal)
		{
			logger.LogInformation("\r\nPress any Key to Exit.");
			Console.ReadKey();
		}

		transactionContextManager.FinalizeContext(transactionContextManager.CurrentContext);

		return result;
	}

	private static ServiceProvider Configure()
	{
		IConfiguration configuration = new ConfigurationBuilder()
			.AddJsonFile(
				"appsettings.json",
				false,
				true)
			.Build();

		ServiceProvider serviceProvider = new ServiceCollection()
			.AddSingleton(configuration)
			.AddSingleton<IArgumentParser, ArgumentParser>()
			.AddSingleton<ICommandSelector, CommandSelector>()
			.AddSingleton<ICommandRunner, CommandRunner>()
			.AddSingleton<IHelpCommand, HelpCommand>()
			.AddSingleton<ICommand, VersionCommand>()
			.AddCareSourceServices(configuration)
			.AddCareSourceLogging(configuration)
			.BuildServiceProvider();

		return serviceProvider;
	}
}