// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   WC.Services.Document
//   BaseDocumentManager.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.Document.Managers.v6
{
	using System;
	using System.Collections.Generic;
	using System.Data.SqlTypes;
	using System.IO;
	using System.Linq;
	using System.Web.Http.ModelBinding;
	using CareSource.WC.Entities.Common;
	using CareSource.WC.Entities.Documents;
	using CareSource.WC.OnBase.Core.ExtensionMethods;
	using CareSource.WC.Services.Document.Adapters.v6;
	using CareSource.WC.Services.Document.Models.v6;
	using Microsoft.Extensions.Logging;
	using Document = CareSource.WC.Services.Document.Models.v6.Document;
	using DocumentHeader = CareSource.WC.Services.Document.Models.v6.DocumentHeader;

	/// <summary>
	///    Represents the data used to define a the base document manager
	/// </summary>
	/// <seealso cref="IDocumentManager{DocumentHeader}" />
	public abstract class BaseDocumentManager : IDocumentManager
	{
		/// <summary>
		///    Initializes a new instance of the <see cref="DocumentManager" /> class.
		/// </summary>
		/// <param name="documentAdapter">The SQL adapter to pull information from OB.</param>
		/// <param name="onBaseAdapter">Wrapper of the OB API</param>
		/// <param name="logger"></param>
		protected BaseDocumentManager(
			IOnBaseSqlAdapter<DocumentHeader> documentAdapter,
			IOnBaseAdapter onBaseAdapter,
			ILogger logger)
		{
			DocumentAdapter = documentAdapter;
			OnBaseAdapter = onBaseAdapter;
			Logger = logger;
		}

		/// <summary>
		///    Gets the provider broker.
		/// </summary>
		protected IOnBaseSqlAdapter<DocumentHeader> DocumentAdapter { get; }

		protected IOnBaseAdapter OnBaseAdapter { get; }

		protected ILogger Logger { get; }

		public virtual long DocumentCount(IFilteredRequest request)
		{
			Logger.LogDebug("Starting Document Count");

			long total = DocumentAdapter.TotalRecords(request);

			return total;
		}

		public virtual (IDictionary<string, int>, long) DocumentTypeCounts(IFilteredRequest request)
		{
			Logger.LogDebug("Starting Document Type Counts");
			IDictionary<string, int> docTypes = DocumentAdapter.DocumentTypesCount(request);

			long total = docTypes.Sum(v => v.Value);

			return (docTypes, total);
		}

		public virtual Document Download(IDownloadRequest request)
		{
			OnBaseDocument document = OnBaseAdapter.GetDocument(request);

			string documentData = null;

			if (document.FileData != null)
			{
				// Return as byte array.
				using (MemoryStream streamReader = new MemoryStream())
				{
					document.FileData.CopyTo(streamReader);

					document.FileData.Close();
					document.FileData.Dispose();

					byte[] result = streamReader.ToArray();
					documentData = Convert.ToBase64String(result);
				}
			}

			return new Document
			{
				FileData = documentData,
				Filename = document.Filename,
				Id = document.Id,
				Name = document.Name,
				Type = document.Type,
				DocumentDate = document.DocumentDate,
				DisplayColumns = (Dictionary<string, string>)document.Keywords
			};
		}

		/// <summary>
		///    Searches the specified search identifier.
		/// </summary>
		/// <param name="request">The request data.</param>
		/// <returns></returns>
		public virtual ISearchResult<DocumentHeader> Search(ISearchRequest request)
		{
			Logger.LogDebug($"Starting {nameof(BaseDocumentManager)}.Search");

				SetDisplayColumns(request);
			SetFilters(request);

			ISearchResult<DocumentHeader> documents = DocumentAdapter.Search(request);

			return documents;
		}

		public virtual bool IsValid(
			ISearchRequest request,
			ModelStateDictionary modelState)
			=> IsValid(
				(IFilteredRequest)request,
				modelState);

		/// <summary>
		///    Validates the request.
		/// </summary>
		public virtual bool IsValid(
			IFilteredRequest request,
			ModelStateDictionary modelState)
		{
			DateTime? startDate = request.StartDate;
			DateTime? endDate = request.EndDate;

			request.DocumentTypes = request.DocumentTypes ?? new List<string>();
			request.Filters = request.Filters ?? new List<Filter>();

			if (!request.DocumentTypes.Any() &&
				!request.Filters.Any())
			{
				modelState.AddModelError(
					"Request",
					"No document types or filters were sent, please apply some filters so we can " +
					"safely search for documents");
			}

			if (startDate.HasValue)
			{
				if (startDate.Value < SqlDateTime.MinValue.Value)
				{
					modelState.AddModelError(
						"StartDate",
						$"Start must be greater than {SqlDateTime.MinValue}");
				}

				if (startDate.Value > SqlDateTime.MaxValue.Value)
				{
					modelState.AddModelError(
						"StartDate",
						$"Start must be less than {SqlDateTime.MaxValue}");
				}

				if (!endDate.HasValue)
				{
					modelState.AddModelError(
						"EndDate",
						"If Start Date is provided then End Date is also required");
				}
			}

			if (endDate.HasValue)
			{
				if (endDate.Value < SqlDateTime.MinValue.Value)
				{
					modelState.AddModelError(
						"EndDate",
						$"End must be greater than {SqlDateTime.MinValue}");
				}

				if (endDate.Value > SqlDateTime.MaxValue.Value)
				{
					modelState.AddModelError(
						"EndDate",
						$"End must be less than {SqlDateTime.MaxValue}");
				}

				if (!startDate.HasValue)
				{
					modelState.AddModelError(
						"StartDate",
						"If End Date is provided then Start Date is also required");
				}
			}

			if (request.Filters.Any(f => f.Name == "Member Suffix") &&
				request.Filters.All(f => f.Name != "Subscriber ID"))
			{
				modelState.AddModelError(
					"Member Suffix",
					"When searching by Member Suffix you must also search with Subscriber ID");
			}

			Logger.LogInformation(
				$"{(modelState.IsValid ? "Validated" : "Invalid")} Model State",
				new Dictionary<string, object>
				{
					{ "Model State", modelState }
				});

			return modelState.IsValid;
		}

		public virtual bool IsValid(
			IDownloadRequest request,
			ModelStateDictionary modelState)
		{
			Logger.LogInformation(
				$"{(modelState.IsValid ? "Validated" : "Invalid")} Model State",
				new Dictionary<string, object>
				{
					{ "Model State", modelState }
				});

			return modelState.IsValid;
		}

		protected virtual void SetDisplayColumns(ISearchRequest request)
		{
			request.DisplayColumns = request.DisplayColumns ?? new List<string>();

			switch (request.Sorting.ColumnName)
			{
				case "DocumentDate":
				case "DocumentType":
				case "DocumentName":
					break;
				default:
					if (!request.DisplayColumns.Contains(request.Sorting.ColumnName))
					{
						List<string> columns = request.DisplayColumns.ToList();
							columns.Add(request.Sorting.ColumnName);

							request.DisplayColumns = columns;
					}

					break;
			}

			List<string> newDisplayColumns = new List<string>();

			newDisplayColumns.AddRange(request.DisplayColumns);

			request.DisplayColumns = newDisplayColumns;
		}

		protected virtual void SetFilters(IFilteredRequest request)
		{
			if (request.Filters == null)
			{
				request.Filters = new List<Filter>();
			}

			List<Filter> filters = new List<Filter>();

			filters.AddRange(request.Filters);

			request.Filters = filters;
		}
	}
}