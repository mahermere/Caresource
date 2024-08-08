// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   WC.Services.Document
//   MemberManager.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.Document.Managers.v4
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using CareSource.WC.Entities.Common;
	using CareSource.WC.Entities.Documents;
	using Microsoft.Extensions.Logging;
	using CareSource.WC.Services.Document.Adapters.v4;
	using CareSource.WC.Services.Document.Models.v4;

	/// <summary>
	///    Represents the data used to define a the document manager
	/// </summary>
	public class MemberManager : IMemberManager
	{
		private readonly ISearchDocumentAdapter<DocumentHeader> _adapter;
		private ILogger _logger;

		public MemberManager(
			ISearchDocumentAdapter<DocumentHeader> adapter,
			ILogger logger)
		{
			_adapter = adapter;
			_logger = logger;
		}

		public IDictionary<string, int> GetDocumentTypesCount(
			string memberId,
			DocumentTypesCountRequest request)
		{
			// create a new list for filters; it has three more than the count of passed in filters so
			// we can add [Subscriber ID], [member suffix], and [Member Facing]
			if (request.Filters == null) request.Filters = new List<Filter>();

			List<Filter> filters = new List<Filter>(request.Filters.Count() + 3)
			{
				new Filter(
					"Subscriber ID",
					memberId.Substring(
						0,
						9))
			};

			if (memberId.Length == 11)
			{
				filters.Add(
					new Filter(
						"Member Suffix",
						memberId.Substring(
							9,
							2)));
			}

			if (!filters.Any(
				f => f.Name.Equals(
					"Member Facing",
					StringComparison.InvariantCulture)))
			{
				filters.Add(
					new Filter(
						"Member Facing",
						null));
			}

			filters.AddRange(request.Filters);

			request.Filters = filters;
			return _adapter.GetDocumentTypesCount(request);
		}
	}
}