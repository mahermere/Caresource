// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   Caresource.WC.Services.Document.WC.Services.Document
//   IDocumentManager.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.Document.Dotnet8.Managers.Interfaces
{
	using CareSource.WC.Entities.Common.Interfaces;
	using CareSource.WC.Entities.Documents.Interfaces;

	/// <summary>
	/// Minimum functions an properties for a Document Manager
	/// </summary>
	/// <typeparam name="TDataModel">The type of the data model.</typeparam>
	public interface IDocumentManager<TDataModel>
	{
		/// <summary>
		/// Searches the specified request data.
		/// </summary>
		/// <param name="requestData">The request data.</param>
		/// <returns></returns>
		ISearchResults<TDataModel> Search(IListDocumentsRequest requestData);

		/// <summary>
		/// Searches the specified identifier.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <param name="requestData">The request data.</param>
		/// <returns></returns>
		ISearchResults<TDataModel> Search(
			string id,
			IListDocumentsRequest requestData);
	}
}