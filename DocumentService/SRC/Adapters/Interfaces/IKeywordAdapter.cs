// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   WC.Services.Document
//   IKeywordAdapter.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.Document.Adapters
{
	using System.Collections.Generic;
	using CareSource.WC.Services.Document.Models;

	/// <summary>
	/// Minimum properties and methods needed to work with document keywords
	/// </summary>
	public interface IKeywordAdapter
	{
		/// <summary>
		/// Updates the keywords.
		/// </summary>
		/// <param name="updates">The updates.</param>
		(IEnumerable<long> successfulIds, IEnumerable<string> errors) UpdateKeywords(
			IEnumerable<KeywordUpdate> updates);

		/// <summary>
		/// Validates the document ids.
		/// </summary>
		/// <param name="documentIds">The document ids.</param>
		/// <returns></returns>
		(bool isValid, IEnumerable<long> badIds) ValidateDocumentIds(IEnumerable<long> documentIds);

		(bool isValid, Dictionary<long, IEnumerable<string>> badKeywords) ValidateKeywords(
			IEnumerable<KeywordUpdate> requestRequestData);
	}
}