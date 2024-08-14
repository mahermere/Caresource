// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   Entities
//   ExternalContext.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Entities.Transactions
{
	public class ExternalContext
	{
		public BusinessSecurityContext BusinessSecurityContext { get; set; }

		public string ExternalMessageID { get; set; }
		public string ExternalTransactionID { get; set; }
	}
}