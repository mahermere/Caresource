// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   WorkFlowAndCollab.Integrations
//   IDocumentAdapter.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.Document.Adapters
{
	using CareSource.WC.Entities.Common.Interfaces;
	using CareSource.WC.Entities.Documents.Interfaces;

	/// <summary>
	/// Minimum Interface for the Search Document Adapter
	/// </summary>
	/// <typeparam name="TDataModel">The type of the data model.</typeparam>
	public interface ISearchDocumentAdapter<TDataModel>
	{
		/// <summary>
		/// Searches the documents.
		/// </summary>
		/// <param name="request">The request.</param>
		/// <returns></returns>
		ISearchResults<TDataModel> SearchDocuments(IListDocumentsRequest request);
	}
}