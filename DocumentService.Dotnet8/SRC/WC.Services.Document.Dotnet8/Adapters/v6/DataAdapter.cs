// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2023. All rights reserved.
// 
//   WC.Services.Document
//   DataAdapter.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.Document.Dotnet8.Adapters.v6
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Linq;
	using System.Runtime.InteropServices;
	using System.Text;
	using CareSource.WC.Entities.Common;
	using CareSource.WC.Entities.Exceptions;
	using Hyland.Unity;
	using Microsoft.Extensions.Logging;
    using WC.Services.Document.Dotnet8.Adapters.v6.Interfaces;
    using WC.Services.Document.Dotnet8.App_code;
    using WC.Services.Document.Dotnet8.Models.v6.Interfaces;
    using WC.Services.Document.Dotnet8.Models.v6.Requests;

    /// <summary>
    ///    Data Adapter to use SQL to Query OnBase for documents
    /// </summary>
    public class DataAdapter
	{
		private readonly IOnBaseAdapter _onBaseAdapter;
		private readonly log4net.ILog _logger;
		private static readonly string NewLine = Environment.NewLine;

		public DataAdapter(
			IOnBaseAdapter onBaseAdapter,
			log4net.ILog logger)
		{
			_onBaseAdapter = onBaseAdapter;
			_logger = logger;
		}
		public IEnumerable<Document> SearchDocuments(SearchRequest request)
		{
			throw new NotImplementedException();
		}

		private string CreateSql(IFilteredRequest request)
		{
			string where = BuildWhere(request);

			return where;
		}

		private string BuildWhere(IFilteredRequest request)
		{
			string where = 
				$"	WHERE{NewLine}" +
				// Do nor return any Deleted items
				$"		Document.Status != '16'{NewLine}" +
				$"		{BuildDocTypeWhere(request.DocumentTypes)}" +
				$"		{BuildFilterWhere(request.Filters)}" +
				NewLine;

			return where;
		}

		private string BuildFilterFrom(IEnumerable<Filter> filters)
		{
			//return"";
			IEnumerable<KeywordType> keywords = _onBaseAdapter.GetKeywordTypes(filters.Select(f => f.Name));

			StringBuilder filterBuilder = new StringBuilder();
			foreach (Filter f in filters)
			{
				KeywordType kt = keywords.FirstOrDefault(
					kt => kt.Name.Equals(
						f.Name,
						StringComparison.CurrentCultureIgnoreCase));

				string LeftInner = f.IncludeNull
					? "LEFT"
					: "INNER";

				filterBuilder.AppendLine($"		{LeftInner} JOIN KeyItem{kt.ID} [{kt.Name}]");
			}
			return "";
		}

		public string BuildFilterWhere(IEnumerable<Filter> requestFilters)
		{
			IEnumerable<KeywordType> keywords =
				_onBaseAdapter.GetKeywordTypes(requestFilters.Select(f => f.Name));
			
			IEnumerable<Filter> filters = requestFilters
				.Where(f =>
					!f.Name.Equals("Member Facing")
					&& !f.Name.Equals("Provider Facing") );

			//Get the Distinct Filters
			IEnumerable<string> distinctFilterNames = filters
				.Select(f => f.Name)
				.Distinct();

			if (!distinctFilterNames.SafeAny())
			{
				return string.Empty;
			}

			StringBuilder filterBuilder = new StringBuilder();

			foreach (string filter in distinctFilterNames)
			{
				KeywordType kt = keywords.FirstOrDefault(
					kt => kt.Name.Equals(
						filter,
						StringComparison.CurrentCultureIgnoreCase));

				bool includeNull = filters.Any(f =>
					f.Name.Equals(filter)
					&& f.IncludeNull);

				string columnName = _onBaseAdapter.GetKeywordValueDbColumnName(kt);
				string values = string.Join(
					", ",
					filters.Select(f => "'" + f.Value.Trim() + "'"));

				string column = $"[{kt.Name}].{columnName}";

				filterBuilder.AppendLine($"		AND {column} IN({values})");

				if (includeNull)
				{
					filterBuilder.AppendLine($"			OR {column} IS NULL");
				}
			}

			return filterBuilder.ToString();
		}

		private string BuildDocTypeWhere(IEnumerable<string> requestDocTypes)
			=> $"AND DocumentType IN ({GetDocTypeIds(requestDocTypes)}){NewLine}";

		private string GetDocTypeIds(IEnumerable<string> requestDocTypes)
		{
			IEnumerable<DocumentType> docTypes = _onBaseAdapter.GetDocumentTypes(requestDocTypes);

			IEnumerable<long> ids;

			// If you request specific doc types only include those
			if (requestDocTypes.SafeAny())
			{
				ids = docTypes
					.Where(dts => requestDocTypes.Contains(dts.Name))
					.Select(dts => dts.ID);
			}
			else // Include all document types the current user has access to.
			{
				ids = docTypes.Select(dts => dts.ID);
			}

			string docTypeIds = string.Join(
				", ",
				ids);

			return docTypeIds;
		}
	}
}