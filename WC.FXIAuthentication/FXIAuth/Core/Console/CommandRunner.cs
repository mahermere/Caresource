// ------------------------------------------------------------------------------------------------
//  <copyright>
//    Copyright (c) CareSource, 2020-2022.  All rights reserved.
// 
//    FXIAuthentication
//    CommandRunner.cs
//  </copyright>
//  ------------------------------------------------------------------------------------------------

namespace FXIAuthentication.Core.Console;

using System.Reflection;
using FXIAuthentication.Core.Console.Models;
using FXIAuthentication.Core.Extensions;
using FXIAuthentication.Core.Transaction;
using FXIAuthentication.Core.Transaction.Models;

public class CommandRunner : ICommandRunner
{
	private readonly IArgumentParser _argumentParser;
	private readonly IList<ICommand> _commands;
	private readonly ICommandSelector _commandSelector;
	private readonly ILogger _logger;
	private readonly ITransactionContextManager _transactionContextManager;

	public CommandRunner(
		IArgumentParser argumentParser,
		ICommandSelector commandSelector,
		IEnumerable<ICommand> commands,
		ITransactionContextManager transactionContextManager,
		ILogger<CommandRunner> logger)
	{
		_argumentParser = argumentParser;
		_commandSelector = commandSelector;
		_commands = commands.ToList();
		_transactionContextManager = transactionContextManager;
		_logger = logger;
	}

	public int RunCommands(
		string[] args)
	{
		int exitCode = 0;
		BaseCommandModel commandModel = null;

		try
		{
			IList<Argument> arguments = _argumentParser.ParseArguments(args);

			string triggeredCommand = arguments.FirstOrDefault()
				?.Value as string;

			_logger.LogDebug($"Founding matching '{triggeredCommand}' command...");

			ICommand command = _commandSelector.SelectCommand(
				_commands,
				arguments);

			if (command == null)
			{
				throw new ArgumentException($"Command '{triggeredCommand}' is not supported.");
			}

			_logger.LogDebug("Building Command Model...");
			commandModel = GetCommandModel(
				command,
				arguments);

			_transactionContextManager.InitializeContext(new TransactionContext
			{
				EventContext = new EventContext { Event = command.CommandName }
			});

			_logger.LogDebug(
				$"Running command '{command.GetType().Name}'...",
				commandModel?.CorrelationGuid);
			object result = command.GetType()
				.GetMethod("Run")
				?.Invoke(
					command,
					new[] { commandModel });

			exitCode = result?.ToString()
				           .ToSafeInt() ??
			           -1;
		}
		catch (ArgumentException ex)
		{
			string errorMsg = ex.InnerException?.Message ?? ex.Message;

			_logger.LogError(
				$"{errorMsg}{Environment.NewLine}" +
				$"{ex.StackTrace}");
			exitCode = -1;
		}
		catch (Exception ex)
		{
			string errorMsg = ex.InnerException?.Message ?? ex.Message;

			_logger.LogCritical(
				$"{errorMsg}{Environment.NewLine}" +
				$"{ex.StackTrace}");
			exitCode = -1;
		}

		return exitCode;
	}

	private BaseCommandModel GetCommandModel(
		ICommand command,
		IList<Argument> arguments)
	{
		Type commandType = command.GetType()
			.GetInterfaces()
			.FirstOrDefault(t => t.GUID == typeof(ICommand<>).GUID);

		if (commandType == null)
		{
			throw new ArgumentException(
				"Command must implement ICommand<TCommandModel>.",
				nameof(command));
		}

		Type commandModelType = commandType.GenericTypeArguments?.FirstOrDefault();

		if (commandModelType == null)
		{
			throw new ArgumentException(
				"Could not find command model type.",
				nameof(command));
		}

		object commandModel = Activator.CreateInstance(commandModelType);

		var properties = commandModelType.GetProperties()
			.Where(p => null != p.GetCustomAttribute<CommandArgumentAttribute>())
			.Select(
				p => new
				{
					Property = p,
					CommandArgumentAttribute = p.GetCustomAttribute<CommandArgumentAttribute>()
				});

		foreach (var property in properties)
		{
			string argumentName = property.CommandArgumentAttribute.ArgumentName?.ToLower();
			string argumentAbbreviation =
				property.CommandArgumentAttribute.ArgumentAbbreviation?.ToLower();

			if (!string.IsNullOrEmpty(argumentName) &&
			    arguments.Any(a => a.ArgumentText == $"--{argumentName}"))
			{
				SetCommandModelProperty(
					arguments,
					property.Property,
					commandModel,
					$"--{argumentName}");
				continue;
			}

			if (!string.IsNullOrEmpty(argumentAbbreviation) &&
			    arguments.Any(a => a.ArgumentText == $"-{argumentAbbreviation}"))
			{
				SetCommandModelProperty(
					arguments,
					property.Property,
					commandModel,
					$"-{argumentAbbreviation}");
				continue;
			}

			if (property.CommandArgumentAttribute.Required)
			{
				throw new ArgumentException(
					$"Missing Required Option: --{property.CommandArgumentAttribute.ArgumentName}.");
			}
		}

		return commandModel as BaseCommandModel;
	}

	private void SetCommandModelProperty(
		IList<Argument> arguments,
		PropertyInfo property,
		object commandModel,
		string argument)
	{
		object foundValue = arguments.First(a => a.ArgumentText == argument)
			.Value;
		object convertedValue = Convert.ChangeType(
			foundValue,
			property.PropertyType);

		property.SetValue(
			commandModel,
			convertedValue);
	}
}