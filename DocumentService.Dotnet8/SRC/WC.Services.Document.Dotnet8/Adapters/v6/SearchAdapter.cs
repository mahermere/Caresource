// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2023. All rights reserved.
// 
//   WC.Services.Document
//   SearchAdapter.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.Document.Dotnet8.Adapters.v6
{
	using System;
	using System.Collections.Generic;
	using WC.Services.Document.Dotnet8.Models.v6;
	using Microsoft.Extensions.Logging;
    //using WC.Services.Document.MVC.Dotnet8.Adapters.v6.Interfaces;
    using WC.Services.Document.Dotnet8.Models.v6.Interfaces;
    using WC.Services.Document.Dotnet8.Adapters.v6.Interfaces;

    public class SearchAdapter : ISearchAdapter
	{
		private readonly log4net.ILog _logger;
		private readonly ISqlAdapter _adapter;

		public SearchAdapter(
			log4net.ILog logger,
			ISqlAdapter adapter)
		{
			_logger = logger;
			_adapter = adapter;
		}
		public IEnumerable<Document> SearchDocuments(ISearchRequest request)
		{
			_adapter.SearchDocuments(request);
			return new List<Document>();
		}
	}
}