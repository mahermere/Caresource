// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   CareSource.WC.Services.Document.WC.Services.Document
//   DocumentManager.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.Document.Managers.v3
{
	using System;
	using System.Linq;
	using CareSource.WC.Entities.Common.Interfaces;
	using CareSource.WC.Entities.Documents.Interfaces;
	using CareSource.WC.Entities.Exceptions;
	using CareSource.WC.OnBase.Core.ExtensionMethods;
	using CareSource.WC.Services.Document.Adapters.v3;
	using CareSource.WC.Services.Document.Models.v3;

	/// <summary>
	/// Represents the data used to define a the base document manager
	/// </summary>
	/// <typeparam name="TDataModel">The type of the data model.</typeparam>
	/// <seealso cref="IDocumentManager{TDataModel}" />
	public abstract class BaseDocumentManager<TDataModel> : IDocumentManager<TDataModel>
	{
		/// <summary>
		///    Initializes a new instance of the <see cref="DocumentManager" /> class.
		/// </summary>
		/// <param name="documentAdapter">The provider broker.</param>
		protected BaseDocumentManager(
			ISearchDocumentAdapter<TDataModel> documentAdapter)
		{
			DocumentAdapter = documentAdapter;
		}

		/// <summary>
		///    Gets the provider broker.
		/// </summary>
		protected ISearchDocumentAdapter<TDataModel> DocumentAdapter { get; }

		/// <summary>
		///    Gets or sets the request data of the document manager{ t data model} class.
		/// </summary>
		protected SearchRequest RequestData { get; set; }

		/// <summary>
		///    Gets or sets the search identifier of the document manager{ t data model} class.
		/// </summary>
		protected string SearchId { get; set; }

		/// <summary>
		///    Searches the specified search identifier.
		/// </summary>
		/// <param name="requestData">The request data.</param>
		/// <returns></returns>
		/// <exception cref="NoDocumentsException">No documents found for Id: {SearchId}</exception>
		public virtual ISearchResults<TDataModel> Search(SearchRequest requestData)
		{
			RequestData = requestData;

			ValidateRequest();

			SetFilters();
			SetDisplayColumns();

			ISearchResults<TDataModel> documents = DocumentAdapter.SearchDocuments(
				RequestData);

			if (!documents.Results.Any())
			{
				throw new NoDocumentsException(SearchId);
			}

			return documents;
		}

		/// <summary>
		///    Searches the specified search identifier.
		/// </summary>
		/// <param name="searchId">The search identifier.</param>
		/// <param name="requestData">The request data.</param>
		/// <returns></returns>
		/// <exception cref="NoDocumentsException">No documents found for Id: {SearchId}</exception>
		public virtual ISearchResults<TDataModel> Search(
			string searchId,
			SearchRequest requestData)
		{
			SearchId = searchId.SafeTrim();
			RequestData = requestData;

			ValidateRequest();
			SetFilters();
			SetDisplayColumns();

			ISearchResults<TDataModel> documents = DocumentAdapter.SearchDocuments(RequestData);

			if (!documents.Results.Any())
			{
				throw new NoDocumentsException(SearchId);
			}
			return documents;
		}

		/// <summary>
		///    Sets the display columns.
		/// </summary>
		protected abstract void SetDisplayColumns();

		/// <summary>
		///    Sets the filters.
		/// </summary>
		protected abstract void SetFilters();

		/// <summary>
		///    Validates the request.
		/// </summary>
		/// <exception cref="Exception">
		///    If Start date is provided then End Date is also required
		///    or
		///    If End Date is provided then Start Date is also required
		///    or
		///    Start Date:{RequestData.StartDate.SafeTrim()} is not in a known format
		///    or
		///    End Date:{RequestData.StartDate.SafeTrim()} is not in a known format
		/// </exception>
		protected virtual void ValidateRequest()
		{
			string startDate = RequestData.StartDate.ToString().SafeTrim();
			string endDate = RequestData.EndDate.ToString().SafeTrim();

			if (!RequestData.StartDate.HasValue
				&& RequestData.EndDate.HasValue)
			{
				throw new Exception("If Start Date is provided then End Date is also required");
			}

			if (RequestData.StartDate.HasValue
				&& !RequestData.EndDate.HasValue)
			{
				throw new Exception("If End Date is provided then Start Date is also required");
			}
		}
	}
}