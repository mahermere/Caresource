// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2023. All rights reserved.
// 
//   WC.Services.Document
//   SearchAdapter.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.Document.Adapters.v6
{
	using System;
	using System.Collections.Generic;
	using CareSource.WC.Services.Document.Models.v6;
	using Microsoft.Extensions.Logging;

	public class SearchAdapter : ISearchAdapter
	{
		private readonly ILogger _logger;
		private readonly ISqlAdapter _adapter;

		public SearchAdapter(
			ILogger logger,
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