// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   WC.Services.OnBase
//   IDocumentManager.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.OnBase.Dotnet8.Managers.Interfaces.v1
{
	using System.Collections.Generic;
    using CareSource.WC.Entities.Requests.Base;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using WC.Services.OnBase.Dotnet8.Models.v1;

    public interface IDocumentManager
	{
		DocumentTypeGroup GetDocumentTypeGroup(long documentTypeId);
		IEnumerable<DocumentTypeGroup> ListDocumentTypeGroups();

		IEnumerable<DocumentType> ListDocumentTypes();

		DocumentType GetDocumentType(long documentTypeId);

		IEnumerable<DocumentType> ListDocumentTypes(long documentTypeGroupId);

		IEnumerable<Keyword> ListKeywords();

		IEnumerable<Keyword> ListKeywords(long documentTypeId);

		IEnumerable<KeywordTypeGroup> ListKeywordTypeGroups();

		bool ValidateRequest(BaseRequest request, ModelStateDictionary modelState);

		IEnumerable<DocumentType> SearchByKeyword(string keywordName);
	}
}