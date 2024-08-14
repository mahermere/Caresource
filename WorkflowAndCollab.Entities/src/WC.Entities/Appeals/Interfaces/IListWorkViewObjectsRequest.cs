using System.Collections.Generic;
using CareSource.WC.Entities.Common;
using CareSource.WC.Entities.Requests.Base;

namespace CareSource.WC.Entities.Appeals.Interfaces
{
    public interface IListWorkViewObjectsRequest
	{
		string WorkViewApplicationName { get; set; }

		string WorkViewClassName { get; set; }

		IEnumerable<string> DisplayColumns { get; set; }

		IEnumerable<Filter> Filters { get; set; }

		Paging Paging { get; set; }

	}
}
