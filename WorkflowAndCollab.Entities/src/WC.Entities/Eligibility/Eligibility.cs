// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2018. All rights reserved.
// 
//   CareSource.WC.Entities
//   Eligibility.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Entities.Eligibility
{
	using System;

	public class Eligibility
	{
		public string CategoryDescription { get; set; }
		public string ContractId { get; set; }
		public long? ContrivedKey { get; set; }
		public DateTime? EffectiveDate { get; set; }
		public string PlanName { get; set; }
		public string PolicyStatus { get; set; }
		public DateTime? TermDate { get; set; }
        public string EligibilityIndicator { get; set; }
	}
}