// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   WorkFlowAndCollab.API.OBAppeals
//   GrievancesManagerBase.cs
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

	public abstract class GrievancesManagerBase : IGrievancesManager<Appeal>
	{
		protected const string WvApplication = "Grievances";
		protected const string WvClass = "Grievance";

		protected const string ObjectId = "ObjectId";
		protected const string FacetsGrievanceID = "FacetsGrievanceID";
		protected const string Status = "Status";
		protected const string DateReceived = "DateReceived";
		protected const string CaseDueDateTime = "Deadline";
		protected const string AppealCategoryCode = "GrievanceCategoryCode";
		protected const string CaseType = "GrievanceType";   //"TypeCode"
		protected const string CurrentAssignment = "AssignedCaseWorker";
		protected const string Issuedescription = "IssueDescription";
		protected const string MemberLast = "MemberLast";
		protected const string MemberFirst = "MemberFirst";
		protected const string MemberMiddleInitial = "MemberMiddle";
		protected const string MemberID = "MemberID";
		protected const string ProviderID = "Provider1ID";
		protected const string ReceiptMethod = "ReceiptMethod";
		protected const string Decision = "DecisionCode";
		protected const string DateComplete = "DateComplete";
		protected const string GrvId = "GrvId";
		protected const string HisTab_SubType = "GrievanceSubcategory";  //"CategoryCode"
		protected const string HisTab_Expedited = "Expedited";
		protected const string HisTab_UpdatedBy = "AssignedClosure";
		//protected const string HisTab_DateUpdated = "";
		protected const string HisTab_Reason = "ResolutionCategory";
		protected const string HisTab_NotifiedOnOption1 = "DateComplete";
		protected const string HisTab_NotifiedOnOption2 = "WrittenNotificationDate";
		protected const string HisTab_Explanation = "Resolution";
		protected const string HisTab_ProviderNPI = "Provider1NPI";
		protected const string HisTab_ProviderName = "Provider1Name";
		protected const string HisTab_ProviderPhone = "Provider1Phone";
		protected const string LinkedTab_Type = "LinkType";
		protected const string LinkedTab_CallID = "CallID";
		protected const string LinkedTab_LinkReason = "LinkReason";
		protected const string Admin_AdminCat1 = "AdminCat1";   //AdminCat1Code
		protected const string Admin_AdminCat2 = "AdminCat2";
		protected const string AllNotes = "AllNotes";

		// Used to determine a particular data point to use.  Not in the response.
		protected const string OralResolution = "OralResolution";
		
		protected GrievancesManagerBase(
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

			if (results == null  || !results.Results.Any())
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
							//CaseLevel = GetAttributeValue(o, CaseLevel).ToString(),
							DateReceived = ToDateTime(GetAttributeValue(o, DateReceived)),
							CaseDueDate = ToDateTime(GetAttributeValue(o, CaseDueDateTime)),
							CategoryCode = GetAttributeValue(o, AppealCategoryCode)
								.ToString(),
							Type = GetAttributeValue(o, CaseType)
								.ToString(),
							User = GetAttributeValue(o, CurrentAssignment)
								.ToString(),
							IssueDescription = GetAttributeValue(o, Issuedescription)
								.ToString(),
							Member = GetMember(o.DisplayAttributes),
							ProviderId = GetAttributeValue(o, ProviderID)
								.ToString(),
							SubmissionMethod = GetAttributeValue(o, ReceiptMethod)
								.ToString(),
							Decision = GetAttributeValue(o, Decision)
								.ToString(),
							DateClosed = ToDateTime(GetAttributeValue(o, DateComplete)),
							GrvId = GetAttributeValue(o, GrvId)
								.ToString(),

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
				//CaseLevel = GetAttributeValue(result, CaseLevel).ToString(),
				DateReceived = ToDateTime(GetAttributeValue(result, DateReceived)),
				CaseDueDate = ToDateTime(GetAttributeValue(result, CaseDueDateTime)),
				CategoryCode = GetAttributeValue(result, AppealCategoryCode)
					.ToString(),
				Type = GetAttributeValue(result, CaseType)
					.ToString(),
				User = GetAttributeValue(result, CurrentAssignment)
					.ToString(),
				IssueDescription = GetAttributeValue(result, Issuedescription)
					.ToString(),
				Member = GetMember(result.DisplayAttributes),
				ProviderId = GetAttributeValue(result, ProviderID)
					.ToString(),
				SubmissionMethod = GetAttributeValue(result, ReceiptMethod)
					.ToString(),
				Decision = GetAttributeValue(result, Decision)
					.ToString(),
				DateClosed = ToDateTime(GetAttributeValue(result, DateComplete)),
				GrvId = GetAttributeValue(result, GrvId)
					.ToString(),
				SubType = GetAttributeValue(result, HisTab_SubType)
				.ToString(),
				DecisionDate = ToDateTime(GetAttributeValue(result, DateComplete)),
				Expedited = GetAttributeValue(result, HisTab_Expedited)
					.ToString(),
				UpdatedBy = GetAttributeValue(result, HisTab_UpdatedBy)
					.ToString(),
				//Reason = GetAttributeValue(result, HisTab_Reason).ToString() == string.Empty 
				//	? "03 - Grievance Resolved" : GetReasonDescription(GetAttributeValue(result, HisTab_Reason)
				//		.ToString()),
				DateNotifiedOn = ToDateTime(GetAttributeValue(result, HisTab_NotifiedOnOption2)),
				Explanation = GetAttributeValue(result, HisTab_Explanation)
					.ToString(),
				ProviderNPI = GetAttributeValue(result, HisTab_ProviderNPI)
					.ToString(),
				ProviderName = GetAttributeValue(result, HisTab_ProviderName)
					.ToString(),
				ProviderPhone= GetAttributeValue(result, HisTab_ProviderPhone)
					.ToString(),
				NextReviewDate = ToDateTime(GetAttributeValue(result, CaseDueDateTime)),
				Initiated = ToDateTime(GetAttributeValue(result, DateReceived)),
				DecisionCode = GetAttributeValue(result, Decision)
					.ToString(),
				StatusDate = ToDateTime(GetAttributeValue(result, DateComplete)),
				PrimaryUser = GetAttributeValue(result, CurrentAssignment)
					.ToString(),
				LinkType = GetAttributeValue(result, LinkedTab_Type)
					.ToString(),
				CallId = GetAttributeValue(result, LinkedTab_CallID)
					.ToString(),
				LinkReason = GetAttributeValue(result, LinkedTab_LinkReason)
					.ToString(),
				AdminCat1 = GetAttributeValue(result, Admin_AdminCat1)
					.ToString(),
				AdminCat2 = GetAttributeValue(result, Admin_AdminCat2)
					.ToString(),
				AllNotes = (List<AllNotes>)GetAttributeValue(result, AllNotes),

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

			//if (!RequestData.DisplayColumns.Any(f => f.Equals(CaseLevel)))
			//{
			//	displayColumns.Add(CaseLevel);
			//}

			if (!RequestData.DisplayColumns.Any(f => f.Equals(DateReceived)))
			{
				displayColumns.Add(DateReceived);
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

			if (!RequestData.DisplayColumns.Any(f => f.Equals(Issuedescription)))
			{
				displayColumns.Add(Issuedescription);
			}

			if (!RequestData.DisplayColumns.Any(f => f.Equals(MemberLast)))
			{
				displayColumns.Add(MemberLast);
			}

			if (!RequestData.DisplayColumns.Any(f => f.Equals(MemberFirst)))
			{
				displayColumns.Add(MemberFirst);
			}

			if (!RequestData.DisplayColumns.Any(f => f.Equals(MemberMiddleInitial)))
			{
				displayColumns.Add(MemberMiddleInitial);
			}

			if (!RequestData.DisplayColumns.Any(f => f.Equals(MemberID)))
			{
				displayColumns.Add(MemberID);
			}

			//if (!RequestData.DisplayColumns.Any(f => f.Equals("RClaimNumber")))
			//{
			//	displayColumns.Add("RClaimNumber");
			//}

			if (!RequestData.DisplayColumns.Any(f => f.Equals(ProviderID)))
			{
				displayColumns.Add(ProviderID);
			}

			if (!RequestData.DisplayColumns.Any(f => f.Equals(ReceiptMethod)))
			{
				displayColumns.Add(ReceiptMethod);
			}

			if (!RequestData.DisplayColumns.Any(f => f.Equals(Decision)))
			{
				displayColumns.Add(Decision);
			}

			if (!RequestData.DisplayColumns.Any(f => f.Equals(DateComplete)))
			{
				displayColumns.Add(DateComplete);
			}

			if (!RequestData.DisplayColumns.Any(f => f.Equals(GrvId)))
			{
				displayColumns.Add(GrvId);
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

			//if (!RequestAppealData.DisplayColumns.Any(f => f.Equals(CaseLevel)))
			//{
			//	displayColumns.Add(CaseLevel);
			//}

			if (!RequestAppealData.DisplayColumns.Any(f => f.Equals(DateReceived)))
			{
				displayColumns.Add(DateReceived);
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

			if (!RequestAppealData.DisplayColumns.Any(f => f.Equals(Issuedescription)))
			{
				displayColumns.Add(Issuedescription);
			}

			if (!RequestAppealData.DisplayColumns.Any(f => f.Equals(MemberLast)))
			{
				displayColumns.Add(MemberLast);
			}

			if (!RequestAppealData.DisplayColumns.Any(f => f.Equals(MemberFirst)))
			{
				displayColumns.Add(MemberFirst);
			}

			if (!RequestAppealData.DisplayColumns.Any(f => f.Equals(MemberMiddleInitial)))
			{
				displayColumns.Add(MemberMiddleInitial);
			}

			if (!RequestAppealData.DisplayColumns.Any(f => f.Equals(MemberID)))
			{
				displayColumns.Add(MemberID);
			}

			//if (!RequestAppealData.DisplayColumns.Any(f => f.Equals("RClaimNumber")))
			//{
			//	displayColumns.Add("RClaimNumber");
			//}

			if (!RequestAppealData.DisplayColumns.Any(f => f.Equals(ProviderID)))
			{
				displayColumns.Add(ProviderID);
			}

			if (!RequestAppealData.DisplayColumns.Any(f => f.Equals(ReceiptMethod)))
			{
				displayColumns.Add(ReceiptMethod);
			}

			if (!RequestAppealData.DisplayColumns.Any(f => f.Equals(Decision)))
			{
				displayColumns.Add(Decision);
			}

			if (!RequestAppealData.DisplayColumns.Any(f => f.Equals(DateComplete)))
			{
				displayColumns.Add(DateComplete);
			}

			if (!RequestAppealData.DisplayColumns.Any(f => f.Equals(GrvId)))
			{
				displayColumns.Add(GrvId);
			}

			if (!RequestAppealData.DisplayColumns.Any(f => f.Equals(HisTab_SubType)))
			{
				displayColumns.Add(HisTab_SubType);
			}

			if (!RequestAppealData.DisplayColumns.Any(f => f.Equals(HisTab_Expedited)))
			{
				displayColumns.Add(HisTab_Expedited);
			}

			if (!RequestAppealData.DisplayColumns.Any(f => f.Equals(HisTab_UpdatedBy)))
			{
				displayColumns.Add(HisTab_UpdatedBy);
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

			if (!RequestAppealData.DisplayColumns.Any(f => f.Equals(LinkedTab_Type)))
			{
				displayColumns.Add(LinkedTab_Type);
			}

			if (!RequestAppealData.DisplayColumns.Any(f => f.Equals(LinkedTab_CallID)))
			{
				displayColumns.Add(LinkedTab_CallID);
			}

			if (!RequestAppealData.DisplayColumns.Any(f => f.Equals(LinkedTab_LinkReason)))
			{
				displayColumns.Add(LinkedTab_LinkReason);
			}

			if (!RequestAppealData.DisplayColumns.Any(f => f.Equals(Admin_AdminCat1)))
			{
				displayColumns.Add(Admin_AdminCat1);
			}

			if (!RequestAppealData.DisplayColumns.Any(f => f.Equals(Admin_AdminCat2)))
			{
				displayColumns.Add(Admin_AdminCat2);
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
					.FirstOrDefault(attribute => attribute.Key.Equals(MemberLast))
					.Value.ToString(),

				FirstName = inputDisplayAttributes.FirstOrDefault(
						attribute => attribute.Key.Equals(MemberFirst))
					.Value.ToString(),

				MiddleInitial = inputDisplayAttributes.FirstOrDefault(
						attribute => attribute.Key.Equals(MemberMiddleInitial))
					.Value.ToString(),

				Id = inputDisplayAttributes
					.FirstOrDefault(attribute => attribute.Key.Equals(MemberID))
					.Value.ToString()
			};

		protected Dictionary<string, object> GetDisplayColumns(IDictionary<string, object> columns)
		{
			string[] appealsFields =
			{
				FacetsGrievanceID,
				Status,
				//CaseLevel,
				DateReceived,
				CaseDueDateTime,
				AppealCategoryCode,
				CaseType,
				CurrentAssignment,
				Issuedescription,
				MemberLast,
				MemberFirst,
				MemberMiddleInitial,
				MemberID,
				ProviderID,
				ReceiptMethod,
				Decision,
				DateComplete,
				GrvId
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
				DateReceived,
				CaseDueDateTime,
				AppealCategoryCode,
				CaseType,
				CurrentAssignment,
				Issuedescription,
				MemberLast,
				MemberFirst,
				MemberMiddleInitial,
				MemberID,
				ProviderID,
				ReceiptMethod,
				Decision,
				DateComplete,
				GrvId,
				HisTab_SubType,
				DateComplete,
				HisTab_Expedited,
				HisTab_UpdatedBy,
				HisTab_Reason,
				HisTab_NotifiedOnOption2,
				HisTab_Explanation,
				HisTab_ProviderNPI,
				HisTab_ProviderName,
				HisTab_ProviderPhone,
				CaseDueDateTime,
				DateReceived,
				Decision,
				DateComplete,
				CurrentAssignment,
				LinkedTab_Type,
				LinkedTab_CallID,
				LinkedTab_LinkReason,
				Admin_AdminCat1,
				Admin_AdminCat2,
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
				case "03":
					reasonDescription = $"{reasonCode} - Grievance Resolved";
					break;

				case "101":
					reasonDescription = $"{reasonCode} - Hold for future use";
					break;

				case "102":
					reasonDescription = $"{reasonCode} - Hold for future use";
					break;

				case "103":
					reasonDescription = $"{reasonCode} - Enrollee Information regarding Comprehensive Primary Care (CPC)";
					break;

				case "104":
					reasonDescription = $"{reasonCode} - Enrollee Information regarding health home services (i.e. grievance issue is related to Health Home/Health Home Services)";
					break;

				case "105":
					reasonDescription = $"{reasonCode} - Enrollee Information for grievance issues not related to Health Home/Health Home Servies";
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