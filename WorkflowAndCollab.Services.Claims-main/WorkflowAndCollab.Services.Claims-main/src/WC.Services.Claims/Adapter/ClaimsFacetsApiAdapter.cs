// ------------------------------------------------------------------------------------------------
//  <copyright>
//    Copyright (c) CareSource, 2020-2022.  All rights reserved.
// 
//    Claims
//    ClaimsFacetsApiAdapter.cs
//  </copyright>
//  ------------------------------------------------------------------------------------------------

namespace Claims.Adapter
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;
	using System.ServiceModel;
	using System.ServiceModel.Channels;
	using Claims.Adapter.Interfaces;
	using Claims.Models;
	using Facets.Claim.Secure.Cdml;
	using Facets.Claims.Secure.List;
	using Microsoft.Extensions.Logging;

	//using FxiIslHeader = Facets.Claim.GetClaimLineItems.FxiIslHeader;

	//using Config = Facets.Claim.GetList.Config;

	public class ClaimsFacetsApiAdapter : FXIAuthAdapter, IClaimsAdapter<Claim>
	{
		private readonly ILogger<ClaimsFacetsApiAdapter> _logger;

		private readonly FacetsConfiguration _facetsConfig;

		public ClaimsFacetsApiAdapter(
			IConfiguration settingsAdapter,
			ILogger<ClaimsFacetsApiAdapter> logger)
		{
			_facetsConfig = new FacetsConfiguration();

			settingsAdapter.GetSection("Facets").Bind(_facetsConfig);
			_logger = logger;
		}

		public Claim GetById(string id)
		{
			Facets.Claims.Secure.List.Config
				facetsSearchConfig =
				new Facets.Claims.Secure.List.Config()
				{
					Region = _facetsConfig.Region,
					FacetsIdentity = _facetsConfig.Identity
				};

			string token = GetAuthToken(_facetsConfig);

			bool getNextPage = true;
			List<REC_CLCL> claims = new List<REC_CLCL>();

			for (int i = 1; getNextPage && i <= _facetsConfig.ClaimServiceSettings.MaxPages; i++)
			{
				
				_logger.LogDebug("Facets FXI Service request built for " +
					$"'{_facetsConfig.EndPoint}/{_facetsConfig.ClaimServiceSettings.ServiceAction}'.");

				WebSvcListClaim_v10SoapClient client = WebSvcListClaim_v10SoapClient(
					$"{_facetsConfig.EndPoint}{_facetsConfig.ClaimServiceSettings.ServiceAction}");

				ListClaim_v10Response response = null;

				using (new OperationContextScope(client.InnerChannel))
				{
					HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
					requestMessage.Headers.Add("Authorization", $"Bearer {token}");

					OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] =
						requestMessage;

					response = client.ListClaim_v10_ClaimIdAsync(
						null,
						facetsSearchConfig,
						id,
						i,
						_facetsConfig.ClaimServiceSettings.PageSize,
						0).Result.ListClaim_v10_ClaimIdResult;
				}

				if ((response.CLCL_COLL?.Length ?? 0) < 1)
				{
					getNextPage = false;
					_logger.LogInformation("No more items found, stopping paged calls.");
				}
				else
				{
					claims.AddRange(response.CLCL_COLL);
					_logger.LogInformation("Items found, calling next page.");
				}
			}

			IEnumerable<Claim> mappedClaims = MapDataModels(claims);

			if ((mappedClaims?.Count() ?? 0) > 1)
			{
				throw new ArgumentException($"More than one claim found for a given claim id '{id}'.");
			}

			Claim claim = mappedClaims?
				.FirstOrDefault();

			if (claim == null)
			{
				throw new ArgumentException($"No claim found for claim id '{id}'.");
			}

			return claim;
		}

		private WebSvcListClaim_v10SoapClient WebSvcListClaim_v10SoapClient(
			string endpointAddress)
		{
			BasicHttpsBinding binding = new BasicHttpsBinding(BasicHttpsSecurityMode.Transport);
			binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;

			binding.MaxReceivedMessageSize = int.MaxValue;

			WebSvcListClaim_v10SoapClient serviceClient =
				new WebSvcListClaim_v10SoapClient(binding, new EndpointAddress(endpointAddress));

			return serviceClient;
		}

		
		public IEnumerable<Claim> GetByFilter(Claim filter)
		{

			Facets.Claims.Secure.List.Config facetsSearchConfig =
				new Facets.Claims.Secure.List.Config()
				{
					Region = _facetsConfig.Region,
					FacetsIdentity = _facetsConfig.Identity
				};

			if (!filter.ContrivedSubscriberKey.HasValue)
			{
				throw new ArgumentException("Subscriber Id is required to search for claims.");
			}

			if (!filter.EarliestDateOfService.HasValue && !filter.LatestDateOfService.HasValue)
			{
				throw new ArgumentException(
					"EarliestDateOfService and LatestDateOfService is required to search for claims by Subscriber Id.");
			}

			string token = GetAuthToken(_facetsConfig);

			List<REC_CLCL> claims = new List<REC_CLCL>();

			foreach (string claimType in _facetsConfig.ClaimServiceSettings.ClaimTypeCodes)
			{
				bool getNextPage = true;

				for (int i = 1; getNextPage && i <= _facetsConfig.ClaimServiceSettings.MaxPages; i++)
				{
					WebSvcListClaim_v10SoapClient client = WebSvcListClaim_v10SoapClient(
						$"{_facetsConfig.EndPoint}{_facetsConfig.ClaimServiceSettings.ServiceAction}");

					ListClaim_v10Response response = null;

					using (new OperationContextScope(client.InnerChannel))
					{
						HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
						requestMessage.Headers.Add("Authorization", $"Bearer {token}");

						OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] =
							requestMessage;

						response = client.ListClaim_v10_SubscriberKeyAsync(
							null,
							facetsSearchConfig,
							filter.ContrivedSubscriberKey,
							filter.EarliestDateOfService,
							filter.LatestDateOfService,
							claimType,
							100,
							i,
							_facetsConfig.ClaimServiceSettings.PageSize,
							0).Result.ListClaim_v10_SubscriberKeyResult;
					}

					if ((response.CLCL_COLL?.Length ?? 0) < 1)
					{
						getNextPage = false;
						_logger.LogInformation($"No more items found, stopping on page '{i}'.");
					}
					else
					{
						claims.AddRange(response.CLCL_COLL);
						_logger.LogInformation($"Items found, calling next page '{i + 1}'.");
					}
				}
			}

			IEnumerable<Claim> mappedClaims = MapDataModels(claims)?
				.Where(c => c != null && ClaimComparison(c, filter));

			return mappedClaims;
		}

		public string GetClaimDeniedStatus(
			string claimId)
		{
			string propExCodeString = "000"; // Default
			const string exCode299 = "299";  // Denied Claim
			const string exCodeE78 = "E78";  // Denied Claim
			const string exCodeGIN = "GIN";  // GA MCD Outlier payment

			Facets.Claim.Secure.Cdml.Config facetsCdmlConfig =
				new Facets.Claim.Secure.Cdml.Config()
					{
						Region = _facetsConfig.Region,
						FacetsIdentity = _facetsConfig.Identity
					};

			string token = GetAuthToken(_facetsConfig);

			List<REC_CDML> claimLines = new List<REC_CDML>();

			_logger.LogDebug("Facets FXI Service request built for " +
			                 $"'{_facetsConfig.ServiceUrl}/" +
			                 $"{_facetsConfig.ClaimLinesServiceSettings.ServiceAction}'.");

			WebSvcListClaimCDML_v9SoapClient client = WebSvcListClaimCDML_v9SoapClient(
				$"{_facetsConfig.EndPoint}{_facetsConfig.ClaimLinesServiceSettings.ServiceAction}");

			ListClaimCDML_v9_ClaimIdResponse response = null;

			using (new OperationContextScope(client.InnerChannel))
			{
				HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
				requestMessage.Headers.Add("Authorization", $"Bearer {token}");

				OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] =
					requestMessage;

				 response = client.ListClaimCDML_v9_ClaimIdAsync(
					null,
					facetsCdmlConfig,
					claimId,
					0,
					0,
					0).Result;
			}

			_logger.LogInformation($"Successfully sent request to Facets FXI CDML Service.");

			if (response == null)
			{
				throw new Exception(
					"Internal FXI CDML Service failed to return a response.");
			}

			if (response.ListClaimCDML_v9_ClaimIdResult.CDML_COLL!= null)
			{
				if (response.ListClaimCDML_v9_ClaimIdResult.CDML_COLL
					.Any(code => code.CDML_DISALL_EXCD == exCode299))
				{
					propExCodeString += $"|{exCode299}";
				}

				if (response.ListClaimCDML_v9_ClaimIdResult.CDML_COLL
					.Any(code => code.CDML_DISALL_EXCD == exCodeE78))
				{
					propExCodeString += $"|{exCodeE78}";
				}

				if (response.ListClaimCDML_v9_ClaimIdResult.CDML_COLL
					.Any(code => code.CDML_DISALL_EXCD == exCodeGIN))
				{
					propExCodeString += $"|{exCodeGIN}";
				}
			}

			return propExCodeString;
		}

		private WebSvcListClaimCDML_v9SoapClient WebSvcListClaimCDML_v9SoapClient(
			string endpointAddress)
		{
			BasicHttpsBinding binding = new BasicHttpsBinding(BasicHttpsSecurityMode.Transport);
			binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;

			binding.MaxReceivedMessageSize = int.MaxValue;

			WebSvcListClaimCDML_v9SoapClient serviceClient =
				new WebSvcListClaimCDML_v9SoapClient(binding, new EndpointAddress(endpointAddress));

			return serviceClient;
		}

		private bool ClaimComparison(Claim original,
			Claim filter)
		{
			IEnumerable<PropertyInfo> properties = filter.GetType()
				.GetProperties(BindingFlags.Public | BindingFlags.Instance)
				.Where(p => p.GetValue(filter) != null);

			foreach (PropertyInfo propertyInfo in properties)
			{
				object originalValue = propertyInfo.GetValue(original);
				object filterValue = propertyInfo.GetValue(filter);

				if (!originalValue.Equals(filterValue))
				{
					return false;
				}
			}

			return true;
		}

		private IEnumerable<Claim> MapDataModels(List<REC_CLCL> fxiClaims)
		{
			if ((fxiClaims?.Count ?? 0) < 1)
			{
				return null;
			}

			return fxiClaims
				.Where(c => c != null)
				.Select(c => new Claim
				{
					ClaimId = c.CLCL_ID,
					ProductCategory = c.CSPD_CAT,
					ClaimAssignmentIndicator = c.CLCL_PAY_PR_IND,
					ClaimType = c.CLCL_CL_TYPE,
					ClaimTypeDescription = c.CLCL_CL_TYPE_DESC,
					ClaimSubType = c.CLCL_CL_SUB_TYPE,
					ClaimSubTypeDescription = c.CLCL_CL_SUB_TYPE_DESC,
					ClaimStatusCode = c.CLCL_CUR_STS,
					ClaimStatusDescription = c.CLCL_CUR_STS_DESC,
					ReceivedDate = c.CLCL_RECD_DT,
					EarliestDateOfService = c.CLCL_LOW_SVC_DT,
					LatestDateOfService = c.CLCL_HIGH_SVC_DT,
					TotalCharge = c.CLCL_TOT_CHG,
					TotalPayable = c.CLCL_TOT_PAYABLE,
					PatientPaidAmount = c.CLCL_PA_PAID_AMT,
					PatientAccountNumber = c.CLCL_PA_ACCT_NO,
					CapitationIndicator = c.CLCL_CAP_IND,
					CapitationIndicatorDescription = c.CLCL_CAP_IND_DESC,
					ProviderId = c.PRPR_ID,
					ProviderName = c.PRPR_NAME,
					ProviderNpi = c.PRPR_NPI,
					ContrivedSubscriberKey = c.SBSB_CK,
					SubscriberId = c.SBSB_ID,
					SubscriberSuffix = c.MEME_SFX,
					MemberFirstName = c.MEME_FIRST_NAME,
					MemberLastName = c.MEME_LAST_NAME,
					MemberMiddleInitial = c.MEME_MID_INIT,
					CategoryDescription = c.CSPD_CAT_DESC,
					GroupName = c.GRGR_NAME,
					GroupId = c.GRGR_ID,
					SubGroupName = c.SGSG_NAME,
					PlanId = c.CSPI_ID,
					PlanDescription = c.PLDS_DESC,
					ClaimPayeeIndicator = c.CLCL_PAY_PR_IND_DESC,
					CarrierPaymentAmount = c.CLCB_COB_AMT,
					CarrierDisallowAmount = c.CLCB_COB_DISALLOW,
					CarrierAllowAmount = c.CLCB_COB_ALLOW,
					DateOfLastAction = c.CLCL_LAST_ACT_DTM,
					EnteredDate = c.CLCL_INPUT_DT,
					PaidDate = c.CLCL_PAID_DT,
					NetworkId = c.NWNW_ID,
					NetworkName = c.NWNW_NAME,
					AgreementId = c.AGAG_ID,
					PayeeProviderId = c.CLCL_PAYEE_PR_ID,
					NetworkIndicator = c.CLCL_NTWK_IND,
					HasPrimaryCarePhysician = c.CLCL_PCP_IND,
					ExplanationOfBenefitCode = c.CLCL_AIAI_EOB_IND,
					ProviderEntity = c.PRPR_ENTITY,
					HraIndicator = c.CLCL_HSA_IND,
					HraDescription = c.CLCL_HSA_IND_DESC,
					HraConsiderAmount = c.CLHS_TOT_CONSIDER,
					HraPaidAmount = c.CLHS_TOT_PAID,
					FamilyDeductible = c.SBHS_DED_AMT,
					MemberDeductible = c.SBHS_ME_DED_AMT,
					FamilyDeductibleApplied = c.SBHS_DED_ACCUM_AMT,
					MemberDeductibleApplied = c.MEHS_DED_ACCUM_AMT,
					SubscriberHraPaid = c.SBHS_PAID_AMT,
					MemberHraPaid = c.MEHS_PAID_AMT,
					ProductId = c.PDPD_ID,
					FacilityId = c.CLMF_PRPR_ID_FAC,
					InputTaxId = c.CLMF_INPUT_TXNM_CD,
					AdmissionDate = c.CLHP_ADM_DT,
					DischargeDate = c.CLHP_DC_DT,
					HospitalStatementFromDate = c.CLHP_STAMENT_FR_DT,
					HospitalStatementToDate = c.CLHP_STAMENT_TO_DT,
					OriginallySubmittedSubscriberId = c.CLMF_INPUT_SBSB_ID,
					RelatedFacilityNpi = c.CLMF_PRPR_FA_NPI,
					RelatedFacilityTaxId = c.CLMF_PRPR_FA_TAX,
					ReferringProviderNpi = c.CLMF_PRPR_RF_NPI,
					ReferringProviderTaxId = c.CLMF_PRPR_RF_TAX,
					ServicingProviderNpi = c.CLMF_PRPR_SE_NPI,
					ServicingProviderTaxId = c.CLMF_PRPR_SE_TAX,
					UniqueHealthId = c.CLMF_INPUT_HLTH_ID,
					CodingSystem = c.CLMF_ICD_IND_PROC_DESC,
					TotalPaymentApplied = c.CLSP_TOT_AMT_APLY
				});
		}
	}
}