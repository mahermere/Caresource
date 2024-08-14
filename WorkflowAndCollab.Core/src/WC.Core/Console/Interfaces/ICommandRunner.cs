// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   Core
//   ICommandRunner.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Core.Console.Interfaces
{
	public interface ICommandRunner
	{
		int RunCommands(
			string[] args);
	}
}