// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   WC.Services.Document
//   IKeywordManager.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.Document.Managers.v6
{
	using System.Collections.Generic;
	using System.Web.Http.ModelBinding;
	using CareSource.WC.Services.Document.Models.v6;

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