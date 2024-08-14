// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   CareSource.WC.Entities
//   ClaimRemittance.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Entities.Claims
{
	using CareSource.WC.Entities.Common;

	/// <summary>
	///    Represents the data used to define a the claim remittance
	/// </summary>
	public class ClaimRemittance
	{
		/// <summary>
		///    Gets or sets the check no of the claim remittance class.
		/// </summary>
		public long? CheckNo { get; set; }

		/// <summary>
		///    Gets or sets the pay to provider address of the claim remittance class.
		/// </summary>
		public Address PayToProviderAddress { get; set; }

		/// <summary>
		///    Gets or sets the pay to provider identifier of the claim remittance class.
		/// </summary>
		public string PayToProviderId { get; set; }

		/// <summary>
		///    Gets or sets the name of the pay to provider.
		/// </summary>
		/// <value>
		///    The name of the pay to provider.
		/// </value>
		public string PayToProviderName { get; set; }
	}
}