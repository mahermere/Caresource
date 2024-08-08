// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2023. All rights reserved.
// 
//   WC.Services.Document
//   IEnumerableExtensions.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.Document
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	public static class EnumerableExtensionMethods
	{
		public static bool SafeAny<T>(this IEnumerable<T> iEnumerable)
			=> iEnumerable != null && iEnumerable.Any();

		public static bool SafeAny<T>(
			this IEnumerable<T> source,
			Func<T, bool> predicate)
			=> source != null && source.Any(predicate);
	}
}