// ------------------------------------------------------------------------------------------------
//  <copyright>
//    Copyright (c) CareSource, 2020-2022.  All rights reserved.
// 
//    Claims
//    DCNFWSApiAdapter.cs
//  </copyright>
//  ------------------------------------------------------------------------------------------------

namespace Claims.Adapter
{
	using Claims.Adapter.Interfaces;
	using Claims.Models;
	using Facets.Claim.GetByImageId;
	using Microsoft.Extensions.Logging;

	public class DCNFWSApiAdapter : IDCNDataAdapter<DCNClaimData>
	{
		private readonly ILogger<DCNFWSApiAdapter> _logger;
		private readonly FacetsConfiguration _settings = new FacetsConfiguration();

		public DCNFWSApiAdapter(
			IConfiguration settingsAdapter,
			ILogger<DCNFWSApiAdapter> logger)
		{
			settingsAdapter.GetSection("Facets").Bind(_settings);
			_logger = logger;
		}

		public DCNClaimData GetDataByDCN(string microId)
		{
			Facets.Claim.GetByImageId.GetCLaimByImageIDRequest facetsImageIdRequest =
				new Facets.Claim.GetByImageId.GetCLaimByImageIDRequest()
				{
					ImageID = microId
				};

			FacetsWebServiceClient client = new FacetsWebServiceClient(
				FacetsWebServiceClient.EndpointConfiguration.basicHttpsWindows,
				_settings.ClaimDcnServiceSettings.ServiceUrl);

			GetCLaimByImageIDResponse response =
				client.GetClaimByImageIDAsync(facetsImageIdRequest)
					.Result;//.GetClaimByImageIDResult;

			DCNClaimData dcnClaimData = LoadDCNClaimData(response);

			return dcnClaimData;
		}

		private DCNClaimData LoadDCNClaimData(
			GetCLaimByImageIDResponse response)
			=> new DCNClaimData()
			{
				ClaimId = response.ClaimNumber,
				MemberId = response.MemberID,
				MemberSuffix = $"{response.MemberSuffix:D2}",
				SubscriberId = response.SubscriberID
			};
	}
}