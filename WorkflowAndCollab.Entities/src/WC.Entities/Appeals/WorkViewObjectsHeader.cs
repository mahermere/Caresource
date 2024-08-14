using System.Collections.Generic;

namespace CareSource.WC.Entities.Appeals
{
    public class WorkViewObjectsHeader
	{
		/// <summary>
		/// Gets or sets the WorkView Object ID.
		/// </summary>
		public long ObjectId { get; set; }

		/// <summary>
		/// Gets or sets the display attributes of the WorkViewObjectsHeader class.
		/// </summary>
		public IDictionary<string, object> DisplayAttributes { get; set; } =
			new Dictionary<string, object>();
	}
}