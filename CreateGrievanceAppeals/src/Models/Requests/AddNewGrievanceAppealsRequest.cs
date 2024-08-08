// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   CreateGrievanceAppeals
//   AddNewGrievanceAppealsRequest.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.CreateGrievanceAppeals.Models.Requests
{
	using System.ComponentModel.DataAnnotations;

	/// <summary>
	///    Public class RequestPayload
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class RequestPayload<T> where T : new()
	{
		/// <summary>
		///    TransactionHeader used to administer transactional data
		/// </summary>
		public TransactionHeader TransactionData { get; set; }

		/// <summary>
		///    T Payload for all other properties
		/// </summary>
		public T Payload { get; set; }
	}

	/// <summary>
	///    GrievanceAppealSubmisison
	/// </summary>
	public class GrievanceAppealSubmission
	{
		[MaxLength(250)]
		public string AdminCat1 { get; set; }

		[MaxLength(250)]
		public string AdminCat2 { get; set; }

		[MaxLength(5)]
		public string AOR { get; set; }

		public string AORExpires { get; set; }

		[MaxLength(30)]
		public string AORRepresentativeCity { get; set; }

		[MaxLength(40)]
		public string AORRepresentativeEmail { get; set; }

		[MaxLength(30)]
		public string AORRepresentativeFirst { get; set; }

		[MaxLength(35)]
		public string AORRepresentativeLast { get; set; }

		[MaxLength(14)]
		//[RegularExpression("\\d+", ErrorMessage = "Must be numeric")]
		public string AORRepresentativePhone { get; set; }

		[MaxLength(2)]
		public string AORRepresentativeState { get; set; }

		[MaxLength(50)]
		public string AORRepresentativeStreet { get; set; }

		[MaxLength(10)]
		public string AORRepresentativeZIP { get; set; }

		[MaxLength(40)]
		public string AppealType { get; set; }

		/// <summary>
		///    Required - Valid values ("SL", "MP", "PP", "NC")
		/// </summary>
		[Required]
		[MaxLength(10)]
		public string ApplicationName { get; set; }

		[MaxLength(5)]
		public string AutoClose { get; set; }

		public string BeginDOS { get; set; }

		[MaxLength(25)]
		public string CallerId { get; set; }

		[MaxLength(10)]
		public string CallerStatus { get; set; }

		[MaxLength(250)]
		public string CallInMethod { get; set; }

		/// <summary>
		///    Required - Valid values ("GRIEVANCE", "APPEAL", "DISPUTE")
		/// </summary>
		[Required]
		[MaxLength(250)]
		public string CallType { get; set; }

		[MaxLength(250)]
		public string Category { get; set; }

		[MaxLength(5)]
		public string CategoryCode { get; set; }

		[MaxLength(15)]
		public string ClaimId { get; set; }

		[MaxLength(4)]
		public string Complaint { get; set; }

		[MaxLength(60)]
		public string CurrentUserName { get; set; }

		[MaxLength(250)]
		public string CustomerType { get; set; }

		[MaxLength(250)]
		public string DateComplete { get; set; }

		public string DateOfService { get; set; }
		public string DateReceived { get; set; }

		[MaxLength(6)]
		public string DaysOld { get; set; }

		[MaxLength(250)]
		public string Decision { get; set; }

		[MaxLength(250)]
		public string Description { get; set; }

		[MaxLength(25)]
		public string DisputeId { get; set; }

		[MaxLength(60)]
		public string DisputeType { get; set; }

		//public string DocumentCount { get; set; }
		public string EndDOS { get; set; }

		[MaxLength(50)]
		public string EnrolleeName { get; set; }

		public string EnrollmentEndDate { get; set; }
		public string EnrollmentStartDate { get; set; }

		[MaxLength(14)]
		public string FacetsGrievanceId { get; set; }

		[MaxLength(250)]
		public string GroupName { get; set; }

		[MaxLength(10)]
		public string HasPriorAuth { get; set; }

		[MaxLength(50)]
		public string HealthPlan { get; set; }

		[MaxLength(12)]
		public string HICN { get; set; }

		public string InitiationDate { get; set; }

		[MaxLength(5)]
		public string IsNonPar { get; set; }

		[MaxLength(10)]
		public string IsRetroAuth { get; set; }

		[MaxLength(5)]
		public string IsSNP { get; set; }

		[MaxLength(60)]
		public string IssueCategory { get; set; }

		[MaxLength(250)]
		public string LinkReason { get; set; }

		[MaxLength(250)]
		public string LinkType { get; set; }

		[MaxLength(20)]
		public string MedicaidId { get; set; }

		[MaxLength(50)]
		public string MemberCity { get; set; }

		[MaxLength(15)]
		public string MemberContactPhone { get; set; }

		public string MemberDOB { get; set; }

		[MaxLength(50)]
		public string MemberEMail { get; set; }

		[MaxLength(30)]
		public string MemberFirst { get; set; }

		[MaxLength(15)]
		public string MemberId { get; set; }

		[MaxLength(35)]
		public string MemberLast { get; set; }

		[MaxLength(2)]
		public string MemberState { get; set; }

		[MaxLength(50)]
		public string MemberStreet { get; set; }

		[MaxLength(10)]
		public string MemberZIP { get; set; }

		[MaxLength(2500)]
		public string Notes { get; set; }

		[MaxLength(20)]
		public string PlanId { get; set; }

		[MaxLength(250)]
		public string PlanName { get; set; }

		[MaxLength(10)]
		public string PlanStatus { get; set; }

		[MaxLength(10)]
		public string Priority { get; set; }

		[MaxLength(40)]
		public string ProviderCity { get; set; }

		[MaxLength(75)]
		public string ProviderContactName { get; set; }

		[MaxLength(50)]
		public string ProviderEmail { get; set; }

		[MaxLength(40)]
		public string ProviderFourCity { get; set; }

		[MaxLength(75)]
		public string ProviderFourContactName { get; set; }
		
		[MaxLength(50)]
		public string ProviderFourEmail { get; set; }

		[MaxLength(25)]
		public string ProviderFourId { get; set; }

		[MaxLength(55)]
		public string ProviderFourName { get; set; }

		[MaxLength(10)]
		public string ProviderFourNPI { get; set; }

		[MaxLength(14)]
		public string ProviderFourPhone { get; set; }

		[MaxLength(2)]
		public string ProviderFourState { get; set; }

		[MaxLength(100)]
		public string ProviderFourStreet { get; set; }

		[MaxLength(12)]
		public string ProviderFourTaxId { get; set; }

		[MaxLength(10)]
		public string ProviderFourZIP { get; set; }

		[MaxLength(25)]
		public string ProviderId { get; set; }

		[MaxLength(60)]
		public string ProviderName { get; set; }

		[MaxLength(10)]
		public string ProviderNPI { get; set; }

		[MaxLength(14)]
		public string ProviderPhone { get; set; }

		[MaxLength(2)]
		public string ProviderState { get; set; }

		[MaxLength(100)]
		public string ProviderStreet { get; set; }

		[MaxLength(40)]
		public string ProviderThreeCity { get; set; }

		[MaxLength(75)]
		public string ProviderThreeContactName { get; set; }

		[MaxLength(50)]
		public string ProviderThreeEmail { get; set; }

		[MaxLength(25)]
		public string ProviderThreeId { get; set; }

		[MaxLength(55)]
		public string ProviderThreeName { get; set; }

		[MaxLength(10)]
		public string ProviderThreeNPI { get; set; }

		[MaxLength(14)]
		public string ProviderThreePhone { get; set; }

		[MaxLength(2)]
		public string ProviderThreeState { get; set; }

		[MaxLength(100)]
		public string ProviderThreeStreet { get; set; }

		[MaxLength(12)]
		public string ProviderThreeTaxId { get; set; }

		[MaxLength(10)]
		public string ProviderThreeZIP { get; set; }

		[MaxLength(40)]
		public string ProviderTwoCity { get; set; }

		[MaxLength(75)]
		public string ProviderTwoContactName { get; set; }

		[MaxLength(50)]
		public string ProviderTwoEmail { get; set; }

		[MaxLength(25)]
		public string ProviderTwoId { get; set; }

		[MaxLength(55)]
		public string ProviderTwoName { get; set; }

		[MaxLength(10)]
		public string ProviderTwoNPI { get; set; }

		[MaxLength(14)]
		public string ProviderTwoPhone { get; set; }

		[MaxLength(2)]
		public string ProviderTwoState { get; set; }

		[MaxLength(100)]
		public string ProviderTwoStreet { get; set; }

		[MaxLength(12)]
		public string ProviderTwoTaxId { get; set; }

		[MaxLength(10)]
		public string ProviderTwoZIP { get; set; }

		[MaxLength(10)]
		public string ProviderZIP { get; set; }

		[MaxLength(250)]
		public string Reason { get; set; }

		public string ReceivedDate { get; set; }
		public string RepresentativeTermDate { get; set; }

		[MaxLength(2500)]
		public string Resolution { get; set; }

		[MaxLength(50)]
		public string Signature { get; set; }

		public string SignatureDate { get; set; }

		[MaxLength(250)]
		public string Status { get; set; }

		[MaxLength(250)]
		public string SubGroup { get; set; }

		[MaxLength(250)]
		public string Subject { get; set; }

		[MaxLength(5)]
		public string SubjectCode { get; set; }

		[MaxLength(250)]
		public string SubType { get; set; }

		[MaxLength(250)]
		public string Summary { get; set; }

		[MaxLength(12)]
		public string TaxId { get; set; }

		[MaxLength(250)]
		public string Type { get; set; }

		public KeywordValue[] AdditionalInput { get; set; }
	}

	public class TransactionHeader
	{
		public string CorrelationId { get; set; }

		[MaxLength(3)]
		public string DocumentCount { get; set; }

		/// <summary>
		///    Required - ESB will generate
		/// </summary>
		[Required]
		[MaxLength(36)]
		public string ESBGUID { get; set; }

		[MaxLength(1)]
		public string IncludeDocuments { get; set; }

		public string Notify { get; set; }
		public string NotifyType { get; set; }
		public string NotifyList { get; set; }
		public string SourceAppId { get; set; }
		public string SourceAppName { get; set; }
		public string TransactionType { get; set; }
		public string UserId { get; set; }
	}

	public class KeywordValue
	{
		public string InputName { get; set; }
		public string Value { get; set; }
	}
}