﻿// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   CreateGrievanceAppeals
//   GrievanceAppealsInput.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.CreateGrievanceAppeals.Models.Requests
{
	using System.Collections.Generic;

	public class GrievanceAppealsInput
	{
		public string AdminCat1 { get; set; }
		public string AdminCat2 { get; set; }
		public string AOR { get; set; }
		public string AORExpires { get; set; }
		public string AORRepresentativeCity { get; set; }
		public string AORRepresentativeEmail { get; set; }
		public string AORRepresentativeFirst { get; set; }
		public string AORRepresentativeLast { get; set; }
		public string AORRepresentativePhone { get; set; }
		public string AORRepresentativeState { get; set; }
		public string AORRepresentativeStreet { get; set; }
		public string AORRepresentativeZIP { get; set; }
		public string ApplicationName { get; set; }
		public string AutoClose { get; set; }
		public string CallerStatus { get; set; }
		public string CallInMethod { get; set; }
		public string CallType { get; set; }
		public string Category { get; set; }
		public string CategoryCode { get; set; }
		public string Complaint { get; set; }
		public string CurrentUserName { get; set; }
		public string CustomerType { get; set; }
		public string DateComplete { get; set; }
		public string Decision { get; set; }
		public string Description { get; set; }
		public string EnrollmentEndDate { get; set; }
		public string EnrollmentStartDate { get; set; }
		public string ESBGUID { get; set; }
		public string FacetsGrievanceID { get; set; }
		public string GroupName { get; set; }
		public string HICN { get; set; }
		public string InitiationDate { get; set; }
		public string LinkReason { get; set; }
		public string LinkType { get; set; }
		public string MedicaidId { get; set; }
		public string MemberCity { get; set; }
		public string MemberContactPhone { get; set; }
		public string MemberDOB { get; set; }
		public string MemberEMail { get; set; }
		public string MemberFirst { get; set; }
		public string MemberID { get; set; }
		public string MemberLast { get; set; }
		public string MemberState { get; set; }
		public string MemberStreet { get; set; }
		public string MemberZIP { get; set; }
		public string Notes1 { get; set; }
		public string Notes10 { get; set; }
		public string Notes2 { get; set; }
		public string Notes3 { get; set; }
		public string Notes4 { get; set; }
		public string Notes5 { get; set; }
		public string Notes6 { get; set; }
		public string Notes7 { get; set; }
		public string Notes8 { get; set; }
		public string Notes9 { get; set; }
		public List<string> PageData { get; set; }
		public string PlanID { get; set; }
		public string PlanName { get; set; }
		public string PlanStatus { get; set; }
		public string PPAppealType { get; set; }
		public string PPBeginDOS { get; set; }
		public string PPClaimNumber { get; set; }
		public string PPDateOfService { get; set; }
		public string PPDateReceived { get; set; }
		public string PPDaysOld { get; set; }
		public string PPDisputeID { get; set; }
		public string PPDisputeType { get; set; }
		public string PPDocumentType { get; set; }
		public string PPEndDOS { get; set; }
		public string PPEnrolleeName { get; set; }
		public string PPFileName { get; set; }
		public string PPHasPriorAuth { get; set; }
		public string PPHealthPlan { get; set; }
		public string PPIsNonPar { get; set; }
		public string PPIsRetroAuth { get; set; }
		public string PPIsSNP { get; set; }
		public string PPPlanID { get; set; }
		public string PPSignature { get; set; }
		public string PPSignatureDate { get; set; }
		public string PPSubGroup { get; set; }
		public string Priority { get; set; }
		public string ProviderCity { get; set; }
		public string ProviderEmail { get; set; }
		public string ProviderFourCity { get; set; }
		public string ProviderFourEmail { get; set; }
		public string ProviderFourId { get; set; }
		public string ProviderFourName { get; set; }
		public string ProviderFourNPI { get; set; }
		public string ProviderFourPhone { get; set; }
		public string ProviderFourState { get; set; }
		public string ProviderFourStreet { get; set; }
		public string ProviderFourTaxID { get; set; }
		public string ProviderFourZIP { get; set; }
		public string ProviderName { get; set; }
		public string ProviderNPI { get; set; }
		public string ProviderOneId { get; set; }
		public string ProviderPhone { get; set; }
		public string ProviderState { get; set; }
		public string ProviderStreet { get; set; }
		public string ProviderThreeCity { get; set; }
		public string ProviderThreeEmail { get; set; }
		public string ProviderThreeId { get; set; }
		public string ProviderThreeName { get; set; }
		public string ProviderThreeNPI { get; set; }
		public string ProviderThreePhone { get; set; }
		public string ProviderThreeState { get; set; }
		public string ProviderThreeStreet { get; set; }
		public string ProviderThreeTaxID { get; set; }
		public string ProviderThreeZIP { get; set; }
		public string ProviderTwoCity { get; set; }
		public string ProviderTwoEmail { get; set; }
		public string ProviderTwoId { get; set; }
		public string ProviderTwoName { get; set; }
		public string ProviderTwoNPI { get; set; }
		public string ProviderTwoPhone { get; set; }
		public string ProviderTwoState { get; set; }
		public string ProviderTwoStreet { get; set; }
		public string ProviderTwoTaxID { get; set; }
		public string ProviderTwoZIP { get; set; }
		public string ProviderZIP { get; set; }
		public string Reason { get; set; }
		public string ReceivedDate { get; set; }
		public string RepresentativeTermDate { get; set; }
		public string Resolution1 { get; set; }
		public string Resolution10 { get; set; }
		public string Resolution2 { get; set; }
		public string Resolution3 { get; set; }
		public string Resolution4 { get; set; }
		public string Resolution5 { get; set; }
		public string Resolution6 { get; set; }
		public string Resolution7 { get; set; }
		public string Resolution8 { get; set; }
		public string Resolution9 { get; set; }
		public string Source { get; set; }
		public string Status { get; set; }
		public string StreamLineCallerID { get; set; }
		public string Subject { get; set; }
		public string SubjectCode { get; set; }
		public string SubscriberID { get; set; }
		public string SubType { get; set; }
		public string Summary { get; set; }
		public string TaxID { get; set; }
		public string Type { get; set; }
		public string UserId { get; set; }
	}
}