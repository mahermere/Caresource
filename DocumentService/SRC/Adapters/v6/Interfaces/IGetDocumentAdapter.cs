// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   WC.Services.Document
//   IGetDocumentAdapter.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.Document.Adapters.v6
{
	using System.Collections.Generic;
	using System.Threading.Tasks;
	using CareSource.WC.Services.Document.Models.v6;

	/// <summary>
	///    Interface
	/// </summary>
	/// <typeparam name="TDataModel"></typeparam>
	public interface IGetDocumentAdapter<out TDataModel>
	{
		/// <summary>
		///    Gets the document.
		/// </summary>
		/// <param name="documentId">The document identifier.</param>
		/// <param name="request">The request.</param>
		/// <returns></returns>
		TDataModel GetDocument(
			long documentId,
			IDownloadRequest request);

		/// <summary>
		///    Gets the document.
		/// </summary>
		/// <param name="documentId">The document identifier.</param>
		/// <param name="keywords">The list of request keyword values.</param>
		/// <returns></returns>
		TDataModel GetDocument(
			long documentId,
			IEnumerable<string> keywords);

		TDataModel GetDocument(long documentId);
	}
}