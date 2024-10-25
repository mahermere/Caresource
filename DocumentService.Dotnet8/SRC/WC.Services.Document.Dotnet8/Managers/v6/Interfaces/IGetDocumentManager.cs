// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   WC.Services.Document
//   IGetDocumentManager.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.Document.Dotnet8.Managers.v6.Interfaces
{
	using System.Collections.Generic;
	using System.Threading.Tasks;
	using WC.Services.Document.Dotnet8.Models.v6;
    using WC.Services.Document.Dotnet8.Models.v6.Interfaces;
    using WC.Services.Document.Dotnet8.Models.v6.Requests;

    /// <summary>
    /// </summary>
    /// <typeparam name="TDataModel">The type of the data model.</typeparam>
    public interface IGetDocumentManager<TDataModel>
	{
		/// <summary>
		///    Gets the document.
		/// </summary>
		/// <param name="documentId">The document identifier.</param>
		/// <param name="requestData">The request data.</param>
		/// <returns></returns>
		TDataModel GetDocument(
			long documentId,
			DownloadRequest requestData);

		TDataModel GetDocument(
			long documentId,
			IEnumerable<string> requestRequestData);

		/// <summary>
		///    Searches the specified request.
		/// </summary>
		/// <param name="request">The request.</param>
		/// <returns></returns>
		Task<(IEnumerable<TDataModel>, int)> SearchAsync(ISearchRequest request);
	}
}