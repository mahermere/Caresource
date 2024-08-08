// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   WC.Services.Document
//   OnBaseDocumentAdapter.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.Document.Adapters
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using CareSource.WC.Entities.Common;
	using CareSource.WC.Entities.Common.Interfaces;
	using CareSource.WC.Entities.Documents;
	using CareSource.WC.Entities.Documents.Interfaces;
	using CareSource.WC.Entities.Exceptions;
	using CareSource.WC.Entities.Requests.Base.Interfaces;
	using CareSource.WC.OnBase.Core.Connection.Interfaces;
	using Microsoft.Extensions.Logging;
	using CareSource.WC.OnBase.Core.ExtensionMethods;
	using Hyland.Unity;

	/// <summary>
	///    Represents the data used to define a the OnBase document adapter
	/// </summary>
	public class OnBaseDocumentAdapter : ISearchDocumentAdapter<DocumentHeader>
	{
		private readonly IApplicationConnectionAdapter<Application> _hylandApplication;
		private readonly ILogger _logger;

		/// <summary>
		///    Initializes a new instance of the <see cref="OnBaseDocumentAdapter" /> class.
		/// </summary>
		/// <param name="applicationConnectionAdapter">The application connection adapter.</param>
		/// <param name="logger"></param>
		public OnBaseDocumentAdapter(
			IApplicationConnectionAdapter<Application> applicationConnectionAdapter,
			ILogger logger)
		{
			_logger = logger;
			_hylandApplication = applicationConnectionAdapter;
		}

		/// <summary>
		///    Searches the documents.
		/// </summary>
		/// <param name="request">The request.</param>
		/// <returns></returns>
		public ISearchResults<DocumentHeader> SearchDocuments(
			IListDocumentsRequest request)
		{
			VerifyDocumentTypes(request.DocumentTypes);

			// Call on base API/Database
			DocumentQuery documentQuery = _hylandApplication.Application.Core.CreateDocumentQuery();

			documentQuery.RetrievalOptions = DocumentRetrievalOptions.None;

			AddKeywords(
				request.Filters,
				documentQuery);

			VerifyDisplayColumns(request.DisplayColumns);

			AddDocumentTypes(
				request.DocumentTypes,
				request.Filters,
				documentQuery);

			documentQuery.AddSort(
				DocumentQuery.SortAttribute.DocumentDate,
				false);

			if (!request.StartDate.IsNullOrWhiteSpace())
			{
				documentQuery.AddDateRange(
					DateTime.Parse(request.StartDate),
					DateTime.Parse(request.EndDate));
			}

			if (request.Sorting != null)
			{
				AddSortColumn(
					request.Sorting.ColumnName,
					documentQuery);
			}

			QueryResult queryResults = documentQuery.ExecuteQueryResults(10000);
			List<QueryResultItem> items;

			if (request.Filters.ToList()
				.Any(kw => kw.Value != null && kw.IncludeNull))
			{
				items =
				(
					from keyword in request.Filters.Where(kw => kw.Value != null && kw.IncludeNull)
					from qri in queryResults.QueryResultItems
					let value = GetKeyWordValue(
						qri,
						keyword.Name,
						null)
					where value == null ||
					      value.ToString()
						      .Equals(
							      keyword.Value,
							      StringComparison.InvariantCultureIgnoreCase)
					select qri).ToList();
			}
			else
			{
				items = queryResults.QueryResultItems.ToList();
			}

			items = items.GroupBy(doc => doc.Document.ID)
				.Select(doc => doc.First())
				.ToList();

			items = SortItems(
					request,
					items)
				.ToList();

			return new SearchResults<DocumentHeader>
			{
				TotalRecordCount = items.Count,
				Results = items
					.Skip((request.Paging.PageNumber - 1) * request.Paging.PageSize)
					.Take(request.Paging.PageSize)
					.Select(
						item => new DocumentHeader
						{
							DocumentName = item.Document.Name,
							DocumentId = item.Document.ID,
							DocumentDate = item.Document.DocumentDate,
							DocumentType = item.Document.DocumentType.Name,
							DisplayColumns = request.DisplayColumns
								.ToDictionary(
									dc => dc,
									dc => GetKeyWordValue(
										item,
										dc,
										string.Empty))
						})
					.AsEnumerable()
			};
		}

		/// <summary>
		///    Adds the display columns.
		/// </summary>
		/// <param name="displayColumns">The display columns.</param>
		private void VerifyDisplayColumns(IEnumerable<string> displayColumns)
		{
			foreach (string columnName in displayColumns)
			{
				GetKeywordType(columnName);
			}
		}

		/// <summary>
		///    Adds the sort column.
		/// </summary>
		/// <param name="sortColumn">The sort column.</param>
		/// <param name="documentQuery">The document query.</param>
		private void AddSortColumn(
			string sortColumn,
			DocumentQuery documentQuery)
		{
			if (typeof(DocumentHeader)
				.GetProperties()
				.Select(pi => pi.Name)
				.Contains(sortColumn))
			{
				return;
			}

			KeywordType column = GetKeywordType(sortColumn);

			documentQuery.AddDisplayColumn(column);
		}

		/// <summary>
		///    Adds the document types.
		/// </summary>
		/// <param name="documentTypes">The document types.</param>
		/// <param name="filters">The filters.</param>
		/// <param name="documentQuery">The document query.</param>
		/// <exception cref="DocTypeKeywordConflictException"></exception>
		private void AddDocumentTypes(
			IEnumerable<string> documentTypes,
			IEnumerable<Filter> filters,
			DocumentQuery documentQuery)
		{
			int totalDocTypes = 0;


			foreach (DocumentType dt in _hylandApplication.Application.Core.DocumentTypes
				.Where(
					doctype =>
						!documentTypes.Any() ||
						doctype.Name ==
						documentTypes
							.FirstOrDefault(
								d => d.Equals(
									doctype.Name,
									StringComparison.InvariantCultureIgnoreCase))))
			{
				bool useDoc = true;
				foreach (Filter filter in filters)
				{
					useDoc = dt.KeywordRecordTypes.Any(
						krt => krt.KeywordTypes.Any(
							kt => kt.Name.Equals(
								filter.Name,
								StringComparison.InvariantCulture)));

					if (!useDoc)
					{
						break;
					}
				}

				if (!useDoc)
				{
					continue;
				}

				totalDocTypes++;
				documentQuery.AddDocumentType(dt);
			}

			if (totalDocTypes == 0)
			{
				throw new DocTypeKeywordConflictException();
			}
		}

		/// <summary>
		///    Adds the keywords to the query.
		/// </summary>
		/// <param name="filters">The filters.</param>
		/// <param name="documentQuery">The document query.</param>
		/// <exception cref="NoKeywordsException"></exception>
		/// <exception cref="InvalidKeywordException"></exception>
		private void AddKeywords(
			IEnumerable<Filter> filters,
			DocumentQuery documentQuery)
		{
			if (!filters.Any())
			{
				throw new NoKeywordsException();
			}

			foreach (Filter filter in filters)
			{
				KeywordType keyword = GetKeywordType(filter.Name);

				if (filter.Value.IsNullOrWhiteSpace() ||
				    filter.IncludeNull)
				{
					continue;
				}

				try
				{
					switch (keyword.DataType)
					{
						case KeywordDataType.AlphaNumeric:
						case KeywordDataType.Undefined:
							documentQuery.AddKeyword(
								keyword.ID,
								filter.Value);
							break;

						case KeywordDataType.FloatingPoint:
							documentQuery.AddKeyword(
								keyword.ID,
								Convert.ToDouble(filter.Value));
							break;

						case KeywordDataType.Currency:
						case KeywordDataType.SpecificCurrency:
							documentQuery.AddKeyword(
								keyword.ID,
								Convert.ToDecimal(filter.Value));
							break;

						case KeywordDataType.DateTime:
						case KeywordDataType.Date:
							documentQuery.AddKeyword(
								keyword.ID,
								Convert.ToDateTime(filter.Value));
							break;

						case KeywordDataType.Numeric20:
							documentQuery.AddKeyword(
								keyword.ID,
								Convert.ToDecimal(filter.Value));
							break;

						case KeywordDataType.Numeric9:
							documentQuery.AddKeyword(
								keyword.ID,
								Convert.ToInt64(filter.Value));
							break;

						default:
							documentQuery.AddKeyword(
								keyword.ID,
								filter.Value);
							break;
					}
				}
				catch
				{
					throw new Exception(
						$"The value [{filter.Value}] is not in the correct format for " +
						$"the '{filter.Name}' keyword.");
				}
			}
		}

		/// <summary>
		///    Gets the type of the keyword.
		/// </summary>
		/// <param name="filterName">Name of the filter.</param>
		/// <returns></returns>
		/// <exception cref="InvalidKeywordException"></exception>
		private KeywordType GetKeywordType(
			string filterName)
		{
			KeywordType keyword =
				_hylandApplication.Application.Core.KeywordTypes
					.FirstOrDefault(
						krt => krt.Name.Equals(
							filterName,
							StringComparison.InvariantCulture));

			if (keyword == null)
			{
				throw new InvalidKeywordException(filterName);
			}

			return keyword;
		}

		/// <summary>
		///    Gets the key word value.
		/// </summary>
		/// <param name="item">The item.</param>
		/// <param name="columnName">Name of the column.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <returns></returns>
		private object GetKeyWordValue(
			QueryResultItem item,
			string columnName,
			string defaultValue)
		{
			Keyword column = null;

			foreach (KeywordRecord kr in item.Document.KeywordRecords)
			{
				foreach (Keyword k in kr.Keywords)
				{
					if (k.KeywordType.Name.Equals(
						columnName,
						StringComparison.InvariantCulture))
					{
						column = k;

						break;
					}
				}

				if (column != null) continue;
			}

			if (column == null ||
			    column.IsBlank)
			{
				return defaultValue;
			}

			switch (column.KeywordType.DataType)
			{
				case KeywordDataType.AlphaNumeric:
					return column.AlphaNumericValue;
				case KeywordDataType.Currency:
					return column.CurrencyValue;
				case KeywordDataType.Date:
				case KeywordDataType.DateTime:
					return column.DateTimeValue;
				case KeywordDataType.FloatingPoint:
					return column.FloatingPointValue;
				case KeywordDataType.Numeric20:
					return column.Numeric20Value;
				case KeywordDataType.Numeric9:
					return column.Numeric9Value;
				case KeywordDataType.SpecificCurrency:
					return column.CurrencyValue;
				case KeywordDataType.Undefined:
					return column.AlphaNumericValue;
				default:
					return column.AlphaNumericValue;
			}
		}

		/// <summary>
		///    Verifies the document types.
		/// </summary>
		/// <param name="documentTypes">The document types.</param>
		/// <exception cref="InvalidDocumentTypeException"></exception>
		private void VerifyDocumentTypes(
			IEnumerable<string> documentTypes)
		{
			foreach (string docType in documentTypes)
			{
				if (!_hylandApplication.Application.Core.DocumentTypes
					.FindAll(
						dt => dt.Name.Equals(
							docType,
							StringComparison.InvariantCulture))
					.Any())
				{
					throw new InvalidDocumentTypeException(docType);
				}
			}
		}

		private static IEnumerable<QueryResultItem> SortItems(
			ISortingRequest request,
			IEnumerable<QueryResultItem> items)
		{
			if (request.Sorting == null)
			{
				return items;
			}

			if (request.Sorting.SortAscending)
			{
				switch (request.Sorting.ColumnName)
				{
					case "DocumentName":
						return items
							.OrderBy(e => e.Document.Name)
							.ThenByDescending(e => e.Document.DocumentDate);
					case "DocumentType":
						return items
							.OrderBy(e => e.Document.DocumentType.Name)
							.ThenByDescending(e => e.Document.DocumentDate);
					case "DocumentDate":
						return items
							.OrderBy(e => e.Document.DocumentDate);
					default:
						return new List<QueryResultItem>(
							items
								.OrderBy(
									e => e.DisplayColumns
										.ElementAt(0)
										.IsBlank
										? string.Empty
										: e.DisplayColumns.ElementAt(0)
											.AlphaNumericValue)
								.ThenByDescending(e => e.Document.DocumentDate));
				}
			}

			switch (request.Sorting.ColumnName)
			{
				case "DocumentName":
					return items
						.OrderByDescending(e => e.Document.Name)
						.ThenByDescending(e => e.Document.DocumentDate);
				case "DocumentType":
					return items
						.OrderByDescending(e => e.Document.DocumentType.Name)
						.ThenByDescending(e => e.Document.DocumentDate);
				case "DocumentDate":
					// This just returns the items because the default sort is document date descending
					return items;
				default:
					return items
						.OrderByDescending(
							e => e.DisplayColumns
								.ElementAt(0)
								.IsBlank
								? string.Empty
								: e.DisplayColumns.ElementAt(0)
									.AlphaNumericValue)
						.ThenByDescending(e => e.Document.DocumentDate);
			}
		}
	}
}