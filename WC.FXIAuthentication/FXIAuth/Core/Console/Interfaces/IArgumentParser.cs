// ------------------------------------------------------------------------------------------------
//  <copyright>
//    Copyright (c) CareSource, 2020-2022.  All rights reserved.
// 
//    FXIAuthentication
//    IArgumentParser.cs
//  </copyright>
//  ------------------------------------------------------------------------------------------------

namespace FXIAuthentication.Core.Console;

using FXIAuthentication.Core.Console.Models;

public interface IArgumentParser
{
	IList<Argument> ParseArguments(
		string[] args);
}