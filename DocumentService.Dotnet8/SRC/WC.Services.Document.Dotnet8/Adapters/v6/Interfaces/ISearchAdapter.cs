// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2023. All rights reserved.
// 
//   WC.Services.Document
//   ISearchAdapter.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.Document.Dotnet8.Adapters.v6.Interfaces
{
	using System.Collections.Generic;
	using WC.Services.Document.Dotnet8.Models.v6;
    using WC.Services.Document.Dotnet8.Models.v6.Interfaces;

    public interface ISearchAdapter
	{
		IEnumerable<Document> SearchDocuments(ISearchRequest request);
	}
}