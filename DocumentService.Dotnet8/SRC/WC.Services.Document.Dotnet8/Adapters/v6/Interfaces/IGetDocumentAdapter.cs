// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   WC.Services.Document
//   IGetDocumentAdapter.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.Document.Dotnet8.Adapters.v6.Interfaces
{
	using System.Collections.Generic;
    using WC.Services.Document.Dotnet8.Models.v6.Interfaces;


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