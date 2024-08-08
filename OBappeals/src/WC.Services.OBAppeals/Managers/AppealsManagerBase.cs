// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   WorkFlowAndCollab.API.OBAppeals
//   AppealsManagerBase.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.OBAppeals.Managers
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using CareSource.WC.Entities.Appeals;
	using CareSource.WC.Entities.Common;
	using CareSource.WC.Entities.Common.Interfaces;
	using CareSource.WC.Entities.Exceptions;
	using CareSource.WC.OnBase.Core.ExtensionMethods;
	using CareSource.WC.Services.OBAppeals.Adapters.Interfaces;
	using CareSource.WC.Services.OBAppeals.Managers.Interfaces;

	public abstract class AppealsManagerBase : IAppealsManager<Appeal>
	{
		protected const string WvApplication = "Appeals";
		protected const string WvClass = "Case";
		protected const string ObjectId = "ObjectId";
		protected const string FacetsGrievanceID = "FacetsGrievanceID";
		protected const string Status = "Status";
		protected const string CaseLevel = "CaseLevel";
		protected const string CorporateReceivedDateTime = "CorporateReceivedDateTime";
		protected const string CaseDueDateTime = "CaseDueDate/Time";
		protected const string AppealCategoryCode = "AppealCategoryCode";
		protected const string CaseType = "TypeCode";
		protected const string CurrentAssignment = "CurrentAssignment";
		protected const string RIssuedescription = "RIssuedescription";
		protected const string RBeneficiaryLastName = "RBeneficiaryLastName";
		protected const string RBeneficiaryFirstName = "RBeneficiaryFirstName";
		protected const string RBeneficiaryMiddleInitial = "RBeneficiaryMiddleInitial";
		protected const string RCardholderID = "SubscriberID";
		protected const string ClaimId = "linktoAssociatedClaim.RClaimNumber";
		protected const string ProviderId = "linktoProvider.ProviderID";
		protected const string SubmissionMethod = "SubmissionMethod";
		protected const string Decision = "CaseDisposition";
		protected const string DateClosed = "CaseClosedDate/Time";
		protected const string CaseNumber = "CaseNumber";
		protected const string HisTab_SubType = "SubtypeCode";  
		protected const string DecisionDate = "CaseDispositionDateTime"; 
		protected const string HisTab_Expedited = "CaseOpeningPriority";
		//protected const string HisTab_DateUpdated = "";
		protected const string HisTab_Reason = "DecisionReasonCode";
		protected const string HisTab_NotifiedOnOption1 = "OralNotificationDateTime";
		protected const string HisTab_NotifiedOnOption2 = "WrittenNotificationDateTime";
		protected const string HisTab_Explanation = "CaseDispositionRationale";
		protected const string HisTab_ProviderNPI = "linktoProvider.NPI";
		protected const string HisTab_ProviderName = "linktoProvider.Name";
		protected const string HisTab_ProviderPhone = "linktoProvider.Phone";
		protected const string LinkedTab_CallID = "CallID";
		protected const string Admin_AdminCat1 = "AdminCat1Code";
		protected const string AllNotes = "AllNotes";

		// Used to determine a particular data point to use.  Not in the response.
		protected const string OralResolution = "OralResolution";


		protected AppealsManagerBase(
			IWorkViewObjectsAdapter<WorkViewObjectsHeader> workviewobjectsbroker)
			=> Broker = workviewobjectsbroker;

		protected IWorkViewObjectsAdapter<WorkViewObjectsHeader> Broker { get; }

		protected ListAppealsRequest RequestData { get; set; }

		protected WorkviewObjectItemRequest RequestAppealData { get; set; }

		protected string SearchId { get; set; }

		public ISearchResults<Appeal> Search(string Id, ListAppealsRequest request)
		{
			SearchId = Id.SafeTrim();
			RequestData = request;

			ValidateRequest();

			SetFilters();

			SetDisplayColumns();

			ISearchResults<WorkViewObjectsHeader> results = Broker.Search(
				new ListWorkViewObjectsRequest
				{
					Filters = RequestData.Filters,
					DisplayColumns = request.DisplayColumns,
					WorkViewApplicationName = WvApplication,
					WorkViewClassName = WvClass
				});

			if (results == null || !results.Results.Any())
			{
				throw new NoResultsFoundException(string.Empty);
			}

			return new SearchResults<Appeal>
			{
				TotalRecordCount = results.Results.Count(),
				Results = results.Results
					.Select(
						o => new Appeal
						{
							ObjectId = o.ObjectId.ToString(),
							Id = GetAttributeValue(o, FacetsGrievanceID)
								.ToString(),
							Status = GetAttributeValue(o, Status)
								.ToString(),
							CaseLevel = GetAttributeValue(o, CaseLevel)
								.ToString(),
							DateReceived = ToDateTime(GetAttributeValue(o, CorporateReceivedDateTime)),
							CaseDueDate = ToDateTime(GetAttributeValue(o, CaseDueDateTime)),
							CategoryCode = GetAttributeValue(o, AppealCategoryCode)
								.ToString(),
							Type = GetAttributeValue(o, CaseType)
								.ToString(),
							User = GetAttributeValue(o, CurrentAssignment)
								.ToString(),
							IssueDescription = GetAttributeValue(o, RIssuedescription)
								.ToString(),
							Member = GetMember(o.DisplayAttributes),
							ClaimId = GetAttributeValue(o, ClaimId)
								.ToString(),
							ProviderId = GetAttributeValue(o, ProviderId)
								.ToString(),
							SubmissionMethod = GetAttributeValue(o, SubmissionMethod)
								.ToString(),
							Decision = GetAttributeValue(o, Decision)
								.ToString(),
							DateClosed = ToDateTime(GetAttributeValue(o, DateClosed)),
							CaseNumber = GetAttributeValue(o, CaseNumber)
								.ToString(),
							DecisionDate = ToDateTime(GetAttributeValue(o, DecisionDate)),

							DisplayColumns = GetDisplayColumns(o.DisplayAttributes)
						})
			};
		}

		public Appeal GetAppeal(long objectId, WorkviewObjectItemRequest request)
		{
			RequestAppealData = request;

			SetDisplayColumnsAppeal();

			WorkViewObjectsHeader result = Broker.GetWVObject(
				objectId,
				new WorkviewObjectItemRequest
				{
					WorkViewApplicationName = WvApplication,
					WorkViewClassName = WvClass,
					DisplayColumns = request.DisplayColumns
				});

			return new Appeal
			{
				ObjectId = result.ObjectId.ToString(),
				Id = GetAttributeValue(result, FacetsGrievanceID).ToString(),
				Status = GetAttributeValue(result, Status).ToString(),
				CaseLevel = GetAttributeValue(result, CaseLevel).ToString(),
				DateReceived = ToDateTime(GetAttributeValue(result, CorporateReceivedDateTime)),
				CaseDueDate = ToDateTime(GetAttributeValue(result, CaseDueDateTime)),
				CategoryCode = GetAttributeValue(result, AppealCategoryCode)
					.ToString(),
				Type = GetAttributeValue(result, CaseType)
					.ToString(),
				User = GetAttributeValue(result, CurrentAssignment)
					.ToString(),
				IssueDescription = GetAttributeValue(result, RIssuedescription)
					.ToString(),
				Member = GetMember(result.DisplayAttributes),
				ClaimId = GetAttributeValue(result, ClaimId)
					.ToString(),
				ProviderId = GetAttributeValue(result, ProviderId)
					.ToString(),
				SubmissionMethod = GetAttributeValue(result, SubmissionMethod)
					.ToString(),
				Decision = GetAttributeValue(result, Decision)
					.ToString(),
				DateClosed = ToDateTime(GetAttributeValue(result, DateClosed)),
				CaseNumber = GetAttributeValue(result, CaseNumber)
					.ToString(),
				SubType = GetAttributeValue(result, HisTab_SubType)
					.ToString(),
				DecisionDate = ToDateTime(GetAttributeValue(result, DecisionDate)),
				Expedited = GetAttributeValue(result, HisTab_Expedited)
					.ToString(),
				UpdatedBy = GetAttributeValue(result, CurrentAssignment)
					.ToString(),
				//Reason = GetAttributeValue(result, Decision)
				//	.ToString(),
				Reason = GetAttributeValue(result, HisTab_Reason).ToString() == string.Empty
					? "" : GetReasonDescription(GetAttributeValue(result, HisTab_Reason)
						.ToString()),
				DateNotifiedOn = ToDateTime(GetAttributeValue(result, HisTab_NotifiedOnOption2)),
				Explanation = GetAttributeValue(result, HisTab_Explanation)
					.ToString(),
				ProviderNPI = GetAttributeValue(result, HisTab_ProviderNPI)
					.ToString(),
				ProviderName = GetAttributeValue(result, HisTab_ProviderName)
					.ToString(),
				ProviderPhone = GetAttributeValue(result, HisTab_ProviderPhone)
					.ToString(),
				NextReviewDate = ToDateTime(GetAttributeValue(result, CaseDueDateTime)),
				Initiated = ToDateTime(GetAttributeValue(result, CorporateReceivedDateTime)),
				DecisionCode = GetAttributeValue(result, Decision)
					.ToString(),
				StatusDate = ToDateTime(GetAttributeValue(result, DecisionDate)),
				PrimaryUser = GetAttributeValue(result, CurrentAssignment)
					.ToString(),
				LinkType = GetAttributeValue(result, ClaimId)
					.ToString(),
				CallId = GetAttributeValue(result, LinkedTab_CallID)
					.ToString(),
				AllNotes = (List<AllNotes>) GetAttributeValue(result, AllNotes),
				AdminCat1 = GetAttributeValue(result, Admin_AdminCat1)
					.ToString(),
				DisplayColumns = GetDisplayColumnsAppeal(result.DisplayAttributes)
			};
		}

		protected abstract void SetFilters();

		protected virtual void SetDisplayColumns()
		{
			List<string> displayColumns = RequestData.DisplayColumns.ToList();

			if (!RequestData.DisplayColumns.Any(f => f.Equals(FacetsGrievanceID)))
			{
				displayColumns.Add(FacetsGrievanceID);
			}

			if (!RequestData.DisplayColumns.Any(f => f.Equals(Status)))
			{
				displayColumns.Add(Status);
			}

			if (!RequestData.DisplayColumns.Any(f => f.Equals(CaseLevel)))
			{
				displayColumns.Add(CaseLevel);
			}

			if (!RequestData.DisplayColumns.Any(f => f.Equals(CorporateReceivedDateTime)))
			{
				displayColumns.Add(CorporateReceivedDateTime);
			}

			if (!RequestData.DisplayColumns.Any(f => f.Equals(CaseDueDateTime)))
			{
				displayColumns.Add(CaseDueDateTime);
			}

			if (!RequestData.DisplayColumns.Any(f => f.Equals(AppealCategoryCode)))
			{
				displayColumns.Add(AppealCategoryCode);
			}

			if (!RequestData.DisplayColumns.Any(f => f.Equals(CaseType)))
			{
				displayColumns.Add(CaseType);
			}

			if (!RequestData.DisplayColumns.Any(f => f.Equals(CurrentAssignment)))
			{
				displayColumns.Add(CurrentAssignment);
			}

			if (!RequestData.DisplayColumns.Any(f => f.Equals(RIssuedescription)))
			{
				displayColumns.Add(RIssuedescription);
			}

			if (!RequestData.DisplayColumns.Any(f => f.Equals(RBeneficiaryLastName)))
			{
				displayColumns.Add(RBeneficiaryLastName);
			}

			if (!RequestData.DisplayColumns.Any(f => f.Equals(RBeneficiaryFirstName)))
			{
				displayColumns.Add(RBeneficiaryFirstName);
			}

			if (!RequestData.DisplayColumns.Any(f => f.Equals(RBeneficiaryMiddleInitial)))
			{
				displayColumns.Add(RBeneficiaryMiddleInitial);
			}

			if (!RequestData.DisplayColumns.Any(f => f.Equals(RCardholderID)))
			{
				displayColumns.Add(RCardholderID);
			}

			if (!RequestData.DisplayColumns.Any(f => f.Equals(SubmissionMethod)))
			{
				displayColumns.Add(SubmissionMethod);
			}

			if (!RequestData.DisplayColumns.Any(f => f.Equals(Decision)))
			{
				displayColumns.Add(Decision);
			}

			if (!RequestData.DisplayColumns.Any(f => f.Equals(DateClosed)))
			{
				displayColumns.Add(DateClosed);
			}

			if (!RequestData.DisplayColumns.Any(f => f.Equals(ClaimId)))
			{
				displayColumns.Add(ClaimId);
			}

			if (!RequestData.DisplayColumns.Any(f => f.Equals(ProviderId)))
			{
				displayColumns.Add(ProviderId);
			}

			if (!RequestData.DisplayColumns.Any(f => f.Equals(CaseNumber)))
			{
				displayColumns.Add(CaseNumber);
			}

			RequestData.DisplayColumns = displayColumns;
		}

		protected virtual void SetDisplayColumnsAppeal()
		{
			List<string> displayColumns = RequestAppealData.DisplayColumns.ToList();

			if (!RequestAppealData.DisplayColumns.Any(f => f.Equals(FacetsGrievanceID)))
			{
				displayColumns.Add(FacetsGrievanceID);
			}

			if (!RequestAppealData.DisplayColumns.Any(f => f.Equals(Status)))
			{
				displayColumns.Add(Status);
			}

			if (!RequestAppealData.DisplayColumns.Any(f => f.Equals(CaseLevel)))
			{
				displayColumns.Add(CaseLevel);
			}

			if (!RequestAppealData.DisplayColumns.Any(f => f.Equals(CorporateReceivedDateTime)))
			{
				displayColumns.Add(CorporateReceivedDateTime);
			}

			if (!RequestAppealData.DisplayColumns.Any(f => f.Equals(CaseDueDateTime)))
			{
				displayColumns.Add(CaseDueDateTime);
			}

			if (!RequestAppealData.DisplayColumns.Any(f => f.Equals(AppealCategoryCode)))
			{
				displayColumns.Add(AppealCategoryCode);
			}

			if (!RequestAppealData.DisplayColumns.Any(f => f.Equals(CaseType)))
			{
				displayColumns.Add(CaseType);
			}

			if (!RequestAppealData.DisplayColumns.Any(f => f.Equals(CurrentAssignment)))
			{
				displayColumns.Add(CurrentAssignment);
			}

			if (!RequestAppealData.DisplayColumns.Any(f => f.Equals(RIssuedescription)))
			{
				displayColumns.Add(RIssuedescription);
			}

			if (!RequestAppealData.DisplayColumns.Any(f => f.Equals(RBeneficiaryLastName)))
			{
				displayColumns.Add(RBeneficiaryLastName);
			}

			if (!RequestAppealData.DisplayColumns.Any(f => f.Equals(RBeneficiaryFirstName)))
			{
				displayColumns.Add(RBeneficiaryFirstName);
			}

			if (!RequestAppealData.DisplayColumns.Any(f => f.Equals(RBeneficiaryMiddleInitial)))
			{
				displayColumns.Add(RBeneficiaryMiddleInitial);
			}

			if (!RequestAppealData.DisplayColumns.Any(f => f.Equals(RCardholderID)))
			{
				displayColumns.Add(RCardholderID);
			}

			if (!RequestAppealData.DisplayColumns.Any(f => f.Equals(ClaimId)))
			{
				displayColumns.Add(ClaimId);
			}

			if (!RequestAppealData.DisplayColumns.Any(f => f.Equals(ProviderId)))
			{
				displayColumns.Add(ProviderId);
			}

			if (!RequestAppealData.DisplayColumns.Any(f => f.Equals(SubmissionMethod)))
			{
				displayColumns.Add(SubmissionMethod);
			}

			if (!RequestAppealData.DisplayColumns.Any(f => f.Equals(Decision)))
			{
				displayColumns.Add(Decision);
			}

			if (!RequestAppealData.DisplayColumns.Any(f => f.Equals(DateClosed)))
			{
				displayColumns.Add(DateClosed);
			}

			if (!RequestAppealData.DisplayColumns.Any(f => f.Equals(CaseNumber)))
			{
				displayColumns.Add(CaseNumber);
			}

			if (!RequestAppealData.DisplayColumns.Any(f => f.Equals(HisTab_SubType)))
			{
				displayColumns.Add(HisTab_SubType);
			}

			if (!RequestAppealData.DisplayColumns.Any(f => f.Equals(DecisionDate)))
			{
				displayColumns.Add(DecisionDate);
			}

			if (!RequestAppealData.DisplayColumns.Any(f => f.Equals(HisTab_Expedited)))
			{
				displayColumns.Add(HisTab_Expedited);
			}

			if (!RequestAppealData.DisplayColumns.Any(f => f.Equals(HisTab_Reason)))
			{
				displayColumns.Add(HisTab_Reason);
			}

			if (!RequestAppealData.DisplayColumns.Any(f => f.Equals(HisTab_NotifiedOnOption2)))
			{
				displayColumns.Add(HisTab_NotifiedOnOption2);
			}

			if (!RequestAppealData.DisplayColumns.Any(f => f.Equals(HisTab_Explanation)))
			{
				displayColumns.Add(HisTab_Explanation);
			}

			if (!RequestAppealData.DisplayColumns.Any(f => f.Equals(HisTab_ProviderNPI)))
			{
				displayColumns.Add(HisTab_ProviderNPI);
			}

			if (!RequestAppealData.DisplayColumns.Any(f => f.Equals(HisTab_ProviderName)))
			{
				displayColumns.Add(HisTab_ProviderName);
			}

			if (!RequestAppealData.DisplayColumns.Any(f => f.Equals(HisTab_ProviderPhone)))
			{
				displayColumns.Add(HisTab_ProviderPhone);
			}

			if (!RequestAppealData.DisplayColumns.Any(f => f.Equals(LinkedTab_CallID)))
			{
				displayColumns.Add(LinkedTab_CallID);
			}

			if (!RequestAppealData.DisplayColumns.Any(f => f.Equals(Admin_AdminCat1)))
			{
				displayColumns.Add(Admin_AdminCat1);
			}

			//if (!RequestAppealData.DisplayColumns.Any(f => f.Equals(OralResolution)))
			//{
			//	displayColumns.Add(OralResolution);
			//}

			RequestAppealData.DisplayColumns = displayColumns;
		}

		private static object GetAttributeValue(
			WorkViewObjectsHeader objectHeader,
			string attirbuteName)
			=> objectHeader.DisplayAttributes.FirstOrDefault(
					attribute => attribute.Key.Equals(attirbuteName))
				.Value;

		private static DateTime? ToDateTime(object value)
			=> value == null ||
				value.ToString()
				.IsNullOrWhiteSpace()
				? new DateTime?()
				: Convert.ToDateTime(value);

		private Person GetMember(IDictionary<string, object> inputDisplayAttributes)
			=> new Person
			{
				LastName = inputDisplayAttributes
					.FirstOrDefault(attribute => attribute.Key.Equals(RBeneficiaryLastName))
					.Value.ToString(),
				FirstName = inputDisplayAttributes.FirstOrDefault(
						attribute => attribute.Key.Equals(RBeneficiaryFirstName))
					.Value.ToString(),
				MiddleInitial = inputDisplayAttributes.FirstOrDefault(
						attribute => attribute.Key.Equals(RBeneficiaryMiddleInitial))
					.Value.ToString(),
				Id = inputDisplayAttributes
					.FirstOrDefault(attribute => attribute.Key.Equals(RCardholderID))
					.Value.ToString()
			};

		protected Dictionary<string, object> GetDisplayColumns(IDictionary<string, object> columns)
		{
			string[] appealsFields =
			{
				FacetsGrievanceID,
				Status,
				CaseLevel,
				CorporateReceivedDateTime,
				CaseDueDateTime,
				AppealCategoryCode,
				CaseType,
				CurrentAssignment,
				RIssuedescription,
				RBeneficiaryLastName,
				RBeneficiaryFirstName,
				RBeneficiaryMiddleInitial,
				RCardholderID,
				ClaimId,
				ProviderId,
				SubmissionMethod,
				Decision,
				DateClosed,
				CaseNumber
			};

			return columns
				.Where(item => !appealsFields.Contains(item.Key))
				.ToDictionary(k => k.Key, k => k.Value);
		}

		protected Dictionary<string, object> GetDisplayColumnsAppeal(IDictionary<string, object> columns)
		{
			string[] appealsFields =
			{
				FacetsGrievanceID,
				Status,
				CaseLevel,
				CorporateReceivedDateTime,
				CaseDueDateTime,
				AppealCategoryCode,
				CaseType,
				CurrentAssignment,
				RIssuedescription,
				RBeneficiaryLastName,
				RBeneficiaryFirstName,
				RBeneficiaryMiddleInitial,
				RCardholderID,
				ProviderId,
				SubmissionMethod,
				Decision,
				DateClosed,
				CaseNumber,
				HisTab_SubType,
				DecisionDate,
				HisTab_Expedited,
				CurrentAssignment,
				HisTab_Reason,
				HisTab_NotifiedOnOption2,
				HisTab_Explanation,
				HisTab_ProviderNPI,
				HisTab_ProviderName,
				HisTab_ProviderPhone,
				CaseDueDateTime,
				CorporateReceivedDateTime,
				Decision,
				DateClosed,
				CurrentAssignment,
				ClaimId,
				LinkedTab_CallID,
				Admin_AdminCat1,
				AllNotes
			};

			return columns
				.Where(item => !appealsFields.Contains(item.Key))
				.ToDictionary(k => k.Key, k => k.Value);
		}

		private static string GetReasonDescription(
			string reasonCode)
		{
			string reasonDescription = null;

			switch (reasonCode)
			{
				case "01A":
					reasonDescription = $"{reasonCode} - Appeal Sustained-Part Approve";
					break;

				case "01":
					reasonDescription = $"{reasonCode} - Appeal Sustained-MCP Reversed";
					break;

				case "02":
					reasonDescription = $"{reasonCode} - Appeal Overruled-MCP Stands";
					break;

				case "102":
					reasonDescription = $"{reasonCode} - Clear Selection";
					break;

				default:
					reasonDescription = "";
					break;
			}

			return reasonDescription;
		}

		protected virtual void ValidateRequest()
		{ }
	}
}