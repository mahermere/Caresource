// ------------------------------------------------------------------------------------------------
//  <copyright>
//    Copyright (c) CareSource, 2020-2022.  All rights reserved.
// 
//    FXIAuthentication
//    VersionCommand.cs
//  </copyright>
//  ------------------------------------------------------------------------------------------------

namespace FXIAuthentication.Core.Console.Commands;

using FXIAuthentication.Core.Console.Models;
using Microsoft.Extensions.PlatformAbstractions;

public class VersionCommand : ICommand<VersionCommandModel>
{
	private readonly ILogger _logger;

	public VersionCommand(
		ILogger<VersionCommand> logger) => _logger = logger;

	public string CommandName => "version";

	public string CommandDescription => "Writes the Current Application Version.";

	public int Run(
		VersionCommandModel commandModel)
	{
		string applicationVersion = PlatformServices.Default.Application.ApplicationVersion;

		applicationVersion = FormatApplicationVersion(
			applicationVersion,
			commandModel.DisplayFullVersion);

		_logger.LogInformation(
			$"{PlatformServices.Default.Application.ApplicationName} {applicationVersion}");

		return 0;
	}

	internal static string FormatApplicationVersion(
		string applicationVersion,
		bool displayFullVersion)
	{
		string[] applicationVersionParts = applicationVersion.Split('.');

		if (displayFullVersion)
		{
			return applicationVersion;
		}

		return
			$"{applicationVersionParts[0]}.{applicationVersionParts[1]}.{applicationVersionParts[2]}";
	}
}