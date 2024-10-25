// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   Caresource.WC.Services.Document.WC.Services.Document
//   ProviderDocumentManager.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.Document.Dotnet8.Managers
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using CareSource.WC.Entities.Common;
	using CareSource.WC.Entities.Common.Interfaces;
	using CareSource.WC.Entities.Documents;
	using CareSource.WC.Entities.Documents.Interfaces;
	using CareSource.WC.Entities.Exceptions;
    using WC.Services.Document.Dotnet8.Adapters.Interfaces;
    using WC.Services.Document.Dotnet8.Managers.Interfaces;

    /// <summary>
    /// Represents the data used to define a the provider document manager
    /// </summary>
    public class ProviderDocumentManager :
		BaseDocumentManager<DocumentHeader>,
		IProviderDocumentManager<DocumentHeader>
	{
		/// <summary>
		///    Initializes a new instance of the <see cref="ProviderDocumentManager" /> class.
		/// </summary>
		/// <param name="documentAdapter">The provider broker.</param>
		public ProviderDocumentManager(ISearchDocumentAdapter<DocumentHeader> documentAdapter)
			: base(documentAdapter)
		{ }

		/// <summary>
		///    Searches documents by the privider TIN.
		/// </summary>
		/// <param name="searchId">The search identifier.</param>
		/// <param name="requestData">The request data.</param>
		/// <returns></returns>
		/// <exception cref="NoDocumentsException"></exception>
		public ISearchResults<DocumentHeader> SearchTin(
			string searchId,
			IListDocumentsRequest requestData)
		{
			SearchId = searchId.Trim();
			RequestData = requestData;

			ValidateTinRequest();

			SetTinFilters();
			SetTinDisplayColumns();

			ISearchResults<DocumentHeader> documents = DocumentAdapter.SearchDocuments(RequestData);

			if (!documents.Results.Any())
			{
				throw new NoDocumentsException(SearchId);
			}

			return documents;
		}

		/// <summary>
		///    Sets the display columns.
		/// </summary>
		protected override void SetDisplayColumns()
		{
			List<string> displayColumns = RequestData.DisplayColumns.ToList();

			if (!RequestData.DisplayColumns.Any(
				f => f.Equals(
					"Provider ID",
					StringComparison.InvariantCulture)))
			{
				displayColumns.Add("Provider ID");
			}

			RequestData.DisplayColumns = displayColumns;
		}

		/// <summary>
		///    Sets the filters.  Adds the Provider ID and Provider Facing filters
		/// </summary>
		protected override void SetFilters()
		{
			// create a new list for filters; it has one more than the count of passed in filters so
			// we can add [Provider ID] or [Provider TIN]
			List<Filter> filters = new List<Filter>(RequestData.Filters.Count() + 1)
			{
				new Filter(
					"Provider ID",
					SearchId)
			};

			SetFilters(filters);
		}

		/// <summary>
		///    Sets the filters, adds the provided filters and adds the provider facing filter.
		/// </summary>
		/// <param name="filters">The filters.</param>
		private void SetFilters(List<Filter> filters)
		{
			if (!filters.Any(f => f.Name.Equals(
				"Provider Facing",
				StringComparison.CurrentCulture)))
			{
				filters.Add(
					new Filter(
						"Provider Facing",
						null));
			}

			filters.AddRange(RequestData.Filters);

			RequestData.Filters = filters;
		}

		/// <summary>
		///    Sets the display columns.
		/// </summary>
		protected void SetTinDisplayColumns()
		{
			List<string> displayColumns = RequestData.DisplayColumns.ToList();

			if (!RequestData.DisplayColumns.Any(
				f => f.Equals(
					"Provider TIN",
					StringComparison.InvariantCulture)))
			{
				displayColumns.Add("Provider TIN");
			}

			RequestData.DisplayColumns = displayColumns;
		}

		/// <summary>
		///    Sets the filters.  Adds the Provider TIN and Provider Facing filters
		/// </summary>
		protected void SetTinFilters()
		{
			// create a new list for filters; it has one more than the count of passed in filters so
			// we can add [Provider ID] or [Provider TIN]
			List<Filter> filters = new List<Filter>(RequestData.Filters.Count() + 1)
			{
				new Filter(
					"Provider TIN",
					SearchId)
			};

			SetFilters(filters);
		}

		/// <summary>
		/// Validates the request.
		/// </summary>
		/// <exception cref="Exception">
		/// Provider ID must be 12 characters or less in length.
		/// </exception>
		protected override void ValidateRequest()
		{
			if (SearchId.Length > 12)
			{
				throw new Exception("Provider ID must be 12 characters or less in length.");
			}

			base.ValidateRequest();
		}


		/// <summary>
		/// Validates the tin request.
		/// </summary>
		/// <exception cref="Exception">
		/// Provider TIN must be 9 characters in length.
		/// </exception>
		private void ValidateTinRequest()
		{
			if (!SearchId.Length.Equals(9))
			{
				throw new Exception("Provider TIN must be 9 characters in length.");
			}

			base.ValidateRequest();
		}
	}
}