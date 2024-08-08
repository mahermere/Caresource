// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2021. All rights reserved.
// 
//   ImportProcessor
//   DocumentServiceRequest.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace ImportProcessor.Models.v1
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using CareSource.WC.Entities.Common;
	using CareSource.WC.Entities.Requests.Base;

	/// <summary>
	///    Data and functions describing a CareSource.WC.Services.Document.Models.v5.SearchRequest object.
	/// </summary>
	/// <remarks>
	///    Basic request to search for Documents
	/// </remarks>
	/// <seealso cref="CareSource.WC.Services.Document.Models.v5.ISearchRequest" />
	public class SearchRequest
	{
		/// <summary>
		///    Gets or sets the Request Correlation Unique identifier
		/// </summary>
		/// <remarks>
		///    Defaults to a new guid if not provided
		/// </remarks>
		public Guid CorrelationGuid { get; set; } = Guid.NewGuid();


		/// <summary>
		///    Gets or sets the Request Request Date Time
		/// </summary>
		public DateTime RequestDateTime { get; set; } = DateTime.Now;


		/// <summary>
		///    Gets or sets the Request Source Application
		/// </summary>
		[Required]
		public string SourceApplication { get; set; }


		/// <summary>
		///    Gets or sets the Request User Identifier
		/// </summary>
		[Required]
		public string UserId { get; set; }

		/// <summary>
		///    Gets or sets the Search Request Display Columns
		/// </summary>
		public IEnumerable<string> DisplayColumns { get; set; } = new List<string>();

		/// <summary>
		///    Gets or sets the Search Request Document Types
		/// </summary>
		public IEnumerable<string> DocumentTypes { get; set; } = new List<string>();

		/// <summary>
		///    Gets or sets the Search Request Filters
		/// </summary>
		public IEnumerable<Filter> Filters { get; set; } = new List<Filter>();

		/// <summary>
		///    Gets or sets the Search Request Start Date
		/// </summary>
		public DateTime? StartDate { get; set; }

		/// <summary>
		///    Gets or sets the Search Request End Date
		/// </summary>
		public DateTime? EndDate { get; set; }

		/// <summary>
		///    Gets or sets the Search Request Paging
		/// </summary>
		/// <remarks>
		///    Determines the paging for the response, defaults Page number = 1, page size = 100
		/// </remarks>
		public Paging Paging { get; set; } = new Paging
		{
			PageNumber = 1,
			PageSize = 100
		};

		/// <summary>
		///    Gets or sets the Search Request Sort
		/// </summary>
		/// <remarks>
		///    The Sort of the search, can be one column and defaults to DocumentDate Descending,
		///    most recent on top
		/// </remarks>
		public Sort Sorting { get; set; } = new Sort
		{
			ColumnName = "DocumentDate",
			SortAscending = false
		};
	}


	public class SearchResult
	{
		public int TotalItems { get; set; }
		public string CorrelationGuid { get; set; }
		public int ErrorCode { get; set; }
		public string Message { get; set; }
		public Responsedata[] ResponseData { get; set; }
		public string Status { get; set; }
	}

	public class Responsedata
	{
		public IDictionary<string, string> DisplayColumns { get; set; }
		public DateTime DocumentDate { get; set; }
		public long DocumentId { get; set; }
		public string DocumentName { get; set; }
		public string DocumentType { get; set; }
	}
}