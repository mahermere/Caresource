// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   Core
//   CommandSelector.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Core.Console
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using CareSource.WC.Core.Console.Interfaces;
	using CareSource.WC.Core.Console.Models;

	public class CommandSelector : ICommandSelector
	{
		private readonly IHelpCommand _helpCommand;

		public CommandSelector(
			IHelpCommand helpCommand) => _helpCommand = helpCommand;

		public ICommand SelectCommand(
			IList<ICommand> commands,
			IList<Argument> arguments)
		{
			Dictionary<string, ICommand> commandDictionary = commands.ToDictionary(
				c => c.CommandName.ToLower(),
				c => c);

			if (arguments.Any(a => a.ArgumentText == "--help" || a.ArgumentText == "-?"))
			{
				if (arguments.Any(a => a.ArgumentText == string.Empty))
				{
					arguments.Add(
						new Argument(
							"--command",
							arguments.First(a => a.ArgumentText == string.Empty)
								.Value));
				}

				foreach (Argument argument in arguments.Where(
					a => a.ArgumentText == "--help" || a.ArgumentText == "-?"))
				{
					argument.Processed = true;
				}

				return _helpCommand;
			}

			if (arguments.Any(a => a.ArgumentText == "--version"))
			{
				foreach (Argument argument in arguments.Where(a => a.ArgumentText == "--version"))
				{
					argument.Processed = true;
				}

				return commandDictionary["version"];
			}

			if (arguments.Any(a => a.ArgumentText == string.Empty))
			{
				string commandName = arguments.First(a => a.ArgumentText == string.Empty)
					.Value.ToString()
					.ToLower();

				if (!commandDictionary.ContainsKey(commandName))
				{
					throw new ArgumentException(
						$"Unknown command: {commandName}.",
						commandName);
				}

				return commandDictionary[commandName];
			}

			if (commandDictionary.ContainsKey(string.Empty))
			{
				return commandDictionary[string.Empty];
			}

			return _helpCommand;
		}
	}
}