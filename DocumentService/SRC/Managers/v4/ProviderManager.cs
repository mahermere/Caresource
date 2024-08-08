// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   WC.Services.Document
//   ProviderManager.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.Document.Managers.v4
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using CareSource.WC.Entities.Common;
	using CareSource.WC.Entities.Common.Interfaces;
	using CareSource.WC.Entities.Documents;
	using Microsoft.Extensions.Logging;
	using CareSource.WC.OnBase.Core.ExtensionMethods;
	using CareSource.WC.Services.Document.Adapters.v4;
	using CareSource.WC.Services.Document.Models.v4;

	/// <summary>
	///    Represents the data used to define a the document manager
	/// </summary>
	public class ProviderManager : IProviderManager
	{
		private readonly ISearchDocumentAdapter<DocumentHeader> _adapter;
		private ILogger _logger;

		public ProviderManager(
			ISearchDocumentAdapter<DocumentHeader> adapter,
			ILogger logger)
		{
			_adapter = adapter;
			_logger = logger;
		}

		public IDictionary<string, int> GetDocumentTypesCount(
			string providerId,
			DocumentTypesCountRequest request)
		{
			_logger.LogDebug("Starting Provider Get Document Type Count");
			request.Filters = SetFilters(
				providerId,
				request.Filters);

			return _adapter.GetDocumentTypesCount(request);
		}

		public ISearchResults<DocumentHeader> Search(
			string providerId,
			ListDocumentsRequest request)
		{
			_logger.LogDebug("Starting Provider Search");
			request.Filters = SetFilters(
				providerId,
				request.Filters);

			ISearchResults<DocumentHeader> results = _adapter.SearchDocuments(request);

			//results.TotalRecordCount = GetDocumentTypesCount(
			//		providerId,
			//		new DocumentTypesCountRequest()
			//		{
			//			Filters = request.Filters.Where(e => e.Name != "Provider ID"),
			//			DocumentTypes = request.DocumentTypes,
			//			CorrelationGuid = request.CorrelationGuid,
			//			EndDate = request.EndDate,
			//			RequestDateTime = request.RequestDateTime,
			//			SourceApplication = request.SourceApplication,
			//			StartDate = request.StartDate,
			//			UserId = request.UserId
			//		})
			//	.Count;

			return results;
		}

		private static IEnumerable<Filter> SetFilters(
			string providerId,
			IEnumerable<Filter> filters)
		{
			if (filters == null)
			{
				filters = new List<Filter>();
			}

			List<Filter> newFilters = new List<Filter>(filters.Count() + 2)
			{
				new Filter(
					"Provider ID",
					providerId)
			};

			if (!filters.Any(
				f => f.Name.Equals(
					"Provider Facing",
					StringComparison.InvariantCulture)))
			{
				newFilters.Add(
					new Filter(
						"Provider Facing",
						null));
			}

			newFilters.AddRange(filters);

			return newFilters;
		}
	}
}