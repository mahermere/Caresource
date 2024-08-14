// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   CareSource.WC.Entities
//   ClaimLineItem.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Entities.Claims
{
	public class ClaimLineItem
	{
		public string DenialReason { get; set; }
		public string FullDiagnosis { get; set; }
		public string PlaceOfService { get; set; }
		public string RDiagnosisSummary { get; set; } // What does R stand for?
		public string RTypeOfService { get; set; } // What does R stand for?
	}
}