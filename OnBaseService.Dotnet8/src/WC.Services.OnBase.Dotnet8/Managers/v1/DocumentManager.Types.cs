// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   WC.Services.OnBase
//   DocumentManager.Types.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.OnBase.Dotnet8.Managers.v1
{
	using System.Collections.Generic;
    using global::WC.Services.OnBase.Dotnet8.Models.v1;

    public partial class DocumentManager
	{
		public IEnumerable<DocumentType> ListDocumentTypes()
			=> _adapter.DocumentTypes();

		public DocumentType GetDocumentType(long documentTypeId)
			=> _adapter.DocumentType(documentTypeId);

		public IEnumerable<DocumentType> ListDocumentTypes(long documentTypeGroupId)
			=> _adapter.DocumentTypes(documentTypeGroupId);

		public IEnumerable<DocumentType> SearchByKeyword(string keywordName)
			=> _adapter.SearchByKeyword(keywordName);
	}
}