// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   WC.Services.Document
//   IDocumentManager.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.Document.Managers.v6
{
	using System.Collections.Generic;
	using System.Web.Http.ModelBinding;
	using CareSource.WC.Entities.Documents;
	using CareSource.WC.Services.Document.Models.v6;
	using Document = CareSource.WC.Services.Document.Models.v6.Document;
	using DocumentHeader = CareSource.WC.Services.Document.Models.v6.DocumentHeader;

	/// <summary>
	///    Minimum functions and properties for a Document Manager
	/// </summary>
	public interface IDocumentManager
	{
		long DocumentCount(IFilteredRequest request);

		(IDictionary<string, int>, long) DocumentTypeCounts(IFilteredRequest request);

		Document Download(IDownloadRequest request);

		bool IsValid(
			IFilteredRequest request,
			ModelStateDictionary modelStateDictionary);

		bool IsValid(
			IDownloadRequest request,
			ModelStateDictionary modelStateDictionary);

		bool IsValid(
			ISearchRequest request,
			ModelStateDictionary modelStateDictionary);

		ISearchResult<DocumentHeader> Search(ISearchRequest request);
	}
}