// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   WC.Services.Document
//   IKeywordAdapter.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.Document.Adapters.v6
{
	using System.Collections.Generic;
	using CareSource.WC.Services.Document.Models.v6;

	/// <summary>
	///    Minimum properties and methods needed to work with document keywords
	/// </summary>
	public interface IKeywordAdapter
	{
		/// <summary>
		///    Updates the keywords.
		/// </summary>
		/// <param name="updates">The updates.</param>
		(IEnumerable<long> successfulIds, IDictionary<long, IEnumerable<string>> errors)
			UpdateKeywords(BatchUpdateKeywordsRequest updates);
	}
}