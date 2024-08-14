// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   Core
//   ICommand.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Core.Console.Interfaces
{
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
}