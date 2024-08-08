// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   WC.Services.Document
//   ICreateDocumentAdapter.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.Document.Adapters.v6
{
	using System.Collections.Generic;
	using System.IO;

	public interface ICreateDocumentAdapter
	{
		long? CreateDocument(
			string documentType,
			IDictionary<string, string> keywords,
			string fileName,
			List<Stream> documentPages);

		long? CreateDocument(
			string documentType,
			IDictionary<string, string> keywords);
	}
}