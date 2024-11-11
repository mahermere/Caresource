// ------------------------------------------------------------------------------------------------
//  <copyright>
//    Copyright (c) CareSource, 2020-2022.  All rights reserved.
// 
//    FXIAuthentication
//    CommandArgumentAttribute.cs
//  </copyright>
//  ------------------------------------------------------------------------------------------------

namespace FXIAuthentication.Core.Console.Models;

[AttributeUsage(AttributeTargets.Property)]
public class CommandArgumentAttribute : Attribute
{
	public CommandArgumentAttribute(
		string argumentName,
		string argumentAbbreviation,
		string description = null)
	{
		ArgumentName = argumentName;
		ArgumentAbbreviation = argumentAbbreviation;
		Description = description;
	}

	public string ArgumentName { get; set; }

	public string ArgumentAbbreviation { get; set; }

	public string Description { get; set; }

	public bool Required { get; set; } = false;
}