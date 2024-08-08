// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   WC.Services.Document
//   ICreateDocumentManager.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.Document.Managers.v5
{
	using System.Web.Http.ModelBinding;
	using CareSource.WC.Services.Document.Models.v5;

	public interface ICreateDocumentManager<out TDocumentDataModel>
	{
		TDocumentDataModel CreateDocument(CreateDocumentFileLinkRequest request);

		TDocumentDataModel CreateDocument(CreateDocumentRequest request);

		bool IsValid(
			CreateDocumentRequest request,
			ModelStateDictionary state);
	}
}