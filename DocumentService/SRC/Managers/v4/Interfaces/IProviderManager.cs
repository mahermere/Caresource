// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   WC.Services.Document
//   IProviderManager.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.Document.Managers.v4
{
	using System.Collections.Generic;
	using CareSource.WC.Entities.Common.Interfaces;
	using CareSource.WC.Entities.Documents;
	using CareSource.WC.Services.Document.Models.v4;

	public interface IProviderManager
	{
		/// <summary>
		/// Gets the document types count.
		/// </summary>
		/// <param name="providerId">The provider identifier.</param>
		/// <param name="request">The request.</param>
		/// <returns></returns>
		IDictionary<string, int> GetDocumentTypesCount(
			string providerId,
			DocumentTypesCountRequest request);


		/// <summary>
		/// Searches the specified provider identifier.
		/// </summary>
		/// <param name="providerId">The provider identifier.</param>
		/// <param name="request">The request.</param>
		/// <returns></returns>
		ISearchResults<DocumentHeader> Search(
			string providerId,
			ListDocumentsRequest request);

	}
}