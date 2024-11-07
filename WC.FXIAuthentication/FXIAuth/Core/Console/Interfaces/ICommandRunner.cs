// ------------------------------------------------------------------------------------------------
//  <copyright>
//    Copyright (c) CareSource, 2020-2022.  All rights reserved.
// 
//    FXIAuthentication
//    ICommandRunner.cs
//  </copyright>
//  ------------------------------------------------------------------------------------------------

namespace FXIAuthentication.Core.Console;

public interface ICommandRunner
{
	int RunCommands(
		string[] args);
}