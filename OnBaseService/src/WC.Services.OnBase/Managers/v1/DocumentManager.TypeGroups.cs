// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   WC.Services.OnBase
//   DocumentManager.TypeGroups.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.OnBase.Managers.v1
{
	using System.Collections.Generic;
	using CareSource.WC.Services.OnBase.Models.v1;

	public partial class DocumentManager
	{
		public DocumentTypeGroup GetDocumentTypeGroup(long documentTypeGroupId)
			=> _adapter.GetDocumentTypeGroup(documentTypeGroupId);

		public IEnumerable<DocumentTypeGroup> ListDocumentTypeGroups()
			=> _adapter.DocumentTypeGroups();
	}
}