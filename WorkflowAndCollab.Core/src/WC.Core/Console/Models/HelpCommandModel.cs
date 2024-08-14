// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   Core
//   HelpCommandModel.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Core.Console.Models
{
	public class HelpCommandModel : BaseCommandModel
	{
		[CommandArgument(
			"command",
			"c",
			"Command to Display Help Information For.  (Optional)")]
		public string CommandName { get; set; }
	}
}