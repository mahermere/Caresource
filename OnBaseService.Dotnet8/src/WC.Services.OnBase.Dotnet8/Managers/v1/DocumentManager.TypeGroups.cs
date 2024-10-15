// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   WC.Services.OnBase
//   DocumentManager.TypeGroups.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.OnBase.Dotnet8.Managers.v1
{
    using global::WC.Services.OnBase.Dotnet8.Models.v1;
    using System.Collections.Generic;


	public partial class DocumentManager
	{
		public DocumentTypeGroup GetDocumentTypeGroup(long documentTypeGroupId)
			=> _adapter.GetDocumentTypeGroup(documentTypeGroupId);

		public IEnumerable<DocumentTypeGroup> ListDocumentTypeGroups()
			=> _adapter.DocumentTypeGroups();
	}
}