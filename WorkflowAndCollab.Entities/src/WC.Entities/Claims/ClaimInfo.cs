// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   CareSource.WC.Entities
//   ClaimInfo.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Entities.Claims
{
	using System.Collections.Generic;
	using CareSource.WC.Entities.Members;
	using CareSource.WC.Entities.Providers;

	public class ClaimInfo
	{
		/*
			The script context would need to deal with updates to the attributes differently - probably some private methods for overloads for each type.
			The body of that method can check for null, and perform the necessary remove and/or update operations.
		*/

		public string ClaimId { get; set; }
		public List<ClaimLineItem> LineItems { get; set; } = new List<ClaimLineItem>();
		public Member Member { get; set; }
		public Provider Provider { get; set; }
		public List<ClaimRemittance> Remittance { get; set; } = new List<ClaimRemittance>();
	}
}