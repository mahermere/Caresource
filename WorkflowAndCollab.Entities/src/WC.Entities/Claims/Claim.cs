// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   CareSource.WC.Entities
//   Claim.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Entities.Claims
{
	using System;

	public class Claim
	{
		/*
			Consider whether we could refer to the "Provider" object for those properties
			declared below that are related to the provider, and to the "Member" object for those
			properties that are related to the member.
		*/

		public decimal? AmountPaid { get; set; }
		public decimal? Charges { get; set; }
		public string ClaimId { get; set; }
		public DateTime? DateOfService { get; set; }
		public string MemberSuffix { get; set; }
		public string PatientFirstName { get; set; }
		public string PatientLastName { get; set; }
		public string Product { get; set; }
		public string ProviderId { get; set; }
		public string ProviderIdNpi { get; set; }
		public string ProviderName { get; set; }
		public string ProviderNetworkStatus { get; set; }
		public string SubscriberId { get; set; }
	}
}