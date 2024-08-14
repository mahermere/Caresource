using System;
using System.Collections.Generic;

namespace CareSource.WC.Entities.Appeals
{
	public class Appeal
	{
		public string ObjectId { get; set; }

		public string Id { get; set; }

		public string Status { get; set; }

		public string CaseLevel { get; set; }

		public DateTime? DateReceived { get; set; }

		public DateTime? CaseDueDate { get; set; } 

		public string CategoryCode { get; set; } 

		public string Type { get; set; } 

		public string User { get; set; } 

		public string IssueDescription { get; set; }

		public Person Member { get; set; }

		public string ClaimId { get; set; }

		public string ProviderId { get; set; }

		public string SubmissionMethod { get; set; }

		public string Decision { get; set; }

		public DateTime? DateClosed { get; set; }

		public string CaseNumber { get; set; }

		public string GrvId { get; set; }

		public string SubType { get; set; }

		public DateTime? DecisionDate { get; set; }

		public string Expedited { get; set; }

		public string UpdatedBy { get; set; }

		public DateTime? DateUpdated { get; set; }

		public string Reason { get; set; }

		public DateTime? DateNotifiedOn { get; set; }

		public string NotifiedBy { get; set; }

		public string Explanation { get; set; }

		public string ProviderNPI { get; set; }

		public string ProviderName { get; set; }

		public string ProviderPhone { get; set; }

		public DateTime? NextReviewDate { get; set; }
		
		public DateTime? Initiated { get; set; }

		public string DecisionCode { get; set; }

		public DateTime? StatusDate { get; set; }

		public string PrimaryUser { get; set; }

		public string LinkType { get; set; }

		public string CallId { get; set; }

		public string LinkReason { get; set; }

		public string AdminCat1 { get; set; }

		public string AdminCat2 { get; set; }

		public List<AllNotes> AllNotes { get; set; }

		/// <summary>
		///    Gets or sets the display columns of the document header class.
		/// </summary>
		public IDictionary<string, object> DisplayColumns { get; set; } =
			new Dictionary<string, object>();
	}

	public class Person
	{ 
		public string LastName { get; set; }

		public string FirstName { get; set; }

		public string MiddleInitial { get; set; }

		public string Id { get; set; }

	}

	public class AllNotes
	{
		public string NoteType { get; set; }

		public string CreatedBy { get; set; }

		public string Note { get; set; }
	}
}
