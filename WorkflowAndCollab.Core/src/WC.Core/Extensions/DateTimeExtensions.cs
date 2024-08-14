// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   Core
//   DateTimeExtensions.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Core.Extensions
{
	using System;

	public class DateTimeExtensions
	{
		public static class DateHelper
		{
			public static DateTime SqlMinDate => new DateTime(
				1753,
				1,
				1);

			public static DateTime SqlMaxDate => new DateTime(
				9999,
				12,
				31);
		}
	}
}