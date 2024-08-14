//  ------------------------------------------------------------------------------------------------
//  <copyright>
//    Copyright (c) CareSource, 2020-2022.  All rights reserved.
// 
//    OnBase.Core
//    DictionaryExtensionMethods.cs
//  </copyright>
//  ------------------------------------------------------------------------------------------------

namespace CareSource.WC.OnBase.Core.ExtensionMethods
{
	using System.Collections.Generic;

	public static class DictionaryExtensionMethods
	{
		/// <summary>
		/// Gets the value or default.
		/// </summary>
		/// <typeparam name="TKey">The type of the key.</typeparam>
		/// <typeparam name="TValue">The type of the value.</typeparam>
		/// <param name="dictionary">The dictionary.</param>
		/// <param name="key">The key.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <returns></returns>
		public static TValue GetValueOrDefault<TKey, TValue>(
			this IDictionary<TKey, TValue> dictionary,
			TKey key,
			TValue defaultValue)
		{
			TValue value = dictionary[key];

			return value == null ? defaultValue : value;
		}
	}
}