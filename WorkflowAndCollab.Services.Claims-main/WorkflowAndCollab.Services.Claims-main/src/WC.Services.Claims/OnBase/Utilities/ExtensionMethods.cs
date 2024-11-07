// ------------------------------------------------------------------------------------------------
//  <copyright>
//    Copyright (c) CareSource, 2020-2023.  All rights reserved.
// 
//    Claims
//    ExtensionMethods.cs
//  </copyright>
//  ------------------------------------------------------------------------------------------------

namespace Claims.OnBase.Utilities
{
	public static class ExtensionMethods
	{
		public static bool IsNullOrWhiteSpace(this string value) => string.IsNullOrWhiteSpace(value);
	}
}