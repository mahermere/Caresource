// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   WC.Services.Document
//   IKeywordManager.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.Document.Dotnet8.Managers.v6.Interfaces
{
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using System.Collections.Generic;
	using WC.Services.Document.Dotnet8.Models.v6;
    using WC.Services.Document.Dotnet8.Models.v6.Requests;

    /// <summary>
    /// </summary>
    public interface IKeywordManager
	{
		/// <summary>
		///    Updates the keywords.
		/// </summary>
		/// <param name="request">The request containing the document and keywords to update.</param>
		/// <returns></returns>
		(IEnumerable<long> successfulIds, IDictionary<long, IEnumerable<string>> errors)
			UpdateKeywords(
				BatchUpdateKeywordsRequest request);

		bool ValidateRequest(
			BatchUpdateKeywordsRequest request,
			ModelStateDictionary modelState);
	}
}