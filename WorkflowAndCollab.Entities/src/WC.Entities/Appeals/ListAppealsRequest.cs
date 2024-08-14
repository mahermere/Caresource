namespace CareSource.WC.Entities.Appeals
{
    using CareSource.WC.Entities.Common;
    using CareSource.WC.Entities.Requests.Base;
    using System.Collections.Generic;

	public class ListAppealsRequest : BaseRequest
	{ 
		public Paging Paging { get; set; } = new Paging();

		public IEnumerable<Filter> Filters { get; set; }
			= new HashSet<Filter>();

		public IEnumerable<string> DisplayColumns { get; set; }
			= new HashSet<string>();
	}
}