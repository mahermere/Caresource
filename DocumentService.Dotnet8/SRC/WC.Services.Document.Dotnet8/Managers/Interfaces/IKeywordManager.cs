// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   WC.Services.Document
//   IKeywordManager.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.Document.Dotnet8.Managers.Interfaces
{
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using System.Collections.Generic;
	using WC.Services.Document.Dotnet8.Models;

	/// <summary>
	/// 
	/// </summary>
	public interface IKeywordManager
	{
		/// <summary>
		/// Updates the keywords.
		/// </summary>
		/// <param name="request">The request containing the document and keywords to update.</param>
		/// <returns></returns>
		(IEnumerable<long> successfulIds, IEnumerable<string> errors) UpdateKeywords(
			BatchUpdateKeywordsRequest request);

		bool ValidateRequest(
			BatchUpdateKeywordsRequest request,
			ModelStateDictionary modelState);
	}
}