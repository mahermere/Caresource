// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   WC.Services.Document
//   KeywordManager.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.Document.Managers.v6
{
	using System.Collections.Generic;
	using System.Web.Http.ModelBinding;
	using CareSource.WC.Services.Document.Adapters.v6;
	using CareSource.WC.Services.Document.Models.v6;

	/// <summary>
	/// </summary>
	/// <seealso cref="CareSource.WC.Services.Document.Managers.IKeywordManager" />
	public class KeywordManager : IKeywordManager
	{
		private readonly IKeywordAdapter _keywordAdapter;

		/// <summary>
		///    Initializes a new instance of the <see cref="KeywordManager" /> class.
		/// </summary>
		/// <param name="adapter">The adapter.</param>
		public KeywordManager(
			IKeywordAdapter adapter)
			=> _keywordAdapter = adapter;

		/// <summary>
		///    Updates the keywords.
		/// </summary>
		/// <param name="request">The request.</param>
		/// <returns></returns>
		public (IEnumerable<long> successfulIds, IDictionary<long, IEnumerable<string>> errors)
			UpdateKeywords(
				BatchUpdateKeywordsRequest request)
			=> _keywordAdapter.UpdateKeywords(
				request);

		/// <summary>
		///    Validates the request.
		/// </summary>
		/// <param name="request">The request.</param>
		/// <param name="modelState">State of the model.</param>
		/// <returns></returns>
		public bool ValidateRequest(
			BatchUpdateKeywordsRequest request,
			ModelStateDictionary modelState)
			=> modelState.IsValid;
	}
}