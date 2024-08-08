// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2023. All rights reserved.
// 
//   WC.Services.Document
//   ISearchAdapter.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.Document.Adapters.v6
{
	using System.Collections.Generic;
	using CareSource.WC.Services.Document.Models.v6;

	public interface ISearchAdapter
	{
		IEnumerable<Document> SearchDocuments(ISearchRequest request);
	}
}