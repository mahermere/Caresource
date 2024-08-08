// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   WC.Services.Document
//   KeywordManager.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.Document.Managers.v3
{
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.Linq;
	using System.Web.Http.ModelBinding;
	using CareSource.WC.Services.Document.Adapters.v3;
	using CareSource.WC.Services.Document.Models;


	/// <summary>
	/// </summary>
	/// <seealso cref="CareSource.WC.Services.Document.Managers.IKeywordManager" />
	public class KeywordManager : IKeywordManager
	{
		/// <summary>
		///    Initializes a new instance of the <see cref="KeywordManager" /> class.
		/// </summary>
		/// <param name="adapter">The adapter.</param>
		public KeywordManager(
			IKeywordAdapter adapter)
			=> _keywordAdapter = adapter;

		private readonly IKeywordAdapter _keywordAdapter;

		/// <summary>
		/// Updates the keywords.
		/// </summary>
		/// <param name="request">The request.</param>
		/// <returns></returns>
		public (IEnumerable<long> successfulIds, IDictionary<long, IEnumerable<string>> errors)
			UpdateKeywords(
				BatchUpdateKeywordsRequest request)
			=> _keywordAdapter.UpdateKeywords(
				request);

		/// <summary>
		/// Validates the request.
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