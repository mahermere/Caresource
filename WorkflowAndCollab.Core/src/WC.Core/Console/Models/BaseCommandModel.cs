// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   Core
//   BaseCommandModel.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Core.Console.Models
{
	public abstract class BaseCommandModel
	{
		[CommandArgument(
			"correlation-guid",
			"c",
			"Guid used for tracing data as it maniplated/moved between processes.",
			Required = false)]
		public string CorrelationGuid { get; set; }
	}
}