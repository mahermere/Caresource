namespace CareSource.WC.Entities.Appeals
{
	using CareSource.WC.Entities.Common;
	using CareSource.WC.Entities.Requests.Base;
	using System.Collections.Generic;

	public class AppealRequest: BaseRequest
	{
		public IEnumerable<string> DisplayColumns { get; set; }
			= new HashSet<string>();
	}
}
