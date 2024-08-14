// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   Core
//   IArgumentParser.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Core.Console.Interfaces
{
	using System.Collections.Generic;
	using CareSource.WC.Core.Console.Models;

	public interface IArgumentParser
	{
		IList<Argument> ParseArguments(
			string[] args);
	}
}