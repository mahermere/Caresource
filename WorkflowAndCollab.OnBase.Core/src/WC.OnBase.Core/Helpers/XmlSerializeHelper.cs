// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2018. All rights reserved.
// 
//   WorkFlowAndCollab.Tools
//   SerializeXml.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.OnBase.Core.Helpers
{
    using CareSource.WC.OnBase.Core.Helpers.Interfaces;
    using System.IO;
	using System.Xml;
	using System.Xml.Serialization;

	public class XmlSerializerHelper : IXmlSerializerHelper
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
		public string ToXml<T>(
			T obj)
		{
			XmlWriterSettings settings = new XmlWriterSettings
			{
				OmitXmlDeclaration = true
			};

			using (StringWriter sw = new StringWriter())
			{
				using (XmlWriter writer = XmlWriter.Create(
							sw,
							settings))
				{
					// removes namespace
					XmlSerializerNamespaces xmlns = new XmlSerializerNamespaces();
					xmlns.Add(
						string.Empty,
						string.Empty);

					XmlSerializer serializer = new XmlSerializer(typeof(T));
					serializer.Serialize(
						writer,
						obj,
						xmlns);

					return sw.ToString();
				}
			}
		}

		/// <summary>
		///    Converts a string to an instance of an object of type
		///    <typeparam name="T"></typeparam>
		///    from the json.
		/// </summary>
		/// <typeparam name="T">The type of the objec to be deserialized</typeparam>
		/// <param name="xml">The xml string.</param>
		/// <returns>an instace of the object with the provided data.</returns>
		public T FromXml<T>(
			string xml)
		{
			T result;
			XmlSerializer ser = new XmlSerializer(typeof(T));
			using (TextReader tr = new StringReader(xml))
			{
				result = (T)ser.Deserialize(tr);
			}

			return result;
		}
	}
}