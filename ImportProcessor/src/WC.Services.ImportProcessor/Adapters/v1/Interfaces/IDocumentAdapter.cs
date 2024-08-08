// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2021. All rights reserved.
// 
//   ImportProcessor
//   IDocumentAdapter.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace ImportProcessor.Adapters.v1.Interfaces
{
	using System;
	using System.Collections.Generic;
	using CareSource.WC.Entities.Documents;
	using ImportProcessor.Models.v1;
	using Document = ImportProcessor.Models.v1.Document;

	public interface IDocumentAdapter
	{
		long CreateDocument(
			Document document,
			string cpsProcessingDirectory,
			Guid correlationGuid,
			string filename,
			string documentTypeName);

		string DocumentType(long documentTypeId);

		IEnumerable<string> UpdateKeywords(
			IEnumerable<KeywordUpdate> updates);

		IEnumerable<DocumentHeader> SearchDocuments(
			IEnumerable<string> guids,
			string docTypeName);

		(bool isValid, IEnumerable<long> badIds) ValidateDocumentIds(IEnumerable<long> documentIds);
	}
}