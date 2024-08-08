// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   WC.Services.Document
//   OnBaseGetDocumentAdapter.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.Document.Adapters
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Text.RegularExpressions;
	using System.Threading.Tasks;
	using CareSource.WC.Entities.Common;
	using CareSource.WC.Entities.Common.Interfaces;
	using CareSource.WC.Entities.Documents;
	using CareSource.WC.Entities.Documents.Interfaces;
	using CareSource.WC.Entities.Exceptions;
	using CareSource.WC.OnBase.Core.Connection.Interfaces;
	using CareSource.WC.OnBase.Core.ExtensionMethods;
	using CareSource.WC.Services.Document.Models;
	using Hyland.Unity;
	using Document = Hyland.Unity.Document;

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
			IApplicationConnectionAdapter<Application> applicationConnectionAdapter)
			=> _hylandApplication = applicationConnectionAdapter;

		private const string ImageFileFormat = "Image File Format";
		private const string Pdf = "PDF";
		private const string MsWordDocument = "MS Word Document";
		private const string MsExcelSpreadsheet = "MS Excel Spreadsheet";
		private const string TextReportFormat = "Text Report Format";
		private const string WavAudioFile = "WAV Audio File";
		private const string ZipCompressionArchive = "Zip Compression Archive";
		private const string Html = "HTML";
		private const string MsPowerPoint = "MS Power Point";
		private const string RichTextFormat = "Rich Text Format";

		private readonly Dictionary<string, string> _fileTypes = new Dictionary<string, string>
		{
			{"tif", ImageFileFormat},
			{"tiff", ImageFileFormat},
			{"jpg", ImageFileFormat},
			{"jpe", ImageFileFormat},
			{"jpeg", ImageFileFormat},
			{"gif", ImageFileFormat},
			{"png", ImageFileFormat},
			{"???", ImageFileFormat},
			{"pdf", Pdf},
			{"doc", MsWordDocument},
			{"docx", MsWordDocument},
			{"xls", MsExcelSpreadsheet},
			{"xlsx", MsExcelSpreadsheet},
			{"csv", MsExcelSpreadsheet},
			{"txt", TextReportFormat},
			{"wav", WavAudioFile},
			{"zip", ZipCompressionArchive},
			{"html", Html},
			{"htm", Html},
			{"ppt", MsPowerPoint},
			{"pptx", MsPowerPoint},
			{"rtf", RichTextFormat}
		};

		private readonly IApplicationConnectionAdapter<Application> _hylandApplication;

		private Document GetDocument(long documentId)
		{
			Document document = _hylandApplication.Application.Core.GetDocumentByID(
				documentId,
				DocumentRetrievalOptions.LoadKeywords);

			if (document == null)
			{
				throw new NullReferenceException($"No document found for id:{documentId}.");
			}

			return document;
		}

		public OnBaseDocument GetDocument(
			long documentId,
			IEnumerable<string> keywords)
		{
			Document document = GetDocument(documentId);

			OnBaseDocument onBaseDocument = new OnBaseDocument
			{
				Id = document.ID,
				Name = document.Name,
				Filename = document.Name,
				Type = document.DocumentType.Name,
				Keywords = GetKeywordValues(document.KeywordRecords, keywords)
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
			GetDocumentRequest request)
		{
			if (request.UserId.IsNullOrWhiteSpace())
			{
				throw new NullReferenceException($"The {request.GetType().Name}.UserId is Required");
			}

			Document document = GetDocument(documentId);

			_hylandApplication.Application.Core.LogManagement.CreateDocumentHistoryItem(
				document,
				$"{request.SourceApplication} user {request.UserId} "
				+ $"retrieved document by 'GetDocument({documentId})'");

			PageData pageData = GetPageData(document);

			OnBaseDocument onBaseDocument = new OnBaseDocument
			{
				Id = document.ID,
				Name = document.Name,
				Filename = $"{document.Name}.{pageData.Extension}",
				Type = document.DocumentType.Name,
				FileData = pageData.Stream,
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
			string ext = document.DefaultRenditionOfLatestRevision.FileType.Extension;
			if (_fileTypes.ContainsKey(ext))
			{
				if (_fileTypes.First(
						ft => ft.Key.Equals(
							ext,
							StringComparison.InvariantCulture))
					.Value.Equals(
						ImageFileFormat,
						StringComparison.InvariantCulture)
					|| document.DefaultRenditionOfLatestRevision.FileType.Name.Equals(
							ImageFileFormat,
							StringComparison.InvariantCulture))
				{

					ImageDataProvider imageProvider =
						_hylandApplication.Application.Core.Retrieval.Image;

					return imageProvider.GetDocument(document.DefaultRenditionOfLatestRevision);
				}
			}

			NativeDataProvider provider = _hylandApplication.Application.Core.Retrieval.Native;

			return provider.GetDocument(document.DefaultRenditionOfLatestRevision);
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

				if (filter.Value.IsNullOrWhiteSpace()
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
				_hylandApplication.Application.Core.KeywordTypes
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

		/// <summary>
		///    Searches the documents.
		/// </summary>
		/// <param name="request">The request.</param>
		/// <returns></returns>
		public async Task<ISearchResults<OnBaseDocument>> SearchDocumentsAsync(
			IListDocumentsRequest request)
		{
			VerifyDocumentTypes(request.DocumentTypes);


			// Call on base API/Database
			DocumentQuery documentQuery = _hylandApplication.Application.Core.CreateDocumentQuery();
			documentQuery.RetrievalOptions = DocumentRetrievalOptions.None;

			AddKeywords(
				request.Filters,
				documentQuery);

			AddDocumentTypes(
				request.DocumentTypes,
				request.Filters,
				documentQuery);

			if (!request.StartDate.IsNullOrWhiteSpace())
			{
				documentQuery.AddDateRange(
					DateTime.Parse(request.StartDate),
					DateTime.Parse(request.EndDate));
			}

			documentQuery.AddSort(
				DocumentQuery.SortAttribute.DocumentDate,
				false);

			DocumentList queryResults =
				documentQuery.Execute(10000);

			IEnumerable<Document> items;

			if (request.Filters.ToList()
				.Any(kw => kw.Value != null && kw.IncludeNull))
			{
				items =
				(
					from filter in request.Filters.Where(kw => kw.Value != null && kw.IncludeNull)
					from document in queryResults
					from keywordRecord in document.KeywordRecords
					let value = keywordRecord.Keywords
						.FirstOrDefault(
							item => item.KeywordType.Name.Equals(
								filter.Name,
								StringComparison.InvariantCulture))
						?.AlphaNumericValue
					where value == null ||
						value.ToString()
							.Equals(
								filter.Value,
								StringComparison.InvariantCultureIgnoreCase)
					select document).ToList();
			}
			else
			{
				items = queryResults;
			}

			items = items.GroupBy(doc => doc.ID)
				.Select(doc => doc.First());

			SearchResults<OnBaseDocument> result = new SearchResults<OnBaseDocument>
			{
				TotalRecordCount = items.Count()
			};

			items = items
				.Skip((request.Paging.PageNumber - 1) * request.Paging.PageSize)
				.Take(request.Paging.PageSize);

			IEnumerable<Task<OnBaseDocument>> docTasks = items
				.Select(async item =>
				{
					string fileName = null;
					Stream stream = null;

					PageData pageData = GetPageData(item);

					if (pageData != null)
					{
						fileName = Regex.Replace($"{item.Name}.{pageData.Extension}"
							, @"(<|>|:|""|\/|\\|s|\||\?|\*)", " ");

						stream = pageData.Stream;
					}

					return new OnBaseDocument
					{
						Filename = fileName,
						FileData = stream,
						Id = item.ID,
						Name = item.Name,
						Type = item.DocumentType.Name,
						Keywords = GetKeywordValues(
							item.KeywordRecords,
							request.DisplayColumns)
					};
				});

			OnBaseDocument[] documentResults = await Task.WhenAll(docTasks).ConfigureAwait(false);
			result.Results = documentResults;

			return result;
		}

	}
}