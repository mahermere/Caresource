// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2023. All rights reserved.
// 
//   WC.Services.Document
//   ICreateDocumentManager.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.Document.Managers.v3
{
	using CareSource.WC.Services.Document.Models.v3;

	public interface ICreateDocumentManager<TDocumentDataModel>
	{
		TDocumentDataModel CreateDocument(CreateDocumentFileLinkRequest request);
	}
}