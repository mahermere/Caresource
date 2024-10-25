// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   WC.Services.Document
//   IKeywordAdapter.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.Document.Dotnet8.Adapters.v6.Interfaces
{
	using System.Collections.Generic;
    using WC.Services.Document.Dotnet8.Models.v6.Requests;

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