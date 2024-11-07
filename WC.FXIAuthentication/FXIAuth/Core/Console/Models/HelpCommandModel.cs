// ------------------------------------------------------------------------------------------------
//  <copyright>
//    Copyright (c) CareSource, 2020-2022.  All rights reserved.
// 
//    FXIAuthentication
//    HelpCommandModel.cs
//  </copyright>
//  ------------------------------------------------------------------------------------------------

namespace FXIAuthentication.Core.Console.Models;

public class HelpCommandModel : BaseCommandModel
{
	[CommandArgument(
		"command",
		"c",
		"Command to Display Help Information For.  (Optional)")]
	public string CommandName { get; set; }
}