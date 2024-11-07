// ------------------------------------------------------------------------------------------------
//  <copyright>
//    Copyright (c) CareSource, 2020-2022.  All rights reserved.
// 
//    Claims
//    Claim.cs
//  </copyright>
//  ------------------------------------------------------------------------------------------------

namespace Claims.Models
{
	using System;

	public class Claim
	{
		public string ClaimId
		{
			get;
			set;
		}

		public string ProductCategory
		{
			get;
			set;
		}

		public string ClaimAssignmentIndicator
		{
			get;
			set;
		}

		public string ClaimType
		{
			get;
			set;
		}

		public string ClaimTypeDescription
		{
			get;
			set;
		}

		public string ClaimSubType
		{
			get;
			set;
		}

		public string ClaimSubTypeDescription
		{
			get;
			set;
		}

		public string ClaimStatusCode
		{
			get;
			set;
		}

		public string ClaimStatusDescription
		{
			get;
			set;
		}

		public DateTime? ReceivedDate
		{
			get;
			set;
		}

		public DateTime? EarliestDateOfService
		{
			get;
			set;
		}

		public DateTime? LatestDateOfService
		{
			get;
			set;
		}

		public decimal? TotalCharge
		{
			get;
			set;
		}

		public decimal? TotalPayable
		{
			get;
			set;
		}

		public decimal? PatientPaidAmount
		{
			get;
			set;
		}

		public string PatientAccountNumber
		{
			get;
			set;
		}

		public string CapitationIndicator
		{
			get;
			set;
		}

		public string CapitationIndicatorDescription
		{
			get;
			set;
		}

		public string ProviderId
		{
			get;
			set;
		}

		public string ProviderName
		{
			get;
			set;
		}

		public string ProviderNpi
		{
			get;
			set;
		}

		public long? ContrivedSubscriberKey
		{
			get;
			set;
		}

		public string SubscriberId
		{
			get;
			set;
		}

		public int? SubscriberSuffix
		{
			get;
			set;
		}

		public string MemberFirstName
		{
			get;
			set;
		}

		public string MemberLastName
		{
			get;
			set;
		}

		public string MemberMiddleInitial
		{
			get;
			set;
		}

		public string CategoryDescription
		{
			get;
			set;
		}

		public string GroupName
		{
			get;
			set;
		}

		public string GroupId
		{
			get;
			set;
		}

		public string SubGroupName
		{
			get;
			set;
		}

		public string PlanId
		{
			get;
			set;
		}

		public string PlanDescription
		{
			get;
			set;
		}

		public string ClaimPayeeIndicator
		{
			get;
			set;
		}

		public decimal? CarrierPaymentAmount
		{
			get;
			set;
		}

		public decimal? CarrierDisallowAmount
		{
			get;
			set;
		}

		public decimal? CarrierAllowAmount
		{
			get;
			set;
		}

		public DateTime? DateOfLastAction
		{
			get;
			set;
		}

		public DateTime? EnteredDate
		{
			get;
			set;
		}

		public DateTime? PaidDate
		{
			get;
			set;
		}

		public string NetworkId
		{
			get;
			set;
		}

		public string NetworkName
		{
			get;
			set;
		}

		public string AgreementId
		{
			get;
			set;
		}

		public string PayeeProviderId
		{
			get;
			set;
		}

		public string NetworkIndicator
		{
			get;
			set;
		}

		public string HasPrimaryCarePhysician
		{
			get;
			set;
		}

		public string ExplanationOfBenefitCode
		{
			get;
			set;
		}

		public string ProviderEntity
		{
			get;
			set;
		}

		public string HraIndicator
		{
			get;
			set;
		}

		public string HraDescription
		{
			get;
			set;
		}

		public decimal? HraConsiderAmount
		{
			get;
			set;
		}

		public decimal? HraPaidAmount
		{
			get;
			set;
		}

		public decimal? FamilyDeductible
		{
			get;
			set;
		}

		public decimal? MemberDeductible
		{
			get;
			set;
		}

		public decimal? FamilyDeductibleApplied
		{
			get;
			set;
		}

		public decimal? MemberDeductibleApplied
		{
			get;
			set;
		}

		public decimal? SubscriberHraPaid
		{
			get;
			set;
		}

		public decimal? MemberHraPaid
		{
			get;
			set;
		}

		public string ProductId
		{
			get;
			set;
		}

		public string FacilityId
		{
			get;
			set;
		}

		public string InputTaxId
		{
			get;
			set;
		}

		public DateTime? AdmissionDate
		{
			get;
			set;
		}

		public DateTime? DischargeDate
		{
			get;
			set;
		}

		public DateTime? HospitalStatementFromDate
		{
			get;
			set;
		}

		public DateTime? HospitalStatementToDate
		{
			get;
			set;
		}

		public string OriginallySubmittedSubscriberId
		{
			get;
			set;
		}

		public string RelatedFacilityNpi
		{
			get;
			set;
		}

		public string RelatedFacilityTaxId
		{
			get;
			set;
		}

		public string ReferringProviderNpi
		{
			get;
			set;
		}

		public string ReferringProviderTaxId
		{
			get;
			set;
		}

		public string ServicingProviderNpi
		{
			get;
			set;
		}

		public string ServicingProviderTaxId
		{
			get;
			set;
		}

		public string UniqueHealthId
		{
			get;
			set;
		}

		public string CodingSystem
		{
			get;
			set;
		}

		public string TotalPaymentApplied
		{
			get;
			set;
		}

		public int? QueueId
		{
			get;
			set;
		}

		public string QueueName
		{
			get;
			set;
		}

		public int? RoleId
		{
			get;
			set;
		}

		public string RoleName
		{
			get;
			set;
		}
	}
}