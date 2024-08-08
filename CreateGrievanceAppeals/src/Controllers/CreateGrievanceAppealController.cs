// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   CreateGrievanceAppeals
//   CreateGrievanceAppealController.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using CareSource.WC.OnBase.Core.Diagnostics.Interfaces;
using log4net;
using log4net.Config;

namespace CareSource.WC.Services.CreateGrievanceAppeals.Controllers
{
	using System;
	using System.Collections.Generic;
	using System.Globalization;
	using System.Linq;
	using System.Net;
	using System.Net.Http;
	using System.Web.Configuration;
	using System.Web.Http;
	using System.Web.Http.Description;
	using System.Web.Http.ModelBinding;
	using CareSource.WC.OnBase.Core.Diagnostics.Interfaces;
	using CareSource.WC.OnBase.Core.Helpers.Interfaces;
	using CreateGrievanceAppeals.Manager;
	using CreateGrievanceAppeals.Models;
	using CreateGrievanceAppeals.Models.Data;
	using CreateGrievanceAppeals.Models.Requests;
	using KeywordValue = CreateGrievanceAppeals.Models.Data.KeywordValue;

	/// <inheritdoc />
	/// <summary>
	///    CreateGrievanceAppealController : ApiController
	/// </summary>
	public class CreateGrievanceAppealController : ApiController
	{
		public CreateGrievanceAppealController(
			ILogger logger,
			GrievanceAppealManager grievanceAppealManager,
			IJsonSerializerHelper serializer)
		{
			_logger = logger;
			_grievanceAppealManager = grievanceAppealManager;
			_serializer = serializer;
		}

		private readonly IJsonSerializerHelper _serializer;
		private readonly GrievanceAppealManager _grievanceAppealManager;
		private readonly ILogger _logger;

		/// <summary>
		///    Post Grievance/Appeals/Disputes
		/// </summary>
		/// <remarks>
		///    Post grievances, appeals, disputes to OnBase
		/// </remarks>
		/// <returns></returns>
		/// <response code="200"></response>
		[ResponseType(typeof(IEnumerable<GrievanceAppealsResult>))]
		[HttpPost]
		public HttpResponseMessage Post(
			RequestPayload<GrievanceAppealSubmission> request)
		{
			try
			{
				_logger.LogInfo(_serializer.ToJson(request));

				string loggerInit =
					$"{request.Payload.ApplicationName}/{request.TransactionData.ESBGUID}";

				if (!ModelState.IsValid)
				{
					List<string> errorlist = (
						from value in ModelState.Values
						from error in value.Errors
						select error.ErrorMessage).ToList();

					foreach (string error in errorlist)
					{
						_logger.LogInfo($"{loggerInit} {error}");
					}

					_logger.LogInfo($"{loggerInit} Service Aborted");

					return Request.CreateErrorResponse(
						HttpStatusCode.BadRequest,
						ModelState);
				}

				GrievanceAppealsData grievanceAppealsData = FormatGrievanceAppeals(
					request,
					loggerInit, _logger);

				if (grievanceAppealsData.Message != "SUCCESS")
				{
					return Request.CreateResponse(
						HttpStatusCode.BadRequest,
						grievanceAppealsData.Message);
				}

				_logger.LogInfo($"{loggerInit} Getting appication settings.");

				// Determine environment
				OnBaseConnectionParameters connParams = new OnBaseConnectionParameters
				{
					Domain = WebConfigurationManager.AppSettings["OnBase.Domain"],
					Datasource = WebConfigurationManager.AppSettings["OnBase.Datasource"],
					ServerURL = WebConfigurationManager.AppSettings["OnBase.Url"]
				};

				_logger.LogInfo($"{loggerInit} Retrieved appication settings.");

				GrievanceAppealsResult results = _grievanceAppealManager.CreateGrievanceAppeals(
					grievanceAppealsData,
					connParams);

				return Request.CreateResponse(results.Status == "FAILURE"
					? HttpStatusCode.BadRequest
					: HttpStatusCode.OK, results);
			}
			catch (Exception e)
			{
				_logger.LogError(e.Message);
				return Request.CreateErrorResponse(
					HttpStatusCode.BadRequest,
					e.Message);
			}
		}

		//***************************************************************
		// FormatGrievanceAppeals method
		// Validate and verify request data.
		//***************************************************************


		private static GrievanceAppealsData FormatGrievanceAppeals(
			RequestPayload<GrievanceAppealSubmission> request,
			string loggerInit, ILogger _logger)
		{
			GrievanceAppealsData grievanceAppealsData = new GrievanceAppealsData
			{
				Message = "SUCCESS"
			};

			try
			{
				if (!string.IsNullOrEmpty(request.Payload.AdminCat1))
				{
					grievanceAppealsData.AdminCat1 = request.Payload.AdminCat1;
				}

				if (!string.IsNullOrEmpty(request.Payload.AdminCat2))
				{
					grievanceAppealsData.AdminCat2 = request.Payload.AdminCat2;
				}

				if (!string.IsNullOrEmpty(request.Payload.AOR))
				{
					grievanceAppealsData.AOR = request.Payload.AOR;
				}

				if (!string.IsNullOrEmpty(request.Payload.AORExpires))
				{
					grievanceAppealsData.AORExpires = request.Payload.AORExpires;
				}

				if (!string.IsNullOrEmpty(request.Payload.AORRepresentativeCity))
				{
					grievanceAppealsData.AORRepresentativeCity = request.Payload.AORRepresentativeCity;
				}

				if (!string.IsNullOrEmpty(request.Payload.AORRepresentativeEmail))
				{
					grievanceAppealsData.AORRepresentativeEmail = request.Payload.AORRepresentativeEmail;
				}

				if (!string.IsNullOrEmpty(request.Payload.AORRepresentativeFirst))
				{
					grievanceAppealsData.AORRepresentativeFirst = request.Payload.AORRepresentativeFirst;
				}

				if (!string.IsNullOrEmpty(request.Payload.AORRepresentativeLast))
				{
					grievanceAppealsData.AORRepresentativeLast = request.Payload.AORRepresentativeLast;
				}

				if (!string.IsNullOrEmpty(request.Payload.AORRepresentativePhone))
				{
					grievanceAppealsData.AORRepresentativePhone = request.Payload.AORRepresentativePhone;
				}

				if (!string.IsNullOrEmpty(request.Payload.AORRepresentativeState))
				{
					grievanceAppealsData.AORRepresentativeState = request.Payload.AORRepresentativeState;
				}

				if (!string.IsNullOrEmpty(request.Payload.AORRepresentativeStreet))
				{
					grievanceAppealsData.AORRepresentativeStreet =
						request.Payload.AORRepresentativeStreet;
				}

				if (!string.IsNullOrEmpty(request.Payload.AORRepresentativeZIP))
				{
					grievanceAppealsData.AORRepresentativeZIP = request.Payload.AORRepresentativeZIP;
				}

				if (!string.IsNullOrEmpty(request.Payload.AppealType))
				{
					grievanceAppealsData.PPAppealType = request.Payload.AppealType;
				}

				if (!string.IsNullOrEmpty(request.Payload.ApplicationName))
				{
					grievanceAppealsData.ApplicationName = request.Payload.ApplicationName;
				}

				if (!string.IsNullOrEmpty(request.Payload.AutoClose))
				{
					grievanceAppealsData.AutoClose = request.Payload.AutoClose;
				}

				if (!string.IsNullOrEmpty(request.Payload.BeginDOS))
				{
					grievanceAppealsData.PPBeginDOS = request.Payload.BeginDOS;
				}

				if (!string.IsNullOrEmpty(request.Payload.CallerId))
				{
					grievanceAppealsData.StreamLineCallerId = request.Payload.CallerId;
				}

				if (!string.IsNullOrEmpty(request.Payload.CallerStatus))
				{
					grievanceAppealsData.CallerStatus = request.Payload.CallerStatus;
				}

				if (!string.IsNullOrEmpty(request.Payload.CallInMethod))
				{
					grievanceAppealsData.CallInMethod = request.Payload.CallInMethod;
				}

				if (!string.IsNullOrEmpty(request.Payload.CallType))
				{
					grievanceAppealsData.CallType = request.Payload.CallType;
				}

				if (!string.IsNullOrEmpty(request.Payload.Category))
				{
					grievanceAppealsData.Category = request.Payload.Category;
				}

				if (!string.IsNullOrEmpty(request.Payload.CategoryCode))
				{
					grievanceAppealsData.CategoryCode = request.Payload.CategoryCode;
				}

				if (!string.IsNullOrEmpty(request.Payload.ClaimId))
				{
					grievanceAppealsData.PPClaimNumber = request.Payload.ClaimId;
				}

				if (!string.IsNullOrEmpty(request.TransactionData.CorrelationId))
				{
					grievanceAppealsData.CorrelationId = request.TransactionData.CorrelationId;
				}

				if (!string.IsNullOrEmpty(request.Payload.CurrentUserName))
				{
					grievanceAppealsData.CurrentUserName = request.Payload.CurrentUserName;
				}

				if (!string.IsNullOrEmpty(request.Payload.CustomerType))
				{
					grievanceAppealsData.CustomerType = request.Payload.CustomerType;
				}

				if (!string.IsNullOrEmpty(request.Payload.DateComplete))
				{
					grievanceAppealsData.DateComplete = request.Payload.DateComplete;
				}

				if (!string.IsNullOrEmpty(request.Payload.DateOfService))
				{
					grievanceAppealsData.PPDateOfService = request.Payload.DateOfService;
				}

				if (!string.IsNullOrEmpty(request.Payload.DateReceived))
				{
					grievanceAppealsData.PPDateReceived = request.Payload.DateReceived;
				}

				if (!string.IsNullOrEmpty(request.Payload.DaysOld))
				{
					grievanceAppealsData.PPDaysOld = request.Payload.DaysOld;
				}

				if (!string.IsNullOrEmpty(request.Payload.Decision))
				{
					grievanceAppealsData.Decision = request.Payload.Decision;
				}

				if (!string.IsNullOrEmpty(request.Payload.Description))
				{
					grievanceAppealsData.Description = request.Payload.Description;
				}

				if (!string.IsNullOrEmpty(request.Payload.DisputeId))
				{
					grievanceAppealsData.PPDisputeId = request.Payload.DisputeId;
				}

				if (!string.IsNullOrEmpty(request.Payload.DisputeType))
				{
					grievanceAppealsData.PPDisputeType = request.Payload.DisputeType;
				}

				if (!string.IsNullOrEmpty(request.TransactionData.DocumentCount))
				{
					grievanceAppealsData.DocumentCount = request.TransactionData.DocumentCount;
				}

				if (!string.IsNullOrEmpty(request.Payload.EndDOS))
				{
					grievanceAppealsData.PPEndDOS = request.Payload.EndDOS;
				}

				if (!string.IsNullOrEmpty(request.Payload.EnrolleeName))
				{
					grievanceAppealsData.PPEnrolleeName = request.Payload.EnrolleeName;
				}

				if (!string.IsNullOrEmpty(request.Payload.EnrollmentEndDate))
				{
					grievanceAppealsData.EnrollmentEndDate = request.Payload.EnrollmentEndDate;
				}

				if (!string.IsNullOrEmpty(request.Payload.EnrollmentStartDate))
				{
					grievanceAppealsData.EnrollmentStartDate = request.Payload.EnrollmentStartDate;
				}

				if (!string.IsNullOrEmpty(request.TransactionData.ESBGUID))
				{
					grievanceAppealsData.ESBGUID = request.TransactionData.ESBGUID;
				}

				if (!string.IsNullOrEmpty(request.Payload.FacetsGrievanceId))
				{
					grievanceAppealsData.FacetsGrievanceId = request.Payload.FacetsGrievanceId;
				}

				if (!string.IsNullOrEmpty(request.Payload.GroupName))
				{
					grievanceAppealsData.GroupName = request.Payload.GroupName;
				}

				if (!string.IsNullOrEmpty(request.Payload.HasPriorAuth))
				{
					grievanceAppealsData.PPHasPriorAuth = request.Payload.HasPriorAuth;
				}

				if (!string.IsNullOrEmpty(request.Payload.HealthPlan))
				{
					grievanceAppealsData.PPHealthPlan = request.Payload.HealthPlan;
				}

				if (!string.IsNullOrEmpty(request.Payload.HICN))
				{
					grievanceAppealsData.HICN = request.Payload.HICN;
				}

				if (!string.IsNullOrEmpty(request.TransactionData.IncludeDocuments))
				{
					grievanceAppealsData.IncludeDocuments = request.TransactionData.IncludeDocuments;
				}

				if (!string.IsNullOrEmpty(request.Payload.InitiationDate))
				{
					grievanceAppealsData.InitiationDate = request.Payload.InitiationDate;
				}

				if (!string.IsNullOrEmpty(request.Payload.IsNonPar))
				{
					grievanceAppealsData.PPIsNonPar = request.Payload.IsNonPar;
				}

				if (!string.IsNullOrEmpty(request.Payload.IsRetroAuth))
				{
					grievanceAppealsData.PPIsRetroAuth = request.Payload.IsRetroAuth;
				}

				if (!string.IsNullOrEmpty(request.Payload.IsSNP))
				{
					grievanceAppealsData.PPIsSNP = request.Payload.IsSNP;
				}

				if (!string.IsNullOrEmpty(request.Payload.IssueCategory))
				{
					grievanceAppealsData.IssueCategory = request.Payload.IssueCategory;
				}

				if (!string.IsNullOrEmpty(request.Payload.LinkReason))
				{
					grievanceAppealsData.LinkReason = request.Payload.LinkReason;
				}

				if (!string.IsNullOrEmpty(request.Payload.LinkType))
				{
					grievanceAppealsData.LinkType = request.Payload.LinkType;
				}

				if (!string.IsNullOrEmpty(request.Payload.MedicaidId))
				{
					grievanceAppealsData.MedicaidId = request.Payload.MedicaidId;
				}

				if (!string.IsNullOrEmpty(request.Payload.MemberCity))
				{
					grievanceAppealsData.MemberCity = request.Payload.MemberCity;
				}

				if (!string.IsNullOrEmpty(request.Payload.MemberContactPhone))
				{
					grievanceAppealsData.MemberContactPhone = request.Payload.MemberContactPhone;
				}

				if (!string.IsNullOrEmpty(request.Payload.MemberDOB))
				{
					grievanceAppealsData.MemberDOB = request.Payload.MemberDOB;
				}

				if (!string.IsNullOrEmpty(request.Payload.MemberEMail))
				{
					grievanceAppealsData.MemberEMail = request.Payload.MemberEMail;
				}

				if (!string.IsNullOrEmpty(request.Payload.MemberFirst))
				{
					grievanceAppealsData.MemberFirst = request.Payload.MemberFirst;
				}

				if (!string.IsNullOrEmpty(request.Payload.MemberId))
				{
					grievanceAppealsData.MemberId = request.Payload.MemberId;
				}

				if (!string.IsNullOrEmpty(request.Payload.MemberLast))
				{
					grievanceAppealsData.MemberLast = request.Payload.MemberLast;
				}

				if (!string.IsNullOrEmpty(request.Payload.MemberState))
				{
					grievanceAppealsData.MemberState = request.Payload.MemberState;
				}

				if (!string.IsNullOrEmpty(request.Payload.MemberStreet))
				{
					grievanceAppealsData.MemberStreet = request.Payload.MemberStreet;
				}

				if (!string.IsNullOrEmpty(request.Payload.MemberZIP))
				{
					grievanceAppealsData.MemberZIP = request.Payload.MemberZIP;
				}

				if (!string.IsNullOrEmpty(request.Payload.Notes))
				{
					grievanceAppealsData.Notes = request.Payload.Notes;
				}

				if (!string.IsNullOrEmpty(request.Payload.PlanId))
				{
					grievanceAppealsData.PlanId = request.Payload.PlanId;
				}

				if (!string.IsNullOrEmpty(request.Payload.PlanName))
				{
					grievanceAppealsData.PlanName = request.Payload.PlanName;
				}

				if (!string.IsNullOrEmpty(request.Payload.PlanStatus))
				{
					grievanceAppealsData.PlanStatus = request.Payload.PlanStatus;
				}

				if (!string.IsNullOrEmpty(request.Payload.Priority))
				{
					grievanceAppealsData.Priority = request.Payload.Priority;
				}

				if (!string.IsNullOrEmpty(request.Payload.ProviderCity))
				{
					grievanceAppealsData.ProviderCity = request.Payload.ProviderCity;
				}

				if (!string.IsNullOrEmpty(request.Payload.ProviderContactName))
				{
					grievanceAppealsData.ProviderContactName = request.Payload.ProviderContactName;
				}

				if (!string.IsNullOrEmpty(request.Payload.ProviderEmail))
				{
					grievanceAppealsData.ProviderEmail = request.Payload.ProviderEmail;
				}

				if (!string.IsNullOrEmpty(request.Payload.ProviderFourCity))
				{
					grievanceAppealsData.ProviderFourCity = request.Payload.ProviderFourCity;
				}

				if (!string.IsNullOrEmpty(request.Payload.ProviderFourContactName))
				{
					grievanceAppealsData.ProviderFourContactName = request.Payload.ProviderFourContactName;
				}

				if (!string.IsNullOrEmpty(request.Payload.ProviderFourEmail))
				{
					grievanceAppealsData.ProviderFourEmail = request.Payload.ProviderFourEmail;
				}

				if (!string.IsNullOrEmpty(request.Payload.ProviderFourId))
				{
					grievanceAppealsData.ProviderFourId = request.Payload.ProviderFourId;
				}

				if (!string.IsNullOrEmpty(request.Payload.ProviderFourName))
				{
					grievanceAppealsData.ProviderFourName = request.Payload.ProviderFourName;
				}

				if (!string.IsNullOrEmpty(request.Payload.ProviderFourNPI))
				{
					grievanceAppealsData.ProviderFourNPI = request.Payload.ProviderFourNPI;
				}

				if (!string.IsNullOrEmpty(request.Payload.ProviderFourPhone))
				{
					grievanceAppealsData.ProviderFourPhone = request.Payload.ProviderFourPhone;
				}

				if (!string.IsNullOrEmpty(request.Payload.ProviderFourState))
				{
					grievanceAppealsData.ProviderFourState = request.Payload.ProviderFourState;
				}

				if (!string.IsNullOrEmpty(request.Payload.ProviderFourStreet))
				{
					grievanceAppealsData.ProviderFourStreet = request.Payload.ProviderFourStreet;
				}

				if (!string.IsNullOrEmpty(request.Payload.ProviderFourTaxId))
				{
					grievanceAppealsData.ProviderFourTaxId = request.Payload.ProviderFourTaxId;
				}

				if (!string.IsNullOrEmpty(request.Payload.ProviderFourZIP))
				{
					grievanceAppealsData.ProviderFourZIP = request.Payload.ProviderFourZIP;
				}

				if (!string.IsNullOrEmpty(request.Payload.ProviderId))
				{
					grievanceAppealsData.ProviderOneId = request.Payload.ProviderId;
				}

				if (!string.IsNullOrEmpty(request.Payload.ProviderName))
				{
					grievanceAppealsData.ProviderName = request.Payload.ProviderName;
				}

				if (!string.IsNullOrEmpty(request.Payload.ProviderNPI))
				{
					grievanceAppealsData.ProviderNPI = request.Payload.ProviderNPI;
				}

				if (!string.IsNullOrEmpty(request.Payload.ProviderPhone))
				{
					grievanceAppealsData.ProviderPhone = request.Payload.ProviderPhone;
				}

				if (!string.IsNullOrEmpty(request.Payload.ProviderState))
				{
					grievanceAppealsData.ProviderState = request.Payload.ProviderState;
				}

				if (!string.IsNullOrEmpty(request.Payload.ProviderStreet))
				{
					grievanceAppealsData.ProviderStreet = request.Payload.ProviderStreet;
				}

				if (!string.IsNullOrEmpty(request.Payload.ProviderThreeCity))
				{
					grievanceAppealsData.ProviderThreeCity = request.Payload.ProviderThreeCity;
				}

				if (!string.IsNullOrEmpty(request.Payload.ProviderThreeContactName))
				{
					grievanceAppealsData.ProviderThreeContactName = request.Payload.ProviderThreeContactName;
				}

				if (!string.IsNullOrEmpty(request.Payload.ProviderThreeEmail))
				{
					grievanceAppealsData.ProviderThreeEmail = request.Payload.ProviderThreeEmail;
				}

				if (!string.IsNullOrEmpty(request.Payload.ProviderThreeId))
				{
					grievanceAppealsData.ProviderThreeId = request.Payload.ProviderThreeId;
				}

				if (!string.IsNullOrEmpty(request.Payload.ProviderThreeName))
				{
					grievanceAppealsData.ProviderThreeName = request.Payload.ProviderThreeName;
				}

				if (!string.IsNullOrEmpty(request.Payload.ProviderThreeNPI))
				{
					grievanceAppealsData.ProviderThreeNPI = request.Payload.ProviderThreeNPI;
				}

				if (!string.IsNullOrEmpty(request.Payload.ProviderThreePhone))
				{
					grievanceAppealsData.ProviderThreePhone = request.Payload.ProviderThreePhone;
				}

				if (!string.IsNullOrEmpty(request.Payload.ProviderThreeState))
				{
					grievanceAppealsData.ProviderThreeState = request.Payload.ProviderThreeState;
				}

				if (!string.IsNullOrEmpty(request.Payload.ProviderThreeStreet))
				{
					grievanceAppealsData.ProviderThreeStreet = request.Payload.ProviderThreeStreet;
				}

				if (!string.IsNullOrEmpty(request.Payload.ProviderThreeTaxId))
				{
					grievanceAppealsData.ProviderThreeTaxId = request.Payload.ProviderThreeTaxId;
				}

				if (!string.IsNullOrEmpty(request.Payload.ProviderThreeZIP))
				{
					grievanceAppealsData.ProviderThreeZIP = request.Payload.ProviderThreeZIP;
				}

				if (!string.IsNullOrEmpty(request.Payload.ProviderTwoCity))
				{
					grievanceAppealsData.ProviderTwoCity = request.Payload.ProviderTwoCity;
				}

				if (!string.IsNullOrEmpty(request.Payload.ProviderTwoContactName))
				{
					grievanceAppealsData.ProviderTwoContactName = request.Payload.ProviderTwoContactName;
				}

				if (!string.IsNullOrEmpty(request.Payload.ProviderTwoEmail))
				{
					grievanceAppealsData.ProviderTwoEmail = request.Payload.ProviderTwoEmail;
				}

				if (!string.IsNullOrEmpty(request.Payload.ProviderTwoId))
				{
					grievanceAppealsData.ProviderTwoId = request.Payload.ProviderTwoId;
				}

				if (!string.IsNullOrEmpty(request.Payload.ProviderTwoName))
				{
					grievanceAppealsData.ProviderTwoName = request.Payload.ProviderTwoName;
				}

				if (!string.IsNullOrEmpty(request.Payload.ProviderTwoNPI))
				{
					grievanceAppealsData.ProviderTwoNPI = request.Payload.ProviderTwoNPI;
				}

				if (!string.IsNullOrEmpty(request.Payload.ProviderTwoPhone))
				{
					grievanceAppealsData.ProviderTwoPhone = request.Payload.ProviderTwoPhone;
				}

				if (!string.IsNullOrEmpty(request.Payload.ProviderTwoState))
				{
					grievanceAppealsData.ProviderTwoState = request.Payload.ProviderTwoState;
				}

				if (!string.IsNullOrEmpty(request.Payload.ProviderTwoStreet))
				{
					grievanceAppealsData.ProviderTwoStreet = request.Payload.ProviderTwoStreet;
				}

				if (!string.IsNullOrEmpty(request.Payload.ProviderTwoTaxId))
				{
					grievanceAppealsData.ProviderTwoTaxId = request.Payload.ProviderTwoTaxId;
				}

				if (!string.IsNullOrEmpty(request.Payload.ProviderTwoZIP))
				{
					grievanceAppealsData.ProviderTwoZIP = request.Payload.ProviderTwoZIP;
				}

				if (!string.IsNullOrEmpty(request.Payload.ProviderZIP))
				{
					grievanceAppealsData.ProviderZIP = request.Payload.ProviderZIP;
				}

				if (!string.IsNullOrEmpty(request.Payload.Reason))
				{
					grievanceAppealsData.Reason = request.Payload.Reason;
				}

				if (!string.IsNullOrEmpty(request.Payload.ReceivedDate))
				{
					grievanceAppealsData.ReceivedDate = request.Payload.ReceivedDate;
				}

				if (!string.IsNullOrEmpty(request.Payload.RepresentativeTermDate))
				{
					grievanceAppealsData.RepresentativeTermDate = request.Payload.RepresentativeTermDate;
				}

				if (!string.IsNullOrEmpty(request.Payload.Resolution))
				{
					grievanceAppealsData.Resolution = request.Payload.Resolution;
				}

				if (!string.IsNullOrEmpty(request.Payload.Signature))
				{
					grievanceAppealsData.PPSignature = request.Payload.Signature;
				}

				if (!string.IsNullOrEmpty(request.Payload.SignatureDate))
				{
					grievanceAppealsData.PPSignatureDate = request.Payload.SignatureDate;
				}

				if (!string.IsNullOrEmpty(request.TransactionData.SourceAppId))
				{
					grievanceAppealsData.SourceAppId = request.TransactionData.SourceAppId;
				}

				if (!string.IsNullOrEmpty(request.TransactionData.SourceAppName))
				{
					grievanceAppealsData.SourceAppName = request.TransactionData.SourceAppName;
				}

				if (!string.IsNullOrEmpty(request.Payload.Status))
				{
					grievanceAppealsData.Status = request.Payload.Status;
				}

				if (!string.IsNullOrEmpty(request.Payload.SubGroup))
				{
					grievanceAppealsData.PPSubGroup = request.Payload.SubGroup;
				}

				if (!string.IsNullOrEmpty(request.Payload.Subject))
				{
					grievanceAppealsData.Subject = request.Payload.Subject;
				}

				if (!string.IsNullOrEmpty(request.Payload.SubjectCode))
				{
					grievanceAppealsData.SubjectCode = request.Payload.SubjectCode;
				}

				if (!string.IsNullOrEmpty(request.Payload.MemberId))
				{
					grievanceAppealsData.SubscriberId = request.Payload.MemberId.Substring(
						0,
						9);
				}

				if (!string.IsNullOrEmpty(request.Payload.SubType))
				{
					grievanceAppealsData.SubType = request.Payload.SubType;
				}

				if (!string.IsNullOrEmpty(request.Payload.Summary))
				{
					grievanceAppealsData.Summary = request.Payload.Summary;
				}

				if (!string.IsNullOrEmpty(request.Payload.TaxId))
				{
					grievanceAppealsData.TaxId = request.Payload.TaxId;
				}

				if (!string.IsNullOrEmpty(request.TransactionData.TransactionType))
				{
					grievanceAppealsData.TransactionType = request.TransactionData.TransactionType;
				}

				if (!string.IsNullOrEmpty(request.Payload.Type))
				{
					grievanceAppealsData.Type = request.Payload.Type;
				}

				if (!string.IsNullOrEmpty(request.Payload.Type))
				{
					grievanceAppealsData.Type = request.Payload.Type;
				}

				if (!string.IsNullOrEmpty(request.TransactionData.UserId))
				{
					grievanceAppealsData.UserId = request.TransactionData.UserId;
				}


				//AdditionalInput array will be used to add data elements without distruption to ESB model.
				if (request.Payload.AdditionalInput != null)
				{
					int kwIndex = request.Payload.AdditionalInput.Length;

					for (int i = 0; i < kwIndex; i++)
					{
						string erMessage;
						if (string.IsNullOrEmpty(
							    request.Payload.AdditionalInput[i]
								    .InputName) ||
						    string.IsNullOrEmpty(
							    request.Payload.AdditionalInput[i]
								    .Value))
						{
							erMessage = "Error in request data: Need to enter InputName and/or Value.";
							_logger.LogError(
								$"{loggerInit} {erMessage}");
							grievanceAppealsData.Message = erMessage;

							return grievanceAppealsData;
						}

						//grievanceAppealsData.AdditionalInput.Add(new KeywordValue());
						//grievanceAppealsData.AdditionalInput[i].InputName = request.Payload.AdditionalInput[i].InputName;
						//grievanceAppealsData.AdditionalInput[i].Value = request.Payload.AdditionalInput[i].Value;

						int keywordLength;
						switch (request.Payload.AdditionalInput[i]
							.InputName.ToUpper())
						{
							case "PREVIOUS APPEAL OR DISPUTE ID":
								keywordLength = 20;
								grievanceAppealsData.PreviousAppealorDisputeID = request.Payload.AdditionalInput[i]
									.Value;
								break;

							default:
								erMessage =
									"Error: Invalid data element entered in AdditionalInput section.";
								_logger.LogError(
									$"{loggerInit} {erMessage}");
								grievanceAppealsData.Message = erMessage;

								return grievanceAppealsData;
						}

						// Check the length of the keyword.
						if (keywordLength == 0)
						{
							continue;
						}

						erMessage = CheckLengthOfKeyword(
							request.Payload.AdditionalInput[i]
								.Value.Length.ToString(),
							request.Payload.AdditionalInput[i]
								.InputName.ToUpper(),
							keywordLength,
							loggerInit, _logger);

						if (erMessage == "SUCCESS")
						{
							continue;
						}

						grievanceAppealsData.Message = erMessage;
						return grievanceAppealsData;
					}
				}

				// Break the Notes down to the 250 character Notes1 - 10 fields.
				if (!string.IsNullOrEmpty(grievanceAppealsData.Notes))
				{
					string[] arr = grievanceAppealsData.Notes.SplitUpString(250);

					int strIndex = arr.Length - 1;
					for (int i = 0; i <= strIndex && i < 10; i++)
					{
						switch (i)
						{
							case 0:
								grievanceAppealsData.Notes1 = arr[0];
								break;

							case 1:
								grievanceAppealsData.Notes2 = arr[1];
								break;

							case 2:
								grievanceAppealsData.Notes3 = arr[2];
								break;

							case 3:
								grievanceAppealsData.Notes4 = arr[3];
								break;

							case 4:
								grievanceAppealsData.Notes5 = arr[4];
								break;

							case 5:
								grievanceAppealsData.Notes6 = arr[5];
								break;

							case 6:
								grievanceAppealsData.Notes7 = arr[6];
								break;

							case 7:
								grievanceAppealsData.Notes8 = arr[7];
								break;

							case 8:
								grievanceAppealsData.Notes9 = arr[8];
								break;

							case 9:
								grievanceAppealsData.Notes10 = arr[9];
								break;
						}
					}
				}

				// Break the Resolution down to the 250 character Resolution1 - 10 fields.
				if (!string.IsNullOrEmpty(grievanceAppealsData.Resolution))
				{
					string[] arr = grievanceAppealsData.Resolution.SplitUpString(250);

					int strIndex = arr.Length - 1;
					for (int i = 0; i <= strIndex && i < 10; i++)
					{
						switch (i)
						{
							case 0:
								grievanceAppealsData.Resolution1 = arr[0];
								break;

							case 1:
								grievanceAppealsData.Resolution2 = arr[1];
								break;

							case 2:
								grievanceAppealsData.Resolution3 = arr[2];
								break;

							case 3:
								grievanceAppealsData.Resolution4 = arr[3];
								break;

							case 4:
								grievanceAppealsData.Resolution5 = arr[4];
								break;

							case 5:
								grievanceAppealsData.Resolution6 = arr[5];
								break;

							case 6:
								grievanceAppealsData.Resolution7 = arr[6];
								break;

							case 7:
								grievanceAppealsData.Resolution8 = arr[7];
								break;

							case 8:
								grievanceAppealsData.Resolution9 = arr[8];
								break;

							case 9:
								grievanceAppealsData.Resolution10 = arr[9];
								break;
						}
					}
				}
			}
			catch (Exception ex)
			{
				loggerInit = $"{request.Payload.ApplicationName}/{request.TransactionData.ESBGUID}";
				_logger.LogError($"{loggerInit} Error in request data: {ex}");
				throw;
			}

			return grievanceAppealsData;
		}

		// Method to validate the length of the keyword.
		private static string CheckLengthOfKeyword(
			string dataElementValue,
			string dataLabel,
			int length,
			string loggerInit, ILogger _logger)
		{
			if (Convert.ToInt16(dataElementValue) <= length)
			{
				return "SUCCESS";
			}

			string erMessage = 
				$"Data Element: {dataLabel} length is greater than the {length.ToString()} character" +
				" limit.";
			_logger.LogError($"{loggerInit} {erMessage}");
			return erMessage;

		}
	}

	public static class StringExt
	{
		public static string[] SplitUpString(
			this string source,
			int maxLength)
		{
			return source
				.Where(
					(
							x,
							i) => i % maxLength == 0)
				.Select(
					(
						x,
						i) => new string(
						source
							.Skip(i * maxLength)
							.Take(maxLength)
							.ToArray()))
				.ToArray();
		}
	}
}
