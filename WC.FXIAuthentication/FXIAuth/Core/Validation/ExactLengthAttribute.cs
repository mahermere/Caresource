// ------------------------------------------------------------------------------------------------
//  <copyright>
//    Copyright (c) CareSource, 2020-2022.  All rights reserved.
// 
//    FXIAuthentication
//    ExactLengthAttribute.cs
//  </copyright>
//  ------------------------------------------------------------------------------------------------

namespace FXIAuthentication.Core.Validation;

using System.ComponentModel.DataAnnotations;

/// <summary>
/// </summary>
/// <seealso cref="System.ComponentModel.DataAnnotations.StringLengthAttribute" />
public class ExactLengthAttribute : StringLengthAttribute
{
	public ExactLengthAttribute(
		int length)
		: base(length)
	{
		MinimumLength = length;
		ErrorMessage = "The {0} value must be {1} characters in length.";
	}
}