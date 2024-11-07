// ------------------------------------------------------------------------------------------------
//  <copyright>
//    Copyright (c) CareSource, 2020-2022.  All rights reserved.
// 
//    FXIAuthentication
//    ICommandSelector.cs
//  </copyright>
//  ------------------------------------------------------------------------------------------------

namespace FXIAuthentication.Core.Console;

using FXIAuthentication.Core.Console.Models;

public interface ICommandSelector
{
	ICommand SelectCommand(
		IList<ICommand> commands,
		IList<Argument> arguments);
}