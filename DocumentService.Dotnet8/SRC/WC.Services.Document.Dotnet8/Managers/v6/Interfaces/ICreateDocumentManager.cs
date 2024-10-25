// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   WC.Services.Document
//   ICreateDocumentManager.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.Document.Dotnet8.Managers.v6.Interfaces
{
    using Microsoft.AspNetCore.Mvc.ModelBinding;
	using WC.Services.Document.Dotnet8.Models.v6;

	public interface ICreateDocumentManager<out TDocumentDataModel>
	{
		TDocumentDataModel CreateDocument(CreateDocumentFileLinkRequest request);

		TDocumentDataModel CreateDocument(CreateDocumentRequest request);

		bool IsValid(
			CreateDocumentRequest request,
			ModelStateDictionary state);
	}
}