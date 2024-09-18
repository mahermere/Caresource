// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2021. All rights reserved.
// 
//   WC.Services.Hplc
//   ExtensionMethods.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.HplcManagement.Dotnet8
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;
	//using CareSource.WC.OnBase.Core.ExtensionMethods;

	/// <summary>
	///    Data and functions describing a CareSource.WC.Services.Hplc.Models.v1.ExtensionMethods object.
	/// </summary>
	public static class ExtensionMethods
	{
		/// <summary>
		///    Trims all strings.
		/// </summary>
		/// <typeparam name="TSelf">The type of the self.</typeparam>
		/// <param name="obj">The object.</param>
		public static void TrimAllStrings<TSelf>(this TSelf obj)
		{
			BindingFlags flags = BindingFlags.Instance
				| BindingFlags.Public
				| BindingFlags.NonPublic
				| BindingFlags.FlattenHierarchy;

			foreach (PropertyInfo prop in obj.GetType()
				.GetProperties(flags)
				.Where(p => p.PropertyType == typeof(string) && p.CanWrite))
			{
				string currentValue = (string)prop.GetValue(
					obj,
					null);

				prop.SetValue(
					obj,
					currentValue.Trim(),
					null);
			}
		}

		public static bool SafeAny<TSource>(this IEnumerable<TSource> source)
			=> source != null && source.Any();

		public static bool SafeAny<TSource>(
			this IEnumerable<TSource> source,
			Func<TSource, bool> predicate)
		=> source != null && source.Any(predicate);
	}
}