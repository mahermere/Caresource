// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   Caresource.WC.Services.Document.WC.Services.Document
//   IProviderDocumentManager.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.Document.Managers
{
	using CareSource.WC.Entities.Common.Interfaces;
	using CareSource.WC.Entities.Documents.Interfaces;

	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="TDataModel">The type of the data model.</typeparam>
	/// <seealso cref="IDocumentManager{TDataModel}" />
	public interface IProviderDocumentManager<TDataModel> : IDocumentManager<TDataModel>
	{
		/// <summary>
		/// Searches the tin.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <param name="requestData">The request data.</param>
		/// <returns></returns>
		ISearchResults<TDataModel> SearchTin(
			string id,
			IListDocumentsRequest requestData);
	}
}