// ------------------------------------------------------------------------------------------------
//  <copyright>
//    Copyright (c) CareSource, 2020-2022.  All rights reserved.
// 
//    FXIAuthentication
//    VersionCommandModel.cs
//  </copyright>
//  ------------------------------------------------------------------------------------------------

namespace FXIAuthentication.Core.Console.Models;

public class VersionCommandModel : BaseCommandModel
{
	[CommandArgument(
		"full",
		"f",
		"Show the full version number.",
		Required = false)]
	public bool DisplayFullVersion { get; set; }
}