using System.Collections.Generic;
using CareSource.WC.Entities.Requests.Base;

namespace CareSource.WC.Entities.Appeals
{
    public class WorkviewObjectItemRequest : BaseRequest
	{
		public string WorkViewApplicationName { get; set; }

		public string WorkViewClassName { get; set; }

		public IEnumerable<string> DisplayColumns { get; set; }
			= new HashSet<string>();

	}
}