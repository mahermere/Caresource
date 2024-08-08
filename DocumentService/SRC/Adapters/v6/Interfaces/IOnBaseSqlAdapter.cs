// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   WC.Services.Document
//   IOnBaseSqlAdapter.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.Document.Adapters.v6
{
	using System.Collections.Generic;
	using CareSource.WC.Services.Document.Models.v6;

	/// <summary>
	/// Adapter that uses SQL to Search the OnBase DB for Documents
	/// </summary>
	public interface IOnBaseSqlAdapter<TSearchType>
	{
		///  <summary>
		///  Searches the specified request.
		///  </summary>
		///  <param name="request">The request.</param>
		///  <returns>
		/// 	Documents matching the requirements of the request
		///  </returns>
		ISearchResult<DocumentHeader> Search(ISearchRequest request);

		///  <summary>
		///  Gets the total records.
		///  </summary>
		///  <param name="request">The request.</param>
		///  <returns>
		/// 	A total count of Documents matching the search request
		///  </returns>
		long TotalRecords(IFilteredRequest request);

		/// <summary>
		/// Gets the document types count.
		/// </summary>
		/// <param name="request">The request.</param>
		/// <returns>
		///	Returns a dictionary of document types and the total records for each type 
		/// </returns>
		IDictionary<string, int> DocumentTypesCount(IFilteredRequest request);
	}
}