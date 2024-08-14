using System;
using System.Collections.Generic;
using CareSource.WC.Entities.Appeals.Interfaces;
using CareSource.WC.Entities.Common;
using CareSource.WC.Entities.Requests.Base;

namespace CareSource.WC.Entities.Appeals
{
    public class ListWorkViewObjectsRequest : BaseRequest, IListWorkViewObjectsRequest
	{
		public  Paging Paging { get; set; } = new Paging();

		public string WorkViewApplicationName { get; set; }

		public string WorkViewClassName { get; set; }

		public IEnumerable<Filter> Filters { get; set; }
			= new HashSet<Filter>();

        public DateTime StartDate { get; set; } = new DateTime(1753, 1, 1);

        public DateTime EndDate { get; set; } = new DateTime(9999, 12, 31);

        public IEnumerable<string> DisplayColumns { get; set; }
			= new HashSet<string>();
	}
}