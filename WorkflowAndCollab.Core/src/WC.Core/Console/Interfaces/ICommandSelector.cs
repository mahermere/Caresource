// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   Core
//   ICommandSelector.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Core.Console.Interfaces
{
	using System.Collections.Generic;
	using CareSource.WC.Core.Console.Models;

	public interface ICommandSelector
	{
		ICommand SelectCommand(
			IList<ICommand> commands,
			IList<Argument> arguments);
	}
}