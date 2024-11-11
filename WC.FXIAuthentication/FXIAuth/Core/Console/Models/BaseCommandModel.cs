// ------------------------------------------------------------------------------------------------
//  <copyright>
//    Copyright (c) CareSource, 2020-2022.  All rights reserved.
// 
//    FXIAuthentication
//    BaseCommandModel.cs
//  </copyright>
//  ------------------------------------------------------------------------------------------------

namespace FXIAuthentication.Core.Console.Models;

public abstract class BaseCommandModel
{
	[CommandArgument(
		"correlation-guid",
		"c",
		"Guid used for tracing data as it maniplated/moved between processes.",
		Required = false)]
	public string CorrelationGuid { get; set; }
}