// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   WC.Services.Document
//   OnBaseDocument.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.OnBase.Dotnet8.Models
{
	using System.Collections.Generic;
	using System.IO;

	public class OnBaseDocumentRequest
	{
		/// <summary>
		///    Gets or sets the file data of the document class.
		/// </summary>
		/// <remarks>
		///	the Stream data containing the bytes of the file itself
		/// </remarks>
		public Stream FileData { get; set; }

		/// <summary>
		///    Gets or sets the filename of the document class.
		/// </summary>
		/// <remarks>
		///	The Name and file extension.
		/// </remarks>
		public string Filename { get; set; }

		/// <summary>
		///    Gets or sets the identifier of the document class.
		/// </summary>
		public long Id { get; set; }

		/// <summary>
		///    Gets or sets the name of the document class.
		/// </summary>
		/// <remarks>
		///	The OnBase Stored Name for the document.
		/// </remarks>
		public string Name { get; set; }

		/// <summary>
		///    Gets or sets the type of the document class.
		/// </summary>
		/// <remarks>
		///	The OnBase Document Type
		/// </remarks>
		public string Type { get; set; }

		/// <summary>
		/// Gets or sets the On Base Document Keywords
		/// </summary>
		public IDictionary<string, string> Keywords { get; set; }
	}
}