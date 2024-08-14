// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   Core
//   VersionCommandModel.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Core.Console.Models
{
	public class VersionCommandModel : BaseCommandModel
	{
		[CommandArgument(
			"full",
			"f",
			"Show the full version number.",
			Required = false)]
		public bool DisplayFullVersion { get; set; }
	}
}