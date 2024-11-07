// ------------------------------------------------------------------------------------------------
//  <copyright>
//    Copyright (c) CareSource, 2020-2022.  All rights reserved.
// 
//    FXIAuthentication
//    DateTimeExtensions.cs
//  </copyright>
//  ------------------------------------------------------------------------------------------------

namespace FXIAuthentication.Core.Extensions;

public class DateTimeExtensions
{
	public static class DateHelper
	{
		public static DateTime SqlMinDate => new(
			1753,
			1,
			1);

		public static DateTime SqlMaxDate => new(
			9999,
			12,
			31);
	}
}