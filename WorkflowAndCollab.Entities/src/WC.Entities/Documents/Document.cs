// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   Entities
//   Document.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Entities.Documents
{
	using System.Collections.Generic;

	/// <summary>
	///    Represents the data used to define a document
	/// </summary>
	public class Document
	{
		/// <summary>
		///    Gets or sets the Document Display Columns
		/// </summary>
		public Dictionary<string, string> DisplayColumns { get; set; }

		/// <summary>
		///    Gets or sets the file data of the document.
		/// </summary>
		/// <remarks>
		///    This is a Base 64 encoded string of the bytes in the file
		/// </remarks>
		public string FileData { get; set; }

		/// <summary>
		///    Gets or sets the filename of the document.
		/// </summary>
		public string Filename { get; set; }

		/// <summary>
		///    Gets or sets the identifier of the document.
		/// </summary>
		public long Id { get; set; }

		/// <summary>
		///    Gets or sets the name of the document.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		///    Gets or sets the type of the document.
		/// </summary>
		/// <remarks>
		///    The "Document Type" name in the system that is storing the document
		/// </remarks>
		public string Type { get; set; }
	}
}