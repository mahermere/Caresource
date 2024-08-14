// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   Core
//   IXmlSerializeHelper.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Core.Helpers.Interfaces
{
	public interface IXmlSerializerHelper
	{
		/// <summary>
		///    Converts an object to a JSON serialized string
		/// </summary>
		/// <typeparam name="T">The type that we are serializing into XML</typeparam>
		/// <param name="obj">The object that is to be serialized.</param>
		/// <returns>
		///    A XML string representation of the
		///    <param name="obj"></param>
		/// </returns>
		string ToXml<T>(
			T obj);

		/// <summary>
		///    Converts a string to an instance of an object of type
		///    <typeparam name="T"></typeparam>
		///    from the json.
		/// </summary>
		/// <typeparam name="T">The type of the objec to be deserialized</typeparam>
		/// <param name="xml">The xml string.</param>
		/// <returns>an instace of the object with the provided data.</returns>
		T FromXml<T>(
			string xml);
	}
}