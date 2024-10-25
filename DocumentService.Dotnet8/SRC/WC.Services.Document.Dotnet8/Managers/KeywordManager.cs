// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   WC.Services.Document
//   KeywordManager.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.Document.Dotnet8.Managers
{
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using System;
	using System.Collections.Generic;
	using System.Globalization;
	using System.Linq;
	using WC.Services.Document.Dotnet8.Adapters;
    using WC.Services.Document.Dotnet8.Adapters.Interfaces;
    using WC.Services.Document.Dotnet8.Managers.Interfaces;
    using WC.Services.Document.Dotnet8.Models;
   

    /// <summary>
    /// </summary>
    /// <seealso cref=" WC.Services.Document.Dotnet8.Managers.IKeywordManager" />
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
		public (IEnumerable<long> successfulIds, IEnumerable<string> errors) UpdateKeywords(BatchUpdateKeywordsRequest request)
			=> _keywordAdapter.UpdateKeywords(request.RequestData);

		/// <summary>
		/// Validates the request.
		/// </summary>
		/// <param name="request">The request.</param>
		/// <param name="modelState">State of the model.</param>
		/// <returns></returns>
		public bool ValidateRequest(
			BatchUpdateKeywordsRequest request,
			ModelStateDictionary modelState)
		{
			(bool isValid, IEnumerable<long> badIds) validateDocumentsResult =
				_keywordAdapter.ValidateDocumentIds(request.RequestData.Select(d => d.DocumentId));

			if (!validateDocumentsResult.isValid)
			{
				foreach(long id in validateDocumentsResult.badIds)
				{ 
					modelState.AddModelError("Invalid Document Ids", id.ToString());
				}
				return modelState.IsValid;
			}
			
			(bool isValid, Dictionary<long, IEnumerable<string>> badKeywords) validateKeywordsResult =
				_keywordAdapter.ValidateKeywords(request.RequestData);

			if (!validateKeywordsResult.isValid)
			{
				foreach (KeyValuePair<long, IEnumerable<string>> keyword in validateKeywordsResult.badKeywords)
				{
					foreach (string invalid in keyword.Value)
					{
						modelState.AddModelError(
							$"Invalid Keywords for {keyword.Key}",
							invalid);
					}
				}
				
				return modelState.IsValid;
			}

			return modelState.IsValid;
		}
	}
}