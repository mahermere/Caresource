// ------------------------------------------------------------------------------------------------
//  <copyright>
//    Copyright (c) CareSource, 2020-2022.  All rights reserved.
// 
//    FXIAuthentication
//    Argument.cs
//  </copyright>
//  ------------------------------------------------------------------------------------------------

namespace FXIAuthentication.Core.Console.Models;

public class Argument
{
	public Argument(
		string argumentText,
		object value)
	{
		ArgumentText = argumentText;
		Value = value;
	}

	public string ArgumentText { get; set; }

	public object Value { get; set; }

	public bool Processed { get; set; } = false;
}