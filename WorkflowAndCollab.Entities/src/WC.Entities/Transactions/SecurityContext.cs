// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   Entities
//   SecurityContext.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Entities.Transactions
{
	public class SecurityContext
	{
		public string AuthenticatedUserId { get; set; }

		public string Domain { get; set; }

		public string Password { get; set; }

		public string Policy { get; set; }

		public string Type { get; set; }
	}
}