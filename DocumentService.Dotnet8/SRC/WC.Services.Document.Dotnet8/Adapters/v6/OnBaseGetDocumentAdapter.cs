// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   WC.Services.Document
//   OnBaseGetDocumentAdapter.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.Document.Dotnet8.Adapters.v6
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Text.RegularExpressions;
	using System.Threading.Tasks;
	using CareSource.WC.Entities.Common;
	using CareSource.WC.Entities.Exceptions;
	
	using Hyland.Unity;
	using Microsoft.Extensions.Logging;
    using WC.Services.Document.Dotnet8.Adapters.v6.Interfaces;
    using WC.Services.Document.Dotnet8.Models.v6;
    using WC.Services.Document.Dotnet8.Models.v6.Interfaces;
    using WC.Services.Document.Dotnet8.Repository;
    using Document = Hyland.Unity.Document;
	using InvalidDocumentTypeException = CareSource.WC.Entities.Exceptions.InvalidDocumentTypeException;
	using InvalidKeywordException = CareSource.WC.Entities.Exceptions.InvalidKeywordException;
   

    /// <summary>
    /// Represents the data used to define a the OnBase get document adapter
    /// </summary>
    public class OnBaseGetDocumentAdapter : IGetDocumentAdapter<OnBaseDocument>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="OnBaseGetDocumentAdapter" /> class.
		/// </summary>
		/// <param name="applicationConnectionAdapter">The application connection adapter.</param>
		public OnBaseGetDocumentAdapter(
            IRepository repo,
            log4net.ILog logger)
		{
			_repo = repo;
			_logger = logger;
		}

        private readonly IRepository _repo;
        private readonly log4net.ILog _logger;

		private Document getHylanDocument(long documentId)
		{
			Document document = _repo.Application.Core.GetDocumentByID(documentId, DocumentRetrievalOptions.LoadKeywords);
		//	Document document = _repo.Application.Core.GetDocumentByID(documentId);

			if (document == null)
			{
				throw new DocumentNotFoundException(documentId);
			}

			return document;
		}

		public OnBaseDocument GetDocument(
			long documentId,
			IEnumerable<string> keywords)
		{
			Document document = getHylanDocument(documentId);

			OnBaseDocument onBaseDocument = new OnBaseDocument
			{
				Id = document.ID,
				Name = document.Name,
				Filename = document.Name,
				Type = document.DocumentType.Name,
				DocumentDate = document.DocumentDate,
				Keywords = GetKeywordValues(document.KeywordRecords, keywords)
			};

			return onBaseDocument;
		}

		public OnBaseDocument GetDocument(long documentId)
		{
			Document document = getHylanDocument(documentId);

			PageData pageData = GetPageData(document);

			OnBaseDocument onBaseDocument = new OnBaseDocument
			{
				Id = document.ID,
				Name = document.Name,
				Filename = $"{document.Name}.{pageData.Extension}",
				Type = document.DocumentType.Name,
				DocumentDate = document.DocumentDate,
				FileData = pageData.Stream
			};

			return onBaseDocument;
		}

		/// <summary>
		///    Gets the document.
		/// </summary>
		/// <param name="documentId">The document identifier.</param>
		/// <param name="request">The request.</param>
		/// <returns></returns>
		/// <exception cref="NullReferenceException">
		///    The {request.GetType() .Name}
		///    or
		///    No document found for id:{documentId}
		/// </exception>
		public OnBaseDocument GetDocument(
			long documentId,
			IDownloadRequest request)
		{
			if (string.IsNullOrWhiteSpace(request.UserId))
			{
				throw new NullReferenceException($"The {request.GetType().Name}.UserId is Required");
			}

			Document document = getHylanDocument(documentId);

			_repo.Application.Core.LogManagement.CreateDocumentHistoryItem(
				document, $"retrieved document by 'GetDocument({documentId})'");

			PageData pageData = GetPageData(document);

			OnBaseDocument onBaseDocument = new OnBaseDocument
			{
				Id = document.ID,
				Name = document.Name,
				Filename = $"{document.Name}.{pageData.Extension}",
				Type = document.DocumentType.Name,
				FileData = pageData.Stream,
				DocumentDate = document.DocumentDate,
				Keywords = GetKeywordValues(document.KeywordRecords, request.DisplayColumns)
			};

			return onBaseDocument;
		}

		private IDictionary<string, string> GetKeywordValues(
			KeywordRecordList keywords,
			IEnumerable<string> requestDisplayColumns)
			=> (
				from keywordName in requestDisplayColumns
				from keywordRecord in keywords
				let value =
					keywordRecord.Keywords
						.FirstOrDefault(
							item => item.KeywordType.Name.Equals(
								keywordName,
								StringComparison.InvariantCulture))
						?.Value.ToString() ??
					string.Empty
				select new KeyValuePair<string, string>(
					keywordName,
					value)).ToDictionary(
				di => di.Key,
				di => di.Value);

		private PageData GetPageData(Document document)
		{
			DefaultDataProvider provider = _repo.Application.Core.Retrieval.Default;

			PageData pageData = provider.GetDocument(document.DefaultRenditionOfLatestRevision);

			_logger.Info(pageData.Extension);

			return pageData;
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

			foreach (DocumentType dt in _repo.Application.Core.DocumentTypes
				.Where(
					doctype =>
						!documentTypes.Any()
						|| doctype.Name
						== documentTypes
							.FirstOrDefault(
								d => d.Equals(
									doctype.Name,
									StringComparison.InvariantCulture))))
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

				if (string.IsNullOrWhiteSpace(filter.Value)
					|| filter.IncludeNull)
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
						$"The value [{filter.Value}] is not in the correct format for "
						+ $"the '{filter.Name}' keyword.");
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
				_repo.Application.Core.KeywordTypes
					.FirstOrDefault(krt => krt.Name.Equals(filterName, StringComparison.InvariantCulture));

			if (keyword == null)
			{
				throw new InvalidKeywordException(filterName);
			}

			return keyword;
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
				if (!_repo.Application.Core.DocumentTypes
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
	}
}