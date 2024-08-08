// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   CreateGrievanceAppeals
//   GrievanceAppealManager.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.CreateGrievanceAppeals.Manager
{
	using System;
	using CareSource.WC.OnBase.Core.Diagnostics.Interfaces;
	using CreateGrievanceAppeals.Models;
	using CreateGrievanceAppeals.Models.Data;
	using Hyland.Unity;
	using Hyland.Unity.UnityForm;
	using Application = Hyland.Unity.Application;

	public class GrievanceAppealManager
	{
		public GrievanceAppealManager(
			ILogger logger)
		{
			_logger = logger;
		}

		private readonly ILogger _logger;

		//******************************************************************

		const string FacetsGrievanceId_Keyword = "Facets Grievance ID";
		const string DisputeId_Keyword = "Dispute ID";

		const string Grievance_Utility_Forms = "Grievance Utility Forms";
		const string MA_Utility_Unity_Form = "MA Utility Unity Form";

		const string PAPL_Provider_Appeals_Submission_Unity_Form =
			"PAPL Provider Appeals Submission Unity Form";

		const string PAPL_Provider_Dispute_Submission_Unity_Form =
			"PAPL Provider Dispute Submission Unity Form";

		//******************************************************************

		public GrievanceAppealsResult CreateGrievanceAppeals(
			GrievanceAppealsData grievanceAppealsData,
			OnBaseConnectionParameters connParams)
		{

		GrievanceAppealsResult grievanceAppealsResult = new GrievanceAppealsResult();
			string loggerInit = $@"{grievanceAppealsData.ApplicationName}{"/"}{grievanceAppealsData.ESBGUID}";

			Application app = null;
			StoreNewUnityFormProperties eFormProps = null;
			string erMessage = "";

			try
			{
				_logger.LogInfo($@"{loggerInit} {"Into GrievanceAppealManager."}");

				// Connect to OnBase
				app = ConnectApplication(connParams);
				if (null == app)
				{
					erMessage = "Error: Connection not made to OnBase.";
					_logger.LogError(
						$@"{loggerInit} {erMessage}");

					grievanceAppealsResult.Message = erMessage;
					grievanceAppealsResult.Status = "FAILURE";
					return grievanceAppealsResult;
				}

				_logger.LogInfo(
					$@"{loggerInit} {"API Connected."}");

				// Is call from Streamline, Member Portal, NCentarus?
				if (grievanceAppealsData.ApplicationName.ToUpper() == "SL" ||
				    grievanceAppealsData.ApplicationName.ToUpper() == "MP" ||
				    grievanceAppealsData.ApplicationName.ToUpper() == "NC") //  Ncentaurus 
				{
					_logger.LogInfo(
						$@"{loggerInit} {"Application: SL/MP/NC"}");
					if (grievanceAppealsData.CallType.ToUpper() == "GRIEVANCE")
					{
						//******************************************************

						if (CheckforExistingDocument(
							app,
							Grievance_Utility_Forms,
							FacetsGrievanceId_Keyword,
							grievanceAppealsData.FacetsGrievanceId))
						{
							erMessage = "Grievance Already Exists";
							_logger.LogInfo(
								$"{loggerInit} {erMessage}");

							grievanceAppealsResult.Message = erMessage;
							grievanceAppealsResult.Status = "DUPLICATE";
							return grievanceAppealsResult;
						}

						//******************************************************

						// Grievance Unity Forms
						FormTemplate template1 =
							app.Core.UnityFormTemplates.Find("AG - Grievance Streamline API Form II");

						if (null == template1)
						{
							erMessage = "Error: Could not retrieve the Unity form template for Grievance.";
							_logger.LogError(
								$@"{loggerInit} {erMessage}");

							grievanceAppealsResult.Message = erMessage;
							grievanceAppealsResult.Status = "FAILURE";
							return grievanceAppealsResult;
						}

						_logger.LogInfo(
							$@"{loggerInit} {"Found AG - Grievance Streamline API Form II"}");

						eFormProps = app.Core.Storage.CreateStoreNewUnityFormProperties(template1);

						if (null == eFormProps)
						{
							erMessage = "Error: Failed to create the eFormProps Object.";
							_logger.LogError(
								$@"{loggerInit} {erMessage}");

							grievanceAppealsResult.Message = erMessage;
							grievanceAppealsResult.Status = "FAILURE";
							return grievanceAppealsResult;
						}

						_logger.LogInfo(
							$@"{loggerInit} {"Created eFormProps Object."}");

						AddGrievanceKeywords(
							eFormProps,
							grievanceAppealsData,
							loggerInit);
					}
					else if (grievanceAppealsData.CallType.ToUpper() == "APPEAL")
					{
						//******************************************************

						if (CheckforExistingDocument(
							app,
							MA_Utility_Unity_Form,
							FacetsGrievanceId_Keyword,
							grievanceAppealsData.FacetsGrievanceId))
						{
							erMessage = "Appeal Already Exists";
							_logger.LogInfo(
								$"{loggerInit} {erMessage}");

							grievanceAppealsResult.Message = erMessage;
							grievanceAppealsResult.Status = "DUPLICATE";
							return grievanceAppealsResult;
						}

						//******************************************************

						FormTemplate template1 =
							app.Core.UnityFormTemplates.Find("MA Utility Unity Form");

						if (null == template1)
						{
							erMessage = "Error: Could not retrieve the Unity form template for Appeal.";
							_logger.LogError(
								$@"{loggerInit} {erMessage}");

							grievanceAppealsResult.Message = erMessage;
							grievanceAppealsResult.Status = "FAILURE";
							return grievanceAppealsResult;
						}

						_logger.LogInfo(
							$@"{loggerInit} {"Found MA Utility Unity Form"}");

						eFormProps = app.Core.Storage.CreateStoreNewUnityFormProperties(template1);

						if (null == eFormProps)
						{
							erMessage = "Error: Failed to create the eFormProps Object.";
							_logger.LogError(
								$@"{loggerInit} {erMessage}");
							grievanceAppealsResult.Message = erMessage;
							grievanceAppealsResult.Status = "FAILURE";
							return grievanceAppealsResult;
						}

						_logger.LogInfo(
							$@"{loggerInit} {"Created eFormProps Object."}");

						AddAppealKeywords(
							eFormProps,
							grievanceAppealsData,
							loggerInit);
					}
					else
					{
						erMessage =
							"Error: The specified CallType is invalid. Please provide Grievance or Appeal.";
						_logger.LogError(
							$@"{loggerInit} {erMessage}");
						grievanceAppealsResult.Message = erMessage;
						grievanceAppealsResult.Status = "FAILURE";
						return grievanceAppealsResult;
					}

					grievanceAppealsResult.ESBGuid = grievanceAppealsData.ESBGUID;

					Document newUnityForm = app.Core.Storage.StoreNewUnityForm(eFormProps);

					if (null == newUnityForm)
					{
						erMessage = $@"{loggerInit} Error in sending ESBGuid: {grievanceAppealsData.ESBGUID}
							for {grievanceAppealsData.CallType}: {grievanceAppealsData.FacetsGrievanceId}.";
						_logger.LogError(
							$@"{loggerInit} {erMessage}");

						grievanceAppealsResult.Message = erMessage;
						grievanceAppealsResult.Status = "FAILURE";
					}
					else
					{
						erMessage = $@"{loggerInit} Success in sending ESBGuid: {grievanceAppealsData.ESBGUID} for {grievanceAppealsData.CallType}: {grievanceAppealsData.FacetsGrievanceId}.";
						_logger.LogInfo(
							$@"{loggerInit} {erMessage}");

						grievanceAppealsResult.DocumentId = newUnityForm.ID.ToString();
						grievanceAppealsResult.Message = erMessage;
						grievanceAppealsResult.Status = "SUCCESS";
					}
				}
				else if (grievanceAppealsData.ApplicationName.ToUpper() == "PP") // Provider Portal
				{
					// Provider Portal Grievance - Currently pulled through the Grievance - Streamline Processing workflow.
					if (grievanceAppealsData.CallType.ToUpper() == "GRIEVANCE")
					{
						_logger.LogInfo(
							$@"{loggerInit} {"Application: PP Grievance"}");

						//******************************************************

						if (CheckforExistingDocument(
							app,
							Grievance_Utility_Forms,
							FacetsGrievanceId_Keyword,
							grievanceAppealsData.FacetsGrievanceId))
						{
							erMessage = "Grievance Already Exists";
							_logger.LogInfo(
								$"{loggerInit} {erMessage}");

							grievanceAppealsResult.Message = erMessage;
							grievanceAppealsResult.Status = "DUPLICATE";
							return grievanceAppealsResult;
						}

						//******************************************************

						FormTemplate template1 =
							app.Core.UnityFormTemplates.Find("AG - Grievance Streamline API Form II");

						if (null == template1)
						{
							erMessage =
								"Error: Could not retrieve the Unity form template for PP Grievances.";
							_logger.LogError(
								$@"{loggerInit} {erMessage}");
							grievanceAppealsResult.Message = erMessage;
							grievanceAppealsResult.Status = "FAILURE";
							return grievanceAppealsResult;
						}

						_logger.LogInfo(
							$@"{loggerInit} {"Found AG - Grievance Streamline API Form II"}");

						eFormProps = app.Core.Storage.CreateStoreNewUnityFormProperties(template1);

						if (null == eFormProps)
						{
							erMessage = "Error: Failed to create the eFormProps Object.";
							_logger.LogError(
								$@"{loggerInit} {erMessage}");

							grievanceAppealsResult.Message = erMessage;
							grievanceAppealsResult.Status = "FAILURE";
							return grievanceAppealsResult;
						}

						_logger.LogInfo(
							$@"{loggerInit} {"Created eFormProps Object."}");

						AddGrievanceKeywords(
							eFormProps,
							grievanceAppealsData,
							loggerInit);
					}
					else if (grievanceAppealsData.CallType.ToUpper() == "APPEAL")
					{
						_logger.LogInfo(
							$@"{loggerInit} {"Application: PP Appeal"}");

						//******************************************************

						if (CheckforExistingDocument(
							app,
							PAPL_Provider_Appeals_Submission_Unity_Form,
							FacetsGrievanceId_Keyword,
							grievanceAppealsData.FacetsGrievanceId))
						{
							erMessage = "Appeal Already Exists";
							_logger.LogInfo(
								$"{loggerInit} {erMessage}");

							grievanceAppealsResult.Message = erMessage;
							grievanceAppealsResult.Status = "DUPLICATE";
							return grievanceAppealsResult;
						}

						//******************************************************

						FormTemplate template1 =
							app.Core.UnityFormTemplates.Find(
								"PAPL Provider Appeals Submission Unity Form");

						if (null == template1)
						{
							erMessage =
								"Error: Could not retrieve the Unity form template for PP Appeals.";
							_logger.LogError(
								$@"{loggerInit} {erMessage}");

							grievanceAppealsResult.Message = erMessage;
							grievanceAppealsResult.Status = "FAILURE";
							return grievanceAppealsResult;
						}

						_logger.LogInfo(
							$@"{loggerInit} {"Found PAPL Provider Appeals Submission Unity Form"}");

						eFormProps = app.Core.Storage.CreateStoreNewUnityFormProperties(template1);

						if (null == eFormProps)
						{
							erMessage = "Error: Failed to create the eFormProps Object.";
							_logger.LogError(
								$@"{loggerInit} {erMessage}");

							grievanceAppealsResult.Message = erMessage;
							grievanceAppealsResult.Status = "FAILURE";
							return grievanceAppealsResult;
						}

						_logger.LogInfo(
							$@"{loggerInit} {"Created eFormProps Object."}");

						if (!AddPPAppealKeywords(
							eFormProps,
							grievanceAppealsData,
							loggerInit))
						{
							erMessage = "Error: Adding PP Appeal Keywords.";
							_logger.LogError(
								$@"{loggerInit} {erMessage}");

							grievanceAppealsResult.Message = erMessage;
							grievanceAppealsResult.Status = "FAILURE";
							return grievanceAppealsResult;
						}

					}
					// Provider Portal Disputes
					else if (grievanceAppealsData.CallType.ToUpper() == "DISPUTE")
					{
						_logger.LogInfo(
							$@"{loggerInit} {"Application: PP Dispute"}");


						//******************************************************

						if (CheckforExistingDocument(
							app,
							PAPL_Provider_Dispute_Submission_Unity_Form,
							DisputeId_Keyword,
							grievanceAppealsData.PPDisputeId))
						{
							erMessage = "Dispute Already Exists";
							_logger.LogInfo(
								$"{loggerInit} {erMessage}");

							grievanceAppealsResult.Message = erMessage;
							grievanceAppealsResult.Status = "DUPLICATE";
							return grievanceAppealsResult;
						}

						//******************************************************

						FormTemplate template1 =
							app.Core.UnityFormTemplates.Find(
								"PAPL Provider Dispute Submission Unity Form");


						if (null == template1)
						{
							erMessage =
								"Error: Could not retrieve the Unity form template for PP Disputes";
							_logger.LogError(
								$@"{loggerInit} {erMessage}");

							grievanceAppealsResult.Message = erMessage;
							grievanceAppealsResult.Status = "FAILURE";
							return grievanceAppealsResult;
						}

						_logger.LogInfo(
							$@"{loggerInit} {"Found PAPL Provider Dispute Submission Unity Form"}");

						eFormProps = app.Core.Storage.CreateStoreNewUnityFormProperties(template1);

						if (null == eFormProps)
						{
							erMessage = "Error: Failed to create the eFormProps Object.";
							_logger.LogError(
								$@"{loggerInit} {erMessage}");

							grievanceAppealsResult.Message = erMessage;
							grievanceAppealsResult.Status = "FAILURE";
							return grievanceAppealsResult;
						}

						_logger.LogInfo(
							$@"{loggerInit} {"Created eFormProps Object."}");

						AddPPDisputeKeywords(
							eFormProps,
							grievanceAppealsData,
							loggerInit);
					}
					else
					{
						erMessage =
							"Error: The specified CallType is invalid. Please provide Grievance, Appeal or Dispute.";
						_logger.LogError(
							$@"{loggerInit} {erMessage}");
						grievanceAppealsResult.Message = erMessage;
						grievanceAppealsResult.Status = "FAILURE";
					}

					grievanceAppealsResult.ESBGuid = grievanceAppealsData.ESBGUID;

					Document newUnityForm = app.Core.Storage.StoreNewUnityForm(eFormProps);

					if (null == newUnityForm)
					{
						if (grievanceAppealsData.CallType.ToUpper() == "DISPUTE")
						{
							erMessage = $@"{loggerInit} Error in sending ESBGuid: {grievanceAppealsData.ESBGUID} for {grievanceAppealsData.CallType}: {grievanceAppealsData.PPDisputeId}.";
						}
						else
						{
							erMessage = $@"{loggerInit} Error in sending ESBGuid: {grievanceAppealsData.ESBGUID} for {grievanceAppealsData.CallType}: {grievanceAppealsData.FacetsGrievanceId}.";
						}

						_logger.LogError(
							$@"{loggerInit} {erMessage}");

						grievanceAppealsResult.Message = erMessage;
						grievanceAppealsResult.Status = "FAILURE";
						return grievanceAppealsResult;
					}
					else
					{
						if (grievanceAppealsData.CallType.ToUpper() == "DISPUTE")
						{
							erMessage = $@"{loggerInit} Success in sending ESBGuid: {grievanceAppealsData.ESBGUID} for {grievanceAppealsData.CallType}: {grievanceAppealsData.PPDisputeId}.";
						}
						else
						{
							erMessage = $@"{loggerInit} Success in sending ESBGuid: {grievanceAppealsData.ESBGUID} for {grievanceAppealsData.CallType}: {grievanceAppealsData.FacetsGrievanceId}.";
						}

						_logger.LogInfo(
							$@"{loggerInit} {erMessage}");
						grievanceAppealsResult.DocumentId = newUnityForm.ID.ToString();

						grievanceAppealsResult.Message = erMessage;
						grievanceAppealsResult.Status = "SUCCESS";
					}
				}
				//****************************
				else
				{
					_logger.LogError(
						loggerInit +
						"Error: The specified Application Name is invalid. Please provide SL or MP or PP.");

					erMessage =
						"Error: The specified Application Name is invalid. Please provide SL or MP or PP.";
					_logger.LogError(
						$@"{loggerInit} {erMessage}");

					grievanceAppealsResult.Message = erMessage;
					grievanceAppealsResult.Status = "FAILURE";
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(
					$@"{loggerInit} {ex} ");
				throw ex;
			}
			finally
			{
				if (null != app)
				{
					DisconnectApplication(app);
					_logger.LogInfo(
						$@"{loggerInit} {"API Disconnected from OnBase..."} ");
					_logger.LogInfo(
						$@"{"==============================================================================================================="}");
				}
			}

			return grievanceAppealsResult;
		}

		private static bool CheckforExistingDocument(
			Application app,
			string documentType,
			string searchKeyword,
			string searchId)
		{
			DocumentType docType = app.Core.DocumentTypes.Find(documentType);

			DocumentQuery documentQuery = app.Core.CreateDocumentQuery();

			documentQuery.AddDocumentType(docType);

			documentQuery.AddKeyword(
				searchKeyword,
				searchId);

			return documentQuery.ExecuteCount() > 0;
		}

		private  bool AddGrievanceKeywords(
			StoreNewUnityFormProperties eFormProps,
			GrievanceAppealsData grievanceAppealsData,
			string loggerInit)
		{
			// Grievance
			try
			{
				if (!string.IsNullOrEmpty(grievanceAppealsData.AdminCat1))
				{
					eFormProps.AddKeyword(
						"Admin Cat 1",
						grievanceAppealsData.AdminCat1);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.AdminCat2))
				{
					eFormProps.AddKeyword(
						"Admin Cat 2",
						grievanceAppealsData.AdminCat2);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.AOR))
				{
					eFormProps.AddKeyword(
						"AOR Exists",
						grievanceAppealsData.AOR);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.AORExpires))
				{
					eFormProps.AddKeyword(
						"AOR Expires",
						Convert.ToDateTime(grievanceAppealsData.AORExpires));
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.AORRepresentativeCity))
				{
					eFormProps.AddKeyword(
						"Representative City",
						grievanceAppealsData.AORRepresentativeCity);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.AORRepresentativeEmail))
				{
					eFormProps.AddKeyword(
						"Representative Email",
						grievanceAppealsData.AORRepresentativeEmail);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.AORRepresentativeFirst))
				{
					eFormProps.AddKeyword(
						"Representative First",
						grievanceAppealsData.AORRepresentativeFirst);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.AORRepresentativeLast))
				{
					eFormProps.AddKeyword(
						"Representative Last",
						grievanceAppealsData.AORRepresentativeLast);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.AORRepresentativePhone))
				{
					eFormProps.AddKeyword(
						"Representative Phone",
						grievanceAppealsData.AORRepresentativePhone);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.AORRepresentativeState))
				{
					eFormProps.AddKeyword(
						"Representative State",
						grievanceAppealsData.AORRepresentativeState);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.AORRepresentativeStreet))
				{
					eFormProps.AddKeyword(
						"Representative Street",
						grievanceAppealsData.AORRepresentativeStreet);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.AORRepresentativeZIP))
				{
					eFormProps.AddKeyword(
						"Representative Zip",
						grievanceAppealsData.AORRepresentativeZIP);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.ApplicationName))
				{
					eFormProps.AddKeyword(
						"Application Name",
						grievanceAppealsData.ApplicationName);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.AutoClose))
				{
					eFormProps.AddKeyword(
						"Auto-Close",
						grievanceAppealsData.AutoClose);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.CallerStatus))
				{
					eFormProps.AddKeyword(
						"Caller Status",
						grievanceAppealsData.CallerStatus);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.CallInMethod))
				{
					eFormProps.AddKeyword(
						"Reciept Type",
						grievanceAppealsData.CallInMethod);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.CallType))
				{
					eFormProps.AddKeyword(
						"Call Type",
						grievanceAppealsData.CallType);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.Category))
				{
					eFormProps.AddKeyword(
						"Category",
						grievanceAppealsData.Category);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.CategoryCode))
				{
					eFormProps.AddKeyword(
						"Category Code",
						grievanceAppealsData.CategoryCode);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.CurrentUserName))
				{
					eFormProps.AddKeyword(
						"Customer Advocate",
						grievanceAppealsData.CurrentUserName);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.CustomerType))
				{
					eFormProps.AddKeyword(
						"Caller",
						grievanceAppealsData.CustomerType);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.DateComplete))
				{
					eFormProps.AddKeyword(
						"Date Complete",
						Convert.ToDateTime(grievanceAppealsData.DateComplete));
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.Decision))
				{
					eFormProps.AddKeyword(
						"Decision",
						grievanceAppealsData.Decision);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.Description))
				{
					eFormProps.AddKeyword(
						"Issue Description",
						grievanceAppealsData.Description); //.Replace("\"", "\\\""));
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.EnrollmentEndDate))
				{
					eFormProps.AddKeyword(
						"Coverage End",
						Convert.ToDateTime(grievanceAppealsData.EnrollmentEndDate));
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.EnrollmentStartDate))
				{
					eFormProps.AddKeyword(
						"Coverage Start",
						Convert.ToDateTime(grievanceAppealsData.EnrollmentStartDate));
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.ESBGUID))
				{
					eFormProps.AddKeyword(
						"ESB GUID",
						grievanceAppealsData.ESBGUID);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.FacetsGrievanceId))
				{
					eFormProps.AddKeyword(
						"Facets Grievance ID",
						grievanceAppealsData.FacetsGrievanceId);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.GroupName))
				{
					eFormProps.AddKeyword(
						"Case Business Unit",
						grievanceAppealsData.GroupName);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.HICN))
				{
					eFormProps.AddKeyword(
						"Member HICN",
						grievanceAppealsData.HICN);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.InitiationDate))
				{
					eFormProps.AddKeyword(
						"Occurrence Date",
						Convert.ToDateTime(grievanceAppealsData.InitiationDate));
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.LinkReason))
				{
					eFormProps.AddKeyword(
						"Link Reason",
						grievanceAppealsData.LinkReason);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.LinkType))
				{
					eFormProps.AddKeyword(
						"Link Type",
						grievanceAppealsData.LinkType);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.MedicaidId))
				{
					eFormProps.AddKeyword(
						"Medicaid Number",
						grievanceAppealsData.MedicaidId);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.MemberCity))
				{
					eFormProps.AddKeyword(
						"Member City",
						grievanceAppealsData.MemberCity);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.MemberContactPhone))
				{
					eFormProps.AddKeyword(
						"Member Phone",
						grievanceAppealsData.MemberContactPhone);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.MemberDOB))
				{
					eFormProps.AddKeyword(
						"AC: Patient Date of Birth",
						Convert.ToDateTime(grievanceAppealsData.MemberDOB));
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.MemberEMail))
				{
					eFormProps.AddKeyword(
						"Member E-Mail",
						grievanceAppealsData.MemberEMail);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.MemberFirst))
				{
					eFormProps.AddKeyword(
						"Member First Name",
						grievanceAppealsData.MemberFirst);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.MemberId))
				{
					eFormProps.AddKeyword(
						"WV Member ID",
						grievanceAppealsData.MemberId);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.MemberLast))
				{
					eFormProps.AddKeyword(
						"Member Last Name",
						grievanceAppealsData.MemberLast);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.MemberState))
				{
					eFormProps.AddKeyword(
						"Member State",
						grievanceAppealsData.MemberState);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.MemberStreet))
				{
					eFormProps.AddKeyword(
						"Member Street",
						grievanceAppealsData.MemberStreet);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.MemberZIP))
				{
					eFormProps.AddKeyword(
						"Member ZIP",
						grievanceAppealsData.MemberZIP);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.Notes1))
				{
					eFormProps.AddKeyword(
						"Notes1",
						grievanceAppealsData.Notes1); //.Replace("\"", "\\\""));
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.Notes2))
				{
					eFormProps.AddKeyword(
						"Notes2",
						grievanceAppealsData.Notes2); //.Replace("\"", "\\\""));
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.Notes3))
				{
					eFormProps.AddKeyword(
						"Notes3",
						grievanceAppealsData.Notes3); //.Replace("\"", "\\\""));
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.Notes4))
				{
					eFormProps.AddKeyword(
						"Notes4",
						grievanceAppealsData.Notes4); //.Replace("\"", "\\\""));
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.Notes5))
				{
					eFormProps.AddKeyword(
						"Notes5",
						grievanceAppealsData.Notes5); //.Replace("\"", "\\\""));
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.Notes6))
				{
					eFormProps.AddKeyword(
						"Notes6",
						grievanceAppealsData.Notes6); //.Replace("\"", "\\\""));
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.Notes7))
				{
					eFormProps.AddKeyword(
						"Notes7",
						grievanceAppealsData.Notes7); //.Replace("\"", "\\\""));
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.Notes8))
				{
					eFormProps.AddKeyword(
						"Notes8",
						grievanceAppealsData.Notes8); //.Replace("\"", "\\\""));
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.Notes9))
				{
					eFormProps.AddKeyword(
						"Notes9",
						grievanceAppealsData.Notes9); //.Replace("\"", "\\\""));
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.Notes10))
				{
					eFormProps.AddKeyword(
						"Notes10",
						grievanceAppealsData.Notes10); //.Replace("\"", "\\\""));
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.PlanId))
				{
					eFormProps.AddKeyword(
						"Contract ID",
						grievanceAppealsData.PlanId);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.PlanName))
				{
					eFormProps.AddKeyword(
						"Organization Name",
						grievanceAppealsData.PlanName);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.PlanStatus))
				{
					eFormProps.AddKeyword(
						"Plan Status",
						grievanceAppealsData.PlanStatus);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.Priority))
				{
					eFormProps.AddKeyword(
						"Expedited",
						grievanceAppealsData.Priority);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.ProviderCity))
				{
					eFormProps.AddKeyword(
						"Provider City",
						grievanceAppealsData.ProviderCity);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.ProviderEmail))
				{
					eFormProps.AddKeyword(
						"Provider Email",
						grievanceAppealsData.ProviderEmail);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.ProviderFourCity))
				{
					eFormProps.AddKeyword(
						"Provider Four City",
						grievanceAppealsData.ProviderFourCity);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.ProviderFourEmail))
				{
					eFormProps.AddKeyword(
						"Provider Four Email",
						grievanceAppealsData.ProviderFourEmail);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.ProviderFourId))
				{
					eFormProps.AddKeyword(
						"Provider Four ID",
						grievanceAppealsData.ProviderFourId);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.ProviderFourName))
				{
					eFormProps.AddKeyword(
						"Provider Four Name",
						grievanceAppealsData.ProviderFourName);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.ProviderFourNPI))
				{
					eFormProps.AddKeyword(
						"Provider Four NPI",
						grievanceAppealsData.ProviderFourNPI);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.ProviderFourPhone))
				{
					eFormProps.AddKeyword(
						"Provider Four Phone",
						grievanceAppealsData.ProviderFourPhone);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.ProviderFourState))
				{
					eFormProps.AddKeyword(
						"Provider Four State",
						grievanceAppealsData.ProviderFourState);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.ProviderFourStreet))
				{
					eFormProps.AddKeyword(
						"Provider Four Street",
						grievanceAppealsData.ProviderFourStreet);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.ProviderFourTaxId))
				{
					eFormProps.AddKeyword(
						"Provider Four TIN",
						grievanceAppealsData.ProviderFourTaxId);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.ProviderFourZIP))
				{
					eFormProps.AddKeyword(
						"Provider Four ZIP",
						grievanceAppealsData.ProviderFourZIP);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.ProviderName))
				{
					eFormProps.AddKeyword(
						"WV Provider Name",
						grievanceAppealsData.ProviderName);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.ProviderNPI))
				{
					eFormProps.AddKeyword(
						"WV Provider NPI",
						grievanceAppealsData.ProviderNPI);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.ProviderOneId))
				{
					eFormProps.AddKeyword(
						"Provider ID",
						grievanceAppealsData.ProviderOneId);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.ProviderPhone))
				{
					eFormProps.AddKeyword(
						"Provider Phone",
						grievanceAppealsData.ProviderPhone);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.ProviderState))
				{
					eFormProps.AddKeyword(
						"Provider State",
						grievanceAppealsData.ProviderState);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.ProviderStreet))
				{
					eFormProps.AddKeyword(
						"Provider Street",
						grievanceAppealsData.ProviderStreet);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.ProviderThreeCity))
				{
					eFormProps.AddKeyword(
						"Provider Three City",
						grievanceAppealsData.ProviderThreeCity);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.ProviderThreeEmail))
				{
					eFormProps.AddKeyword(
						"Provider Three Email",
						grievanceAppealsData.ProviderThreeEmail);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.ProviderThreeId))
				{
					eFormProps.AddKeyword(
						"Provider Three ID",
						grievanceAppealsData.ProviderThreeId);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.ProviderThreeName))
				{
					eFormProps.AddKeyword(
						"Provider Three Name",
						grievanceAppealsData.ProviderThreeName);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.ProviderThreeNPI))
				{
					eFormProps.AddKeyword(
						"Provider Three NPI",
						grievanceAppealsData.ProviderThreeNPI);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.ProviderThreePhone))
				{
					eFormProps.AddKeyword(
						"Provider Three Phone",
						grievanceAppealsData.ProviderThreePhone);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.ProviderThreeState))
				{
					eFormProps.AddKeyword(
						"Provider Three State",
						grievanceAppealsData.ProviderThreeState);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.ProviderThreeStreet))
				{
					eFormProps.AddKeyword(
						"Provider Three Street",
						grievanceAppealsData.ProviderThreeStreet);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.ProviderThreeTaxId))
				{
					eFormProps.AddKeyword(
						"Provider Three TIN",
						grievanceAppealsData.ProviderThreeTaxId);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.ProviderThreeZIP))
				{
					eFormProps.AddKeyword(
						"Provider Three ZIP",
						grievanceAppealsData.ProviderThreeZIP);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.ProviderTwoCity))
				{
					eFormProps.AddKeyword(
						"Provider Two City",
						grievanceAppealsData.ProviderTwoCity);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.ProviderTwoEmail))
				{
					eFormProps.AddKeyword(
						"Provider Two Email",
						grievanceAppealsData.ProviderTwoEmail);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.ProviderTwoId))
				{
					eFormProps.AddKeyword(
						"Provider Two ID",
						grievanceAppealsData.ProviderTwoId);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.ProviderTwoName))
				{
					eFormProps.AddKeyword(
						"Provider Two Name",
						grievanceAppealsData.ProviderTwoName);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.ProviderTwoNPI))
				{
					eFormProps.AddKeyword(
						"Provider Two NPI",
						grievanceAppealsData.ProviderTwoNPI);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.ProviderTwoPhone))
				{
					eFormProps.AddKeyword(
						"Provider Two Phone",
						grievanceAppealsData.ProviderTwoPhone);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.ProviderTwoState))
				{
					eFormProps.AddKeyword(
						"Provider Two State",
						grievanceAppealsData.ProviderTwoState);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.ProviderTwoStreet))
				{
					eFormProps.AddKeyword(
						"Provider Two Street",
						grievanceAppealsData.ProviderTwoStreet);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.ProviderTwoTaxId))
				{
					eFormProps.AddKeyword(
						"Provider Two TIN",
						grievanceAppealsData.ProviderTwoTaxId);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.ProviderTwoZIP))
				{
					eFormProps.AddKeyword(
						"Provider Two ZIP",
						grievanceAppealsData.ProviderTwoZIP);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.ProviderZIP))
				{
					eFormProps.AddKeyword(
						"Provider ZIP",
						grievanceAppealsData.ProviderZIP);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.Reason))
				{
					eFormProps.AddKeyword(
						"Reason",
						grievanceAppealsData.Reason);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.ReceivedDate))
				{
					eFormProps.AddKeyword(
						"Corporate Received Date Time",
						Convert.ToDateTime(grievanceAppealsData.ReceivedDate));
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.Resolution1))
				{
					eFormProps.AddKeyword(
						"Resolution1",
						grievanceAppealsData.Resolution1); //.Replace("\"", "\\\""));
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.Resolution2))
				{
					eFormProps.AddKeyword(
						"Resolution2",
						grievanceAppealsData.Resolution2); //.Replace("\"", "\\\""));
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.Resolution3))
				{
					eFormProps.AddKeyword(
						"Resolution3",
						grievanceAppealsData.Resolution3); //.Replace("\"", "\\\""));
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.Resolution4))
				{
					eFormProps.AddKeyword(
						"Resolution4",
						grievanceAppealsData.Resolution4); //.Replace("\"", "\\\""));
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.Resolution5))
				{
					eFormProps.AddKeyword(
						"Resolution5",
						grievanceAppealsData.Resolution5); //.Replace("\"", "\\\""));
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.Resolution6))
				{
					eFormProps.AddKeyword(
						"Resolution6",
						grievanceAppealsData.Resolution6); //.Replace("\"", "\\\""));
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.Resolution7))
				{
					eFormProps.AddKeyword(
						"Resolution7",
						grievanceAppealsData.Resolution7); //.Replace("\"", "\\\""));
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.Resolution8))
				{
					eFormProps.AddKeyword(
						"Resolution8",
						grievanceAppealsData.Resolution8); //.Replace("\"", "\\\""));
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.Resolution9))
				{
					eFormProps.AddKeyword(
						"Resolution9",
						grievanceAppealsData.Resolution9); //.Replace("\"", "\\\""));
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.Resolution10))
				{
					eFormProps.AddKeyword(
						"Resolution10",
						grievanceAppealsData.Resolution10); //.Replace("\"", "\\\""));
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.Status))
				{
					eFormProps.AddKeyword(
						"Status",
						grievanceAppealsData.Status);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.StreamLineCallerId))
				{
					eFormProps.AddKeyword(
						"Call ID",
						grievanceAppealsData.StreamLineCallerId);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.Subject))
				{
					eFormProps.AddKeyword(
						"Subject",
						grievanceAppealsData.Subject);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.SubjectCode))
				{
					eFormProps.AddKeyword(
						"Subject Code",
						grievanceAppealsData.SubjectCode);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.SubscriberId))
				{
					eFormProps.AddKeyword(
						"Subscriber ID",
						grievanceAppealsData.SubscriberId);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.SubType))
				{
					eFormProps.AddKeyword(
						"Sub-Type",
						grievanceAppealsData.SubType);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.Summary))
				{
					eFormProps.AddKeyword(
						"Summary",
						grievanceAppealsData.Summary); //.Replace("\"", "\\\""));
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.TaxId))
				{
					eFormProps.AddKeyword(
						"WV Provider TIN",
						grievanceAppealsData.TaxId);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.Type))
				{
					eFormProps.AddKeyword(
						"Type",
						grievanceAppealsData.Type);
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(loggerInit + "Error: Adding Grievance Keywords: " + ex);
				throw new Exception("Error: Adding Grievance Keywords: " + ex);
			}

			return true;
		}

		private  bool AddAppealKeywords(
			StoreNewUnityFormProperties eFormProps,
			GrievanceAppealsData grievanceAppealsData,
			string loggerInit)
		{
			try
			{
				// Appeals
				if (!string.IsNullOrEmpty(grievanceAppealsData.AdminCat1))
				{
					eFormProps.AddKeyword(
						"Admin Cat 1",
						grievanceAppealsData.AdminCat1);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.AdminCat2))
				{
					eFormProps.AddKeyword(
						"Admin Cat 2",
						grievanceAppealsData.AdminCat2);
				}

				//if (!string.IsNullOrEmpty(grievanceAppealsData.AORExpires))
				//    eFormProps.AddKeyword("Case AOR Receipt Date/Time", Convert.ToDateTime(grievanceAppealsData.ReceivedDate));

				if (!string.IsNullOrEmpty(grievanceAppealsData.AOR))
				{
					eFormProps.AddKeyword(
						"Case Authorized Contact Flag",
						grievanceAppealsData.AOR);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.AORRepresentativeFirst))
				{
					eFormProps.AddKeyword(
						"Case Appointment of Representative First Name",
						grievanceAppealsData.AORRepresentativeFirst);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.AORRepresentativeLast))
				{
					eFormProps.AddKeyword(
						"Case Appointment of Representative Last Name",
						grievanceAppealsData.AORRepresentativeLast);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.AORRepresentativePhone))
				{
					eFormProps.AddKeyword(
						"Case Appointment of Representative Phone",
						grievanceAppealsData.AORRepresentativePhone);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.ApplicationName))
				{
					eFormProps.AddKeyword(
						"Application Name",
						grievanceAppealsData.ApplicationName);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.AutoClose))
				{
					eFormProps.AddKeyword(
						"Auto-Close",
						grievanceAppealsData.AutoClose);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.CallerStatus))
				{
					eFormProps.AddKeyword(
						"Caller Status",
						grievanceAppealsData.CallerStatus);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.CallInMethod))
				{
					eFormProps.AddKeyword(
						"Case Submission  Method",
						grievanceAppealsData.CallInMethod);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.CallType))
				{
					eFormProps.AddKeyword(
						"Call Type",
						grievanceAppealsData.CallType);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.Category))
				{
					eFormProps.AddKeyword(
						"Category",
						grievanceAppealsData.Category);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.CategoryCode))
				{
					eFormProps.AddKeyword(
						"Category Code",
						grievanceAppealsData.CategoryCode);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.Complaint))
				{
					eFormProps.AddKeyword(
						"Complaint",
						grievanceAppealsData.Complaint);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.CurrentUserName))
				{
					eFormProps.AddKeyword(
						"Customer Advocate",
						grievanceAppealsData.CurrentUserName);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.CustomerType))
				{
					eFormProps.AddKeyword(
						"Case Submitter Type",
						grievanceAppealsData.CustomerType);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.Decision))
				{
					eFormProps.AddKeyword(
						"Decision",
						grievanceAppealsData.Decision);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.Description))
				{
					eFormProps.AddKeyword(
						"Case Issue Description",
						grievanceAppealsData.Description); //.Replace("\"", "\\\""));
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.EnrollmentEndDate))
				{
					eFormProps.AddKeyword(
						"Enrollment Term Date",
						Convert.ToDateTime(grievanceAppealsData.EnrollmentEndDate));
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.EnrollmentStartDate))
				{
					eFormProps.AddKeyword(
						"Enrollment Effective Date",
						Convert.ToDateTime(grievanceAppealsData.EnrollmentStartDate));
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.ESBGUID))
				{
					eFormProps.AddKeyword(
						"ESB GUID",
						grievanceAppealsData.ESBGUID);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.FacetsGrievanceId))
				{
					eFormProps.AddKeyword(
						"Facets Grievance ID",
						grievanceAppealsData.FacetsGrievanceId);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.GroupName))
				{
					eFormProps.AddKeyword(
						"Case Business Unit",
						grievanceAppealsData.GroupName);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.HICN))
				{
					eFormProps.AddKeyword(
						"Case Beneficiary HICN",
						grievanceAppealsData.HICN);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.InitiationDate))
				{
					eFormProps.AddKeyword(
						"Occurrence Date",
						Convert.ToDateTime(grievanceAppealsData.InitiationDate));
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.LinkReason))
				{
					eFormProps.AddKeyword(
						"Link Reason",
						grievanceAppealsData.LinkReason);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.LinkType))
				{
					eFormProps.AddKeyword(
						"Link Type",
						grievanceAppealsData.LinkType);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.MedicaidId))
				{
					eFormProps.AddKeyword(
						"Medicaid Number",
						grievanceAppealsData.MedicaidId);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.MemberCity))
				{
					eFormProps.AddKeyword(
						"Case Contract Holder Address City",
						grievanceAppealsData.MemberCity);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.MemberContactPhone))
				{
					eFormProps.AddKeyword(
						"Case Submitter Phone Number",
						grievanceAppealsData.MemberContactPhone);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.MemberDOB))
				{
					eFormProps.AddKeyword(
						"AC: Patient Date of Birth",
						Convert.ToDateTime(grievanceAppealsData.MemberDOB));
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.MemberEMail))
				{
					eFormProps.AddKeyword(
						"Case Submitter eMail",
						grievanceAppealsData.MemberEMail);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.MemberFirst))
				{
					eFormProps.AddKeyword(
						"AC: Patient First Name",
						grievanceAppealsData.MemberFirst);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.MemberFirst))
				{
					eFormProps.AddKeyword(
						"Case Beneficiary First Name",
						grievanceAppealsData.MemberFirst);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.MemberId))
				{
					eFormProps.AddKeyword(
						"Case Cardholder ID",
						grievanceAppealsData.MemberId);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.MemberId))
				{
					eFormProps.AddKeyword(
						"Case Subscriber ID",
						grievanceAppealsData.MemberId);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.MemberLast))
				{
					eFormProps.AddKeyword(
						"AC: Patient Last Name",
						grievanceAppealsData.MemberLast);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.MemberLast))
				{
					eFormProps.AddKeyword(
						"Case Beneficiary Last Name",
						grievanceAppealsData.MemberLast);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.MemberState))
				{
					eFormProps.AddKeyword(
						"Case Contract Holder Address State",
						grievanceAppealsData.MemberState);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.MemberStreet))
				{
					eFormProps.AddKeyword(
						"Case Contract Holder Address Line 1",
						grievanceAppealsData.MemberStreet);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.MemberZIP))
				{
					eFormProps.AddKeyword(
						"Case Contract Holder Address Zip",
						grievanceAppealsData.MemberZIP);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.Notes1))
				{
					eFormProps.AddKeyword(
						"Notes1",
						grievanceAppealsData.Notes1); //.Replace("\"", "\\\""));
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.Notes2))
				{
					eFormProps.AddKeyword(
						"Notes2",
						grievanceAppealsData.Notes2); //.Replace("\"", "\\\""));
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.Notes3))
				{
					eFormProps.AddKeyword(
						"Notes3",
						grievanceAppealsData.Notes3); //.Replace("\"", "\\\""));
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.Notes4))
				{
					eFormProps.AddKeyword(
						"Notes4",
						grievanceAppealsData.Notes4); //.Replace("\"", "\\\""));
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.Notes5))
				{
					eFormProps.AddKeyword(
						"Notes5",
						grievanceAppealsData.Notes5); //.Replace("\"", "\\\""));
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.Notes6))
				{
					eFormProps.AddKeyword(
						"Notes6",
						grievanceAppealsData.Notes6); //.Replace("\"", "\\\""));
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.Notes7))
				{
					eFormProps.AddKeyword(
						"Notes7",
						grievanceAppealsData.Notes7); //.Replace("\"", "\\\""));
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.Notes8))
				{
					eFormProps.AddKeyword(
						"Notes8",
						grievanceAppealsData.Notes8); //.Replace("\"", "\\\""));
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.Notes9))
				{
					eFormProps.AddKeyword(
						"Notes9",
						grievanceAppealsData.Notes9); //.Replace("\"", "\\\""));
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.Notes10))
				{
					eFormProps.AddKeyword(
						"Notes10",
						grievanceAppealsData.Notes10); //.Replace("\"", "\\\""));
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.PlanId))
				{
					eFormProps.AddKeyword(
						"Case Contract ID",
						grievanceAppealsData.PlanId);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.PlanName))
				{
					eFormProps.AddKeyword(
						"Organization Name",
						grievanceAppealsData.PlanName);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.Priority))
				{
					eFormProps.AddKeyword(
						"Case Opening Priority",
						grievanceAppealsData.Priority);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.ProviderCity))
				{
					eFormProps.AddKeyword(
						"Provider City",
						grievanceAppealsData.ProviderCity);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.ProviderFourCity))
				{
					eFormProps.AddKeyword(
						"Provider Four City",
						grievanceAppealsData.ProviderFourCity);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.ProviderFourId))
				{
					eFormProps.AddKeyword(
						"Provider Four ID",
						grievanceAppealsData.ProviderFourId);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.ProviderFourName))
				{
					eFormProps.AddKeyword(
						"Provider Four Name",
						grievanceAppealsData.ProviderFourName);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.ProviderFourNPI))
				{
					eFormProps.AddKeyword(
						"Provider Four NPI",
						grievanceAppealsData.ProviderFourNPI);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.ProviderFourState))
				{
					eFormProps.AddKeyword(
						"Provider Four State",
						grievanceAppealsData.ProviderFourState);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.ProviderFourStreet))
				{
					eFormProps.AddKeyword(
						"Provider Four Street",
						grievanceAppealsData.ProviderFourStreet);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.ProviderFourZIP))
				{
					eFormProps.AddKeyword(
						"Provider Four ZIP",
						grievanceAppealsData.ProviderFourZIP);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.ProviderName))
				{
					eFormProps.AddKeyword(
						"WV Provider Name",
						grievanceAppealsData.ProviderName);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.ProviderNPI))
				{
					eFormProps.AddKeyword(
						"WV Provider NPI",
						grievanceAppealsData.ProviderNPI);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.ProviderOneId))
				{
					eFormProps.AddKeyword(
						"Provider ID",
						grievanceAppealsData.ProviderOneId);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.ProviderState))
				{
					eFormProps.AddKeyword(
						"Provider State",
						grievanceAppealsData.ProviderState);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.ProviderStreet))
				{
					eFormProps.AddKeyword(
						"Provider Street",
						grievanceAppealsData.ProviderStreet);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.ProviderThreeCity))
				{
					eFormProps.AddKeyword(
						"Provider Three City",
						grievanceAppealsData.ProviderThreeCity);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.ProviderThreeId))
				{
					eFormProps.AddKeyword(
						"Provider Three ID",
						grievanceAppealsData.ProviderThreeId);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.ProviderThreeName))
				{
					eFormProps.AddKeyword(
						"Provider Three Name",
						grievanceAppealsData.ProviderThreeName);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.ProviderThreeNPI))
				{
					eFormProps.AddKeyword(
						"Provider Three NPI",
						grievanceAppealsData.ProviderThreeNPI);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.ProviderThreeState))
				{
					eFormProps.AddKeyword(
						"Provider Three State",
						grievanceAppealsData.ProviderThreeState);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.ProviderThreeStreet))
				{
					eFormProps.AddKeyword(
						"Provider Three Street",
						grievanceAppealsData.ProviderThreeStreet);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.ProviderThreeZIP))
				{
					eFormProps.AddKeyword(
						"Provider Three ZIP",
						grievanceAppealsData.ProviderThreeZIP);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.ProviderTwoCity))
				{
					eFormProps.AddKeyword(
						"Provider Two City",
						grievanceAppealsData.ProviderTwoCity);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.ProviderTwoId))
				{
					eFormProps.AddKeyword(
						"Provider Two ID",
						grievanceAppealsData.ProviderTwoId);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.ProviderTwoName))
				{
					eFormProps.AddKeyword(
						"Provider Two Name",
						grievanceAppealsData.ProviderTwoName);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.ProviderTwoNPI))
				{
					eFormProps.AddKeyword(
						"Provider Two NPI",
						grievanceAppealsData.ProviderTwoNPI);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.ProviderTwoState))
				{
					eFormProps.AddKeyword(
						"Provider Two State",
						grievanceAppealsData.ProviderTwoState);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.ProviderTwoStreet))
				{
					eFormProps.AddKeyword(
						"Provider Two Street",
						grievanceAppealsData.ProviderTwoStreet);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.ProviderTwoZIP))
				{
					eFormProps.AddKeyword(
						"Provider Two ZIP",
						grievanceAppealsData.ProviderTwoZIP);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.ProviderZIP))
				{
					eFormProps.AddKeyword(
						"Provider ZIP",
						grievanceAppealsData.ProviderZIP);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.Reason))
				{
					eFormProps.AddKeyword(
						"Reason",
						grievanceAppealsData.Reason);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.ReceivedDate))
				{
					eFormProps.AddKeyword(
						"Case Corporate Received Date",
						Convert.ToDateTime(grievanceAppealsData.ReceivedDate));
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.RepresentativeTermDate))
				{
					eFormProps.AddKeyword(
						"Case Appointment of Representative End Date",
						Convert.ToDateTime(grievanceAppealsData.RepresentativeTermDate));
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.Resolution1))
				{
					eFormProps.AddKeyword(
						"Resolution1",
						grievanceAppealsData.Resolution1); //.Replace("\"", "\\\""));
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.Resolution2))
				{
					eFormProps.AddKeyword(
						"Resolution2",
						grievanceAppealsData.Resolution2); //.Replace("\"", "\\\""));
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.Resolution3))
				{
					eFormProps.AddKeyword(
						"Resolution3",
						grievanceAppealsData.Resolution3); //.Replace("\"", "\\\""));
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.Resolution4))
				{
					eFormProps.AddKeyword(
						"Resolution4",
						grievanceAppealsData.Resolution4); //.Replace("\"", "\\\""));
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.Resolution5))
				{
					eFormProps.AddKeyword(
						"Resolution5",
						grievanceAppealsData.Resolution5); //.Replace("\"", "\\\""));
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.Resolution6))
				{
					eFormProps.AddKeyword(
						"Resolution6",
						grievanceAppealsData.Resolution6); //.Replace("\"", "\\\""));
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.Resolution7))
				{
					eFormProps.AddKeyword(
						"Resolution7",
						grievanceAppealsData.Resolution7); //.Replace("\"", "\\\""));
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.Resolution8))
				{
					eFormProps.AddKeyword(
						"Resolution8",
						grievanceAppealsData.Resolution8); //.Replace("\"", "\\\""));
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.Resolution9))
				{
					eFormProps.AddKeyword(
						"Resolution9",
						grievanceAppealsData.Resolution9); //.Replace("\"", "\\\""));
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.Resolution10))
				{
					eFormProps.AddKeyword(
						"Resolution10",
						grievanceAppealsData.Resolution10); //.Replace("\"", "\\\""));
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.Status))
				{
					eFormProps.AddKeyword(
						"Status",
						grievanceAppealsData.Status);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.StreamLineCallerId))
				{
					eFormProps.AddKeyword(
						"Call ID",
						grievanceAppealsData.StreamLineCallerId);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.Subject))
				{
					eFormProps.AddKeyword(
						"Subject",
						grievanceAppealsData.Subject);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.SubjectCode))
				{
					eFormProps.AddKeyword(
						"Subject Code",
						grievanceAppealsData.SubjectCode);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.SubType))
				{
					eFormProps.AddKeyword(
						"Sub-Type",
						grievanceAppealsData.SubType);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.Summary))
				{
					eFormProps.AddKeyword(
						"Summary",
						grievanceAppealsData.Summary); //.Replace("\"", "\\\""));
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.Type))
				{
					eFormProps.AddKeyword(
						"Type",
						grievanceAppealsData.Type);
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(loggerInit + "Error: Adding Appeal Keywords: " + ex);
				throw new Exception("Error: Adding Appeal Keywords: " + ex);
			}

			return true;
		}

		private  bool AddPPAppealKeywords(
			StoreNewUnityFormProperties eFormProps,
			GrievanceAppealsData grievanceAppealsData,
			string loggerInit)
		{
			try
			{
				// Provider Appeals...
				if (!string.IsNullOrEmpty(grievanceAppealsData.ApplicationName))
				{
					eFormProps.AddKeyword(
						"Application Name",
						grievanceAppealsData.ApplicationName);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.FacetsGrievanceId))
				{
					eFormProps.AddKeyword(
						"Appeal ID",
						grievanceAppealsData.FacetsGrievanceId);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.FacetsGrievanceId))
				{
					eFormProps.AddKeyword(
						"Facets Grievance ID",
						grievanceAppealsData.FacetsGrievanceId);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.PPAppealType))
				{
					eFormProps.AddKeyword(
						"Appeal Type",
						grievanceAppealsData.PPAppealType);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.PPBeginDOS))
				{
					eFormProps.AddKeyword(
						"Begin DOS",
						Convert.ToDateTime(grievanceAppealsData.PPBeginDOS));
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.CallInMethod))
				{
					eFormProps.AddKeyword(
						"Case Submission  Method",
						grievanceAppealsData.CallInMethod);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.CategoryCode))
				{
					eFormProps.AddKeyword(
						"Category Code",
						grievanceAppealsData.CategoryCode);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.PPClaimNumber))
				{
					eFormProps.AddKeyword(
						"Claim Number",
						grievanceAppealsData.PPClaimNumber);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.PPDateOfService))
				{
					eFormProps.AddKeyword(
						"Date of Service",
						Convert.ToDateTime(grievanceAppealsData.PPDateOfService));
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.ReceivedDate))
				{
					eFormProps.AddKeyword(
						"Date Received",
						Convert.ToDateTime(grievanceAppealsData.ReceivedDate));
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.PPDaysOld))
				{
					eFormProps.AddKeyword(
						"Days Old",
						Convert.ToInt32(grievanceAppealsData.PPDaysOld));
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.DocumentCount))
				{
					eFormProps.AddKeyword(
						"DocumentCount",
						Convert.ToInt32(grievanceAppealsData.DocumentCount));
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.PPEndDOS))
				{
					eFormProps.AddKeyword(
						"End DOS",
						Convert.ToDateTime(grievanceAppealsData.PPEndDOS));
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.PPEnrolleeName))
				{
					eFormProps.AddKeyword(
						"Enrollee Name",
						grievanceAppealsData.PPEnrolleeName);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.ESBGUID))
				{
					eFormProps.AddKeyword(
						"ESB GUID",
						grievanceAppealsData.ESBGUID);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.PPHealthPlan))
				{
					eFormProps.AddKeyword(
						"Health Plan",
						grievanceAppealsData.PPHealthPlan);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.HICN))
				{
					eFormProps.AddKeyword(
						"HICN",
						grievanceAppealsData.HICN);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.IncludeDocuments))
				{
					eFormProps.AddKeyword(
						"IncludeDocuments",
						grievanceAppealsData.IncludeDocuments);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.MemberId))
				{
					eFormProps.AddKeyword(
						"Member ID",
						grievanceAppealsData.MemberId);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.MemberFirst) &&
				    !string.IsNullOrEmpty(grievanceAppealsData.MemberLast))
				{
					eFormProps.AddKeyword(
						"Member Name",
						grievanceAppealsData.MemberFirst + " " + grievanceAppealsData.MemberLast);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.PlanId))
				{
					eFormProps.AddKeyword(
						"Plan ID",
						grievanceAppealsData.PlanId);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.ProviderOneId))
				{
					eFormProps.AddKeyword(
						"Provider ID",
						grievanceAppealsData.ProviderOneId);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.ProviderName))
				{
					eFormProps.AddKeyword(
						"Provider Name",
						grievanceAppealsData.ProviderName);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.ProviderNPI))
				{
					eFormProps.AddKeyword(
						"Provider NPI",
						grievanceAppealsData.ProviderNPI);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.ProviderState))
				{
					eFormProps.AddKeyword(
						"Provider State",
						grievanceAppealsData.ProviderState);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.TaxId))
				{
					eFormProps.AddKeyword(
						"Provider TIN",
						grievanceAppealsData.TaxId);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.PPSignature))
				{
					eFormProps.AddKeyword(
						"Signature",
						grievanceAppealsData.PPSignature);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.PPSignatureDate))
				{
					eFormProps.AddKeyword(
						"Signature Date",
						Convert.ToDateTime(grievanceAppealsData.PPSignatureDate));
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.Status))
				{
					eFormProps.AddKeyword(
						"Status",
						grievanceAppealsData.Status);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.SubscriberId))
				{
					eFormProps.AddKeyword(
						"Subscriber ID",
						grievanceAppealsData.SubscriberId);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.SubType))
				{
					eFormProps.AddKeyword(
						"Sub-Type",
						grievanceAppealsData.SubType);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.Type))
				{
					eFormProps.AddKeyword(
						"Type",
						grievanceAppealsData.Type);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.Notes1))
				{
					eFormProps.AddKeyword(
						"Notes1",
						grievanceAppealsData.Notes1); //.Replace("\"", "\\\""));
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.Notes2))
				{
					eFormProps.AddKeyword(
						"Notes2",
						grievanceAppealsData.Notes2); //.Replace("\"", "\\\""));
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.Notes3))
				{
					eFormProps.AddKeyword(
						"Notes3",
						grievanceAppealsData.Notes3); //.Replace("\"", "\\\""));
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.Notes4))
				{
					eFormProps.AddKeyword(
						"Notes4",
						grievanceAppealsData.Notes4); //.Replace("\"", "\\\""));
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.Notes5))
				{
					eFormProps.AddKeyword(
						"Notes5",
						grievanceAppealsData.Notes5); //.Replace("\"", "\\\""));
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.Notes6))
				{
					eFormProps.AddKeyword(
						"Notes6",
						grievanceAppealsData.Notes6); //.Replace("\"", "\\\""));
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.Notes7))
				{
					eFormProps.AddKeyword(
						"Notes7",
						grievanceAppealsData.Notes7); //.Replace("\"", "\\\""));
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.Notes8))
				{
					eFormProps.AddKeyword(
						"Notes8",
						grievanceAppealsData.Notes8); //.Replace("\"", "\\\""));
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.Notes9))
				{
					eFormProps.AddKeyword(
						"Notes9",
						grievanceAppealsData.Notes9); //.Replace("\"", "\\\""));
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.Notes10))
				{
					eFormProps.AddKeyword(
						"Notes10",
						grievanceAppealsData.Notes10); //.Replace("\"", "\\\""));
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.PreviousAppealorDisputeID))
				{
					eFormProps.AddKeyword(
						"Previous Appeal or Dispute ID",
						grievanceAppealsData.PreviousAppealorDisputeID);
				}

				// Not sure if these are needed on the unity form.
				//if (!string.IsNullOrEmpty(grievanceAppealsData.PPIsRetroAuth)) eFormProps.AddKeyword("IsRetroAuth", grievanceAppealsData.PPIsRetroAuth);
				//if (!string.IsNullOrEmpty(grievanceAppealsData.PPHasPriorAuth)) eFormProps.AddKeyword("HasPrioAuth", grievanceAppealsData.PPHasPriorAuth);
				//if (!string.IsNullOrEmpty(grievanceAppealsData.PPIsSNP)) eFormProps.AddKeyword("IsSNP", grievanceAppealsData.PPIsSNP);
				//if (!string.IsNullOrEmpty(grievanceAppealsData.PPIsNonPar)) eFormProps.AddKeyword("IsNonPar", grievanceAppealsData.PPIsNonPar);
				//if (!string.IsNullOrEmpty(grievanceAppealsData.PPSubGroup)) grievanceAppealsData.PPSubGroup = grievanceAppealsData.PPSubGroup;
			}
			catch (Exception ex)
			{
				_logger.LogError(loggerInit + "Error: Adding PP Appeal Keywords: " + ex);
				//return false;
				throw new Exception("Error: Adding PP Appeal Keywords: " + ex);
			}

			return true;
		}


		private  bool AddPPDisputeKeywords(
			StoreNewUnityFormProperties eFormProps,
			GrievanceAppealsData grievanceAppealsData,
			string loggerInit)
		{
			try
			{
				// Provider Disputes...

				// This needs to be added to Unity Form.
				if (!string.IsNullOrEmpty(grievanceAppealsData.ApplicationName))
				{
					eFormProps.AddKeyword(
						"Application Name",
						grievanceAppealsData.ApplicationName);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.PPClaimNumber))
				{
					eFormProps.AddKeyword(
						"Claim Number",
						grievanceAppealsData.PPClaimNumber);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.PPDateOfService))
				{
					eFormProps.AddKeyword(
						"Date of Service",
						Convert.ToDateTime(grievanceAppealsData.PPDateOfService));
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.PPDisputeId))
				{
					eFormProps.AddKeyword(
						"Dispute ID",
						grievanceAppealsData.PPDisputeId);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.DocumentCount))
				{
					eFormProps.AddKeyword(
						"DocumentCount",
						Convert.ToInt32(grievanceAppealsData.DocumentCount));
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.ESBGUID))
				{
					eFormProps.AddKeyword(
						"ESB GUID",
						grievanceAppealsData.ESBGUID);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.IncludeDocuments))
				{
					eFormProps.AddKeyword(
						"IncludeDocuments",
						grievanceAppealsData.IncludeDocuments);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.PPDisputeType))
				{
					eFormProps.AddKeyword(
						"Issue Type",
						grievanceAppealsData.PPDisputeType);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.PPHealthPlan))
				{
					eFormProps.AddKeyword(
						"Line of Business",
						grievanceAppealsData.PPHealthPlan);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.MemberId))
				{
					eFormProps.AddKeyword(
						"Member ID",
						grievanceAppealsData.MemberId);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.MemberFirst))
				{
					eFormProps.AddKeyword(
						"Member First Name",
						grievanceAppealsData.MemberFirst);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.MemberLast))
				{
					eFormProps.AddKeyword(
						"Member Last Name",
						grievanceAppealsData.MemberLast);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.MemberFirst) &&
				    !string.IsNullOrEmpty(grievanceAppealsData.MemberLast))
				{
					eFormProps.AddKeyword(
						"Member Name",
						grievanceAppealsData.MemberFirst + " " + grievanceAppealsData.MemberLast);
				}

				// Need to discuss adding additional keyword (Description 2) and then merging them back together.
				//eFormProps.AddKeyword("Description", grievanceAppealsData.Notes1.Replace("\"", "\\\""));     //Trim() + " " + grievanceAppealsData.Notes2.Trim());

				if (!string.IsNullOrEmpty(grievanceAppealsData.Notes1))
				{
					eFormProps.AddKeyword(
						"Notes1",
						grievanceAppealsData.Notes1); //.Replace("\"", "\\\""));
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.Notes2))
				{
					eFormProps.AddKeyword(
						"Notes2",
						grievanceAppealsData.Notes2); //.Replace("\"", "\\\""));
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.Notes3))
				{
					eFormProps.AddKeyword(
						"Notes3",
						grievanceAppealsData.Notes3); //.Replace("\"", "\\\""));
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.Notes4))
				{
					eFormProps.AddKeyword(
						"Notes4",
						grievanceAppealsData.Notes4); //.Replace("\"", "\\\""));
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.Notes5))
				{
					eFormProps.AddKeyword(
						"Notes5",
						grievanceAppealsData.Notes5); //.Replace("\"", "\\\""));
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.Notes6))
				{
					eFormProps.AddKeyword(
						"Notes6",
						grievanceAppealsData.Notes6); //.Replace("\"", "\\\""));
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.Notes7))
				{
					eFormProps.AddKeyword(
						"Notes7",
						grievanceAppealsData.Notes7); //.Replace("\"", "\\\""));
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.Notes8))
				{
					eFormProps.AddKeyword(
						"Notes8",
						grievanceAppealsData.Notes8); //.Replace("\"", "\\\""));
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.Notes9))
				{
					eFormProps.AddKeyword(
						"Notes9",
						grievanceAppealsData.Notes9); //.Replace("\"", "\\\""));
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.Notes10))
				{
					eFormProps.AddKeyword(
						"Notes10",
						grievanceAppealsData.Notes10); //.Replace("\"", "\\\""));
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.PlanId))
				{
					eFormProps.AddKeyword(
						"Plan ID",
						grievanceAppealsData.PlanId);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.ProviderOneId))
				{
					eFormProps.AddKeyword(
						"Provider ID",
						grievanceAppealsData.ProviderOneId);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.ProviderName))
				{
					eFormProps.AddKeyword(
						"Provider Name",
						grievanceAppealsData.ProviderName);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.ProviderNPI))
				{
					eFormProps.AddKeyword(
						"Provider NPI",
						grievanceAppealsData.ProviderNPI);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.TaxId))
				{
					eFormProps.AddKeyword(
						"Provider TAXID",
						grievanceAppealsData.TaxId);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.ProviderContactName))
				{
					eFormProps.AddKeyword(
						"Provider Contact Name",
						grievanceAppealsData.ProviderContactName);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.ProviderStreet))
				{
					eFormProps.AddKeyword(
						"Provider Street",
						grievanceAppealsData.ProviderStreet);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.ProviderCity))
				{
					eFormProps.AddKeyword(
						"Provider City",
						grievanceAppealsData.ProviderCity);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.ProviderState))
				{
					eFormProps.AddKeyword(
						"Provider State",
						grievanceAppealsData.ProviderState);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.ProviderZIP))
				{
					eFormProps.AddKeyword(
						"Provider ZIP",
						grievanceAppealsData.ProviderZIP);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.ProviderEmail))
				{
					eFormProps.AddKeyword(
						"Provider Email",
						grievanceAppealsData.ProviderEmail);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.ProviderPhone))
				{
					eFormProps.AddKeyword(
						"Provider Phone",
						grievanceAppealsData.ProviderPhone);
				}

				if (!string.IsNullOrEmpty(grievanceAppealsData.IssueCategory))
				{
					eFormProps.AddKeyword(
						"Issue Category",
						grievanceAppealsData.IssueCategory);
				}

				// Not sure if these are needed at all for a dispute.
				//if (!string.IsNullOrEmpty(grievanceAppealsData.PPDaysOld)) eFormProps.AddKeyword("Days Old", Convert.ToInt32(grievanceAppealsData.PPDaysOld));
				//if (!string.IsNullOrEmpty(grievanceAppealsData.Status)) eFormProps.AddKeyword("Status", grievanceAppealsData.Status);
				//if (!string.IsNullOrEmpty(grievanceAppealsData.PPDateReceived)) eFormProps.AddKeyword("Date Received", Convert.ToDateTime(grievanceAppealsData.PPDateReceived));
				//if (!string.IsNullOrEmpty(grievanceAppealsData.PPAppealType)) eFormProps.AddKeyword("Appeal Type", grievanceAppealsData.PPAppealType);
				//if (!string.IsNullOrEmpty(grievanceAppealsData.HICN)) eFormProps.AddKeyword("HICN", grievanceAppealsData.HICN);
				//if (!string.IsNullOrEmpty(grievanceAppealsData.PPEnrolleeName)) eFormProps.AddKeyword("Enrollee Name", grievanceAppealsData.PPEnrolleeName);
				//if (!string.IsNullOrEmpty(grievanceAppealsData.PPBeginDOS)) eFormProps.AddKeyword("Begin DOS", Convert.ToDateTime(grievanceAppealsData.PPBeginDOS));
				//if (!string.IsNullOrEmpty(grievanceAppealsData.PPEndDOS)) eFormProps.AddKeyword("End DOS", Convert.ToDateTime(grievanceAppealsData.PPEndDOS));
				//if (!string.IsNullOrEmpty(grievanceAppealsData.PPSignature)) eFormProps.AddKeyword("Signature", grievanceAppealsData.PPSignature);
				//if (!string.IsNullOrEmpty(grievanceAppealsData.PPSignatureDate)) eFormProps.AddKeyword("Signature Date", Convert.ToDateTime(grievanceAppealsData.PPSignatureDate));

				// Not sure if these are needed on the unity form.
				//if (!string.IsNullOrEmpty(grievanceAppealsData.PPIsRetroAuth)) eFormProps.AddKeyword("IsRetroAuth", grievanceAppealsData.PPIsRetroAuth);
				//if (!string.IsNullOrEmpty(grievanceAppealsData.PPHasPriorAuth)) eFormProps.AddKeyword("HasPrioAuth", grievanceAppealsData.PPHasPriorAuth);
				//if (!string.IsNullOrEmpty(grievanceAppealsData.PPIsSNP)) eFormProps.AddKeyword("IsSNP", grievanceAppealsData.PPIsSNP);
				//if (!string.IsNullOrEmpty(grievanceAppealsData.PPIsNonPar)) eFormProps.AddKeyword("IsNonPar", grievanceAppealsData.PPIsNonPar);
			}
			catch (Exception ex)
			{
				_logger.LogError(loggerInit + "Error: Adding PP Dispute Keywords: " + ex);
				throw new Exception("Error: Adding PP Dispute Keywords: " + ex);
			}

			return true;
		}

		private  Application ConnectApplication(
			OnBaseConnectionParameters connParams)
		{
			Application app = null;

			DomainAuthenticationProperties domAuthProps =
				Application.CreateDomainAuthenticationProperties(
					connParams.ServerURL,
					connParams.Datasource);
			domAuthProps.Domain = connParams.Domain;

			//Hyland.Unity.AuthenticationProperties domAuthProps = Hyland.Unity.Application.CreateOnBaseAuthenticationProperties(connParams.ServerURL, "MANAGER", "password", connParams.Datasource);


			try
			{
				app = Application.Connect(domAuthProps);
			}
			catch (MaxLicensesException ex)
			{
				throw new Exception(
					"Error: All available licenses have been consumed.",
					ex);
			}
			catch (SystemLockedOutException ex)
			{
				throw new Exception(
					"Error: The system is currently in lockout mode.",
					ex);
			}
			catch (InvalidLoginException ex)
			{
				throw new Exception(
					"Error: Invalid Login Credentials.",
					ex);
			}

			catch (AuthenticationFailedException ex)
			{
				throw new Exception(
					"Error: NT Authentication Failed.",
					ex);
			}
			catch (MaxConcurrentLicensesException ex)
			{
				throw new Exception(
					"Error: All concurrent licenses for this user group have been consumed.",
					ex);
			}
			catch (InvalidLicensingException ex)
			{
				throw new Exception(
					"Error: Invalid Licensing.",
					ex);
			}
			catch (Exception ex)
			{
				throw new Exception(
					$"Error: {ex.Message}",
					ex);
			}

			return app;
		}

		private  void DisconnectApplication(
			Application app)
		{
			try
			{
				app?.Disconnect();
			}
			catch (SessionNotFoundException ex)
			{
				throw new Exception(
					"Error: Active session could not be found.",
					ex);
			}
			finally
			{
				if (null != app)
				{
					app.Dispose();
					app = null;
				}
			}
		}
	}
}