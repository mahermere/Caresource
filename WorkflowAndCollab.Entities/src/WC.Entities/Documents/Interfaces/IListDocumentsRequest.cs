// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   Entities
//   IListDocumentsRequest.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Entities.Documents.Interfaces
{
	using System;
	using System.Collections.Generic;
	using CareSource.WC.Entities.Common;
	using CareSource.WC.Entities.Requests.Base.Interfaces;

	public interface IListDocumentsRequest : IPagingRequest, ISortingRequest
	{
		Guid CorrelationGuid { get; set; }

		IEnumerable<string> DisplayColumns { get; set; }

		IEnumerable<string> DocumentTypes { get; set; }

		string EndDate { get; set; }

		IEnumerable<Filter> Filters { get; set; }

		string StartDate { get; set; }
	}
}