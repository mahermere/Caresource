// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   CareSource.WC.OnBase.Core
//   DateHelper.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.OnBase.Core.ExtensionMethods
{
	using System;

	public static class DateTimeExtensions
	{
		public static DateTime SqlMaxDate(this DateTime dateTime) => new DateTime(
			9999,
			12,
			31);

		public static DateTime SqlMinDate(this DateTime dateTime) => new DateTime(
            1753,
			1,
			1);
	}
}