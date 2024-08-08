// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   WC.Services.Document
//   GetDocumentManager.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.Document.Managers.v4
{
	using System.Collections.Generic;
	using System.Threading.Tasks;
	using CareSource.WC.Entities.Documents;

	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="TDataModel">The type of the data model.</typeparam>
	public interface IGetDocumentManager<TDataModel>
	{
		/// <summary>
		/// Gets the document.
		/// </summary>
		/// <param name="documentId">The document identifier.</param>
		/// <param name="requestData">The request data.</param>
		/// <returns></returns>
		TDataModel GetDocument(
			long documentId,
			GetDocumentRequest requestData);

		/// <summary>
		/// Searches the specified request.
		/// </summary>
		/// <param name="request">The request.</param>
		/// <returns></returns>
		Task<(IEnumerable<TDataModel>, int)> SearchAsync(
			ListDocumentsRequest request);

		TDataModel GetDocument(
			long documentId,
			IEnumerable<string> requestRequestData);
	}
}