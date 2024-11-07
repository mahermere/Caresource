// ------------------------------------------------------------------------------------------------
//  <copyright>
//    Copyright (c) CareSource, 2020-2022.  All rights reserved.
// 
//    FXIAuthentication
//    ICommand.cs
//  </copyright>
//  ------------------------------------------------------------------------------------------------

namespace FXIAuthentication.Core.Console;

public interface ICommand
{
	string CommandName { get; }

	string CommandDescription { get; }
}

public interface ICommand<in TCommandModel> : ICommand
	where TCommandModel : class
{
	int Run(
		TCommandModel commandModel);
}