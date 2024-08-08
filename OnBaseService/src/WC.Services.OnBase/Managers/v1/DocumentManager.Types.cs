// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   WC.Services.OnBase
//   DocumentManager.Types.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.OnBase.Managers.v1
{
	using System.Collections.Generic;
	using CareSource.WC.Services.OnBase.Models.v1;

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