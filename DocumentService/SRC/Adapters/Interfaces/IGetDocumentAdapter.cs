// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   WC.Services.Document
//   IGetDocumentAdapter.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.Document.Adapters
{
	using System.Collections.Generic;
	using System.Threading.Tasks;
	using CareSource.WC.Entities.Common.Interfaces;
	using CareSource.WC.Entities.Documents;
	using CareSource.WC.Entities.Documents.Interfaces;

	/// <summary>
	/// Interfa
	/// </summary>
	/// <typeparam name="TDataModel"></typeparam>
	public interface IGetDocumentAdapter<TDataModel>
	{
		/// <summary>
		/// Gets the document.
		/// </summary>
		/// <param name="documentId">The document identifier.</param>
		/// <param name="request">The request.</param>
		/// <returns></returns>
		TDataModel GetDocument(
			long documentId,
			GetDocumentRequest request);

		/// <summary>
		/// Gets the document.
		/// </summary>
		/// <param name="documentId">The document identifier.</param>
		/// <param name="keywords">The list of request keyword values.</param>
		/// <returns></returns>
		TDataModel GetDocument(
			long documentId,
			IEnumerable<string> keywords);

		/// <summary>
		/// Searches the documents.
		/// </summary>
		/// <param name="request">The request.</param>
		/// <returns></returns>
		Task<ISearchResults<TDataModel>> SearchDocumentsAsync(
			IListDocumentsRequest request);
	}
}