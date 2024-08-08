// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2023. All rights reserved.
// 
//   WC.Services.Document
//   ISqlAdapter.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.Document.Adapters.v6
{
	using System.Collections.Generic;
	using CareSource.WC.Services.Document.Models.v6;

	public interface ISqlAdapter
	{
		ISearchResult<DocumentHeader> SearchDocuments(ISearchRequest request);
	}
}