// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   Entities
//   BusinessContext.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Entities.Transactions
{
	using System.Collections.Generic;

	public class BusinessContext
	{
		public string BusinessStatus { get; set; }

		public List<ContextList> ContextList { get; set; }

		public ExternalContext ExternalContext { get; set; }

		public string MemberId { get; set; }

		public string ProviderId { get; set; }
		public string SubscriberId { get; set; }
	}
}