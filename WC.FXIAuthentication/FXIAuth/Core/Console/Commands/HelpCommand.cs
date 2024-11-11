// ------------------------------------------------------------------------------------------------
//  <copyright>
//    Copyright (c) CareSource, 2020-2022.  All rights reserved.
// 
//    FXIAuthentication
//    HelpCommand.cs
//  </copyright>
//  ------------------------------------------------------------------------------------------------

namespace FXIAuthentication.Core.Console.Commands;

using System.Reflection;
using FXIAuthentication.Core.Console.Models;
using Microsoft.Extensions.PlatformAbstractions;

public class HelpCommand : IHelpCommand
{
	private readonly string _applicationName;
	private readonly IList<ICommand> _commands;
	private readonly string _executableName;
	private readonly ILogger _logger;

	public HelpCommand(
		ILogger<HelpCommand> logger,
		IEnumerable<ICommand> commands)
	{
		_logger = logger;

		_commands = commands.ToList();
		_commands.Add(this);
		_commands = _commands.OrderBy(c => c.CommandName)
			.ToList();

		_applicationName = PlatformServices.Default.Application.ApplicationName;
		_executableName = _applicationName.ToLower();
	}

	public string CommandName => "help";

	public string CommandDescription
		=> "Writes Help Documentation for the Application or a Provided Command.";

	public int Run(
		HelpCommandModel commandModel)
	{
		if (!string.IsNullOrEmpty(commandModel?.CommandName))
		{
			ICommand command = _commands.FirstOrDefault(
				c => c.CommandName.ToLower() == commandModel.CommandName.ToLower());

			if (null == command)
			{
				throw new ArgumentException(
					$"Unknown command: {commandModel.CommandName}.",
					commandModel.CommandName);
			}

			LogCommandDetails(command);
		}
		else
		{
			ICommand defaultCommand = _commands.FirstOrDefault(c => c.CommandName == string.Empty);
			if (null != defaultCommand)
			{
				LogCommandDetails(defaultCommand);
			}

			LogApplicationCommands();
		}

		return 0;
	}

	internal void LogCommandDetails(
		ICommand command)
	{
		string FormatArgumentName(
			CommandArgumentAttribute a)
		{
			return
				$"{(string.IsNullOrEmpty(a.ArgumentAbbreviation) ? string.Empty : "-")}{a.ArgumentAbbreviation}" +
				$"{(string.IsNullOrEmpty(a.ArgumentAbbreviation) ? string.Empty : "|")}--{a.ArgumentName}" +
				$"{(a.Required ? " [Required]" : string.Empty)}";
		}

		Type iCommandType = command.GetType()
			.GetInterfaces()
			.FirstOrDefault(t => t.GUID == typeof(ICommand<>).GUID);

		if (null == iCommandType)
		{
			throw new ArgumentException(
				"Command must implement ICommand<TCommandModel>.",
				nameof(command));
		}

		Type commandModelType = iCommandType.GenericTypeArguments.FirstOrDefault();

		IEnumerable<(PropertyInfo Property, CommandArgumentAttribute CommandArgumentAttribute)>
			commandModelProperties = commandModelType.GetProperties()
				.Where(p => null != p.GetCustomAttribute<CommandArgumentAttribute>())
				.Select(
					p => (Property: p,
						CommandArgumentAttribute: p.GetCustomAttribute<CommandArgumentAttribute>()));

		_logger.LogInformation($"{command.CommandDescription}{Environment.NewLine}");

		_logger.LogInformation(
			$"Usage: {_executableName} {(string.IsNullOrEmpty(command.CommandName) ? string.Empty : command.CommandName + " ")}" +
			$"{(!commandModelProperties.Any() ? string.Empty : "[options] ")}[common-options]{Environment.NewLine}");

		int maxCommandLength = commandModelProperties.Aggregate(
			0,
			(
				max,
				cur) =>
			{
				string formattedArgumentName = FormatArgumentName(cur.CommandArgumentAttribute);

				return max > formattedArgumentName.Length
					? max
					: formattedArgumentName.Length;
			});

		if (commandModelProperties.Any())
		{
			_logger.LogInformation("Options:");

			foreach ((PropertyInfo Property, CommandArgumentAttribute CommandArgumentAttribute)
			         commandModelProperty in commandModelProperties)
			{
				string formattedArgumentName =
					FormatArgumentName(commandModelProperty.CommandArgumentAttribute);

				_logger.LogInformation(
					$"   {formattedArgumentName.PadRight(maxCommandLength)}      " +
					$"{commandModelProperty.CommandArgumentAttribute.Description}");
			}

			_logger.LogInformation(string.Empty);
		}
	}

	internal void LogApplicationCommands()
	{
		_logger.LogInformation(
			$"Usage: {_executableName} [command] [options] [common-options]{Environment.NewLine}");
		_logger.LogInformation($"{_applicationName} Commands:");

		int maxCommandLength = _commands.Aggregate(
			0,
			(
				max,
				cur) =>
			{
				return max > cur.CommandName.Length
					? max
					: cur.CommandName.Length;
			});

		foreach (ICommand command in _commands)
		{
			if (string.IsNullOrEmpty(command.CommandName))
			{
				continue;
			}

			_logger.LogInformation(
				$"   {command.CommandName.PadRight(maxCommandLength)}      {command.CommandDescription}");
		}

		_logger.LogInformation(
			$"{Environment.NewLine}Run '{_executableName} [command] --help' for more information on a command.{Environment.NewLine}");
	}
}