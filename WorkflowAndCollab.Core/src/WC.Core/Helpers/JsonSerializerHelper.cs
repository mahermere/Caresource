// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   Core
//   JsonSerializerHelper.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Core.Helpers
{
	using CareSource.WC.Core.Helpers.Interfaces;
	using Newtonsoft.Json;

	/// <summary>
	///    Serialization helper class to serialize and deserialize objects to and from JSON
	/// </summary>
	public class JsonSerializerHelper : IJsonSerializerHelper
	{
		/// <summary>
		///    Converts an object to a JSON serialized string
		/// </summary>
		/// <typeparam name="T">The type that we are serializing into JSON</typeparam>
		/// <param name="obj">The object that is to be serialized.</param>
		/// <returns>
		///    A JSON string representation of the
		///    <param name="obj"></param>
		/// </returns>
		public string ToJson<T>(
			T obj)
			=> JsonConvert.SerializeObject(obj);


		/// <summary>
		///    Converts a string to an instance of an object of type
		///    <typeparam name="T"></typeparam>
		///    from the json.
		/// </summary>
		/// <typeparam name="T">The type of the objec to be deserialized</typeparam>
		/// <param name="json">The json string.</param>
		/// <returns>an instace of the object with the provided data.</returns>
		public T FromJson<T>(
			string json)
			=> JsonConvert.DeserializeObject<T>(json);
	}
}