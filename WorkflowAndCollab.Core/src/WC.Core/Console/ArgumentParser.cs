// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   Core
//   ArgumentParser.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Core.Console
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using CareSource.WC.Core.Console.Interfaces;
	using CareSource.WC.Core.Console.Models;
	using Microsoft.Extensions.Logging;

	public class ArgumentParser : IArgumentParser
	{
		private readonly ILogger<ArgumentParser> _logger;

		internal string CurrentArgumentName;
		internal string CurrentArgumentValue;
		internal char? CurrentArgumentValueQuoteType;

		public ArgumentParser(
			ILogger<ArgumentParser> logger) => _logger = logger;

		public IList<Argument> ParseArguments(
			string[] args)
		{
			_logger.LogDebug($"Parsing arguments: [{string.Join(", ", args)}]");

			if (null == args ||
			    !args.Any())
			{
				_logger.LogDebug("No arguments found...");

				return new List<Argument>();
			}

			List<Argument> parsedArguments = new List<Argument>();

			ParseCommandName(
				args[0],
				parsedArguments);

			for (int i = 1; i < args.Length; i++)
			{
				ParseArgumentText(
					args[i],
					parsedArguments);
			}

			ParseFinalArgument(parsedArguments);

			_logger.LogDebug("Successfully parsed arguments...");

			return parsedArguments;
		}

		/// <summary>
		///    Checks the first ArgumentText for a CommandName.  Uses string.Empty for the Command Name in the
		///    Arguments List.
		///    Also starts tracking the first Argument if it is not a Command Name
		/// </summary>
		/// <param name="argumentText"></param>
		/// <param name="parsedArguments"></param>
		internal void ParseCommandName(
			string argumentText,
			List<Argument> parsedArguments)
		{
			if (!IsArgumentName(argumentText))
			{
				parsedArguments.Add(
					new Argument(
						string.Empty,
						argumentText.ToLower()));
			}
			else
			{
				CurrentArgumentName = argumentText;
			}
		}

		internal void ParseArgumentText(
			string argumentText,
			List<Argument> parsedArguments)
		{
			if (IsArgumentName(argumentText))
			{
				StartTrackingNewArgument(
					argumentText,
					parsedArguments);
				return;
			}

			if (CurrentlyTrackingAnArgument())
			{
				ParseArgumentValue(
					argumentText,
					parsedArguments);
				return;
			}

			throw new ArgumentException($"Invalid Argument: {argumentText}.");
		}

		private void ParseArgumentValue(
			string argumentText,
			List<Argument> parsedArguments)
		{
			if (StartsWithQuote(argumentText) &&
			    !EndsWithQuote(argumentText))
			{
				CurrentArgumentValue = argumentText;
				CurrentArgumentValueQuoteType = argumentText.First();
				return;
			}

			if (CurrentlyTrackingQuotedStringArgumentValue())
			{
				CurrentArgumentValue = string.Join(
					" ",
					CurrentArgumentValue,
					argumentText);

				if (CurrentArgumentValue.EndsWith(CurrentArgumentValueQuoteType.ToString()))
				{
					parsedArguments.Add(
						new Argument(
							CurrentArgumentName.ToLower(),
							CurrentArgumentValue.Trim(
								'"',
								'\'')));
					CurrentArgumentValue = string.Empty;
					CurrentArgumentValueQuoteType = null;
					CurrentArgumentName = null;
				}

				return;
			}

			parsedArguments.Add(
				new Argument(
					CurrentArgumentName.ToLower(),
					argumentText.Trim(
						'"',
						'\'')));
			CurrentArgumentName = null;
		}

		internal bool CurrentlyTrackingQuotedStringArgumentValue()
			=> CurrentArgumentValueQuoteType != null;

		internal bool StartsWithQuote(
			string argumentText) => argumentText.StartsWith("\"") || argumentText.StartsWith("'");

		internal bool EndsWithQuote(
			string argumentText) => argumentText.EndsWith("\"") || argumentText.EndsWith("'");

		internal void StartTrackingNewArgument(
			string argumentText,
			List<Argument> parsedArguments)
		{
			if (CurrentlyTrackingAnArgument())
			{
				ParseCurrentArgumentAsBoolean(parsedArguments);
			}

			CurrentArgumentName = argumentText;
		}

		internal void ParseCurrentArgumentAsBoolean(
			List<Argument> parsedArguments) => parsedArguments.Add(
			new Argument(
				CurrentArgumentName.ToLower(),
				true));

		internal bool CurrentlyTrackingAnArgument() => null != CurrentArgumentName;

		private void ParseFinalArgument(
			List<Argument> parsedArguments)
		{
			if (CurrentlyTrackingAnArgument())
			{
				ParseCurrentArgumentAsBoolean(parsedArguments);
			}

			if (CurrentlyTrackingQuotedStringArgumentValue())
			{
				throw new ArgumentException(
					$"Missing closing '{CurrentArgumentValueQuoteType}': {CurrentArgumentValue}.");
			}
		}

		internal bool IsArgumentName(
			string arg) => arg.StartsWith("-");
	}
}