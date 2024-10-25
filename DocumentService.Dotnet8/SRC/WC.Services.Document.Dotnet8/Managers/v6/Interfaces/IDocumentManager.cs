// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   WC.Services.Document
//   IDocumentManager.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.Document.Dotnet8.Managers.v6.Interfaces
{
	using System.Collections.Generic;
	using CareSource.WC.Entities.Documents;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using WC.Services.Document.Dotnet8.Models.v6;
    using WC.Services.Document.Dotnet8.Models.v6.Interfaces;
    using Document = WC.Services.Document.Dotnet8.Models.v6.Document;
	using DocumentHeader = WC.Services.Document.Dotnet8.Models.v6.DocumentHeader;

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