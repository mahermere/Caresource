// ------------------------------------------------------------------------------------------------
//  <copyright>
//    Copyright (c) CareSource, 2020-2022.  All rights reserved.
// 
//    Claims
//    MemberFacetsApiAdapter.cs
//  </copyright>
//  ------------------------------------------------------------------------------------------------

namespace Claims.Adapter
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.ServiceModel;
	using System.ServiceModel.Channels;
	using Claims.Adapter.Interfaces;
	using Claims.Models;
	using Claims.OnBase.Utilities;
	using Facets.Member.Secure.Search;
	using Microsoft.Extensions.Logging;

	public class MemberFacetsApiAdapter : FXIAuthAdapter, IMemberAdapter<Member>
	{
		private readonly ILogger<MemberFacetsApiAdapter> _logger;
		private readonly FacetsConfiguration facetsConfig;

		public MemberFacetsApiAdapter(
			IConfiguration settingsAdapter,
			ILogger<MemberFacetsApiAdapter> logger)
		{
			facetsConfig = new FacetsConfiguration();

			settingsAdapter.GetSection("Facets").Bind(facetsConfig);
			_logger = logger;
		}

		public Member GetById(
			string subscriberId,
			int? memberSuffix)
		{
			if (subscriberId.IsNullOrWhiteSpace() || !memberSuffix.HasValue)
			{
				throw new ArgumentException(
					"Both subscriber id and suffix are required to retrieve member information from Facets.");
			}
			
			Config facetsSearchConfig =
				new Config()
				{
					Region = facetsConfig.Region,
					FacetsIdentity = facetsConfig.Identity
				};

			string token = GetAuthToken(facetsConfig);

			WebSvcSearchMember_v3SoapClient client = GetWebSvcSearchMember_v3SoapClient(
				$"{facetsConfig.EndPoint}{facetsConfig.MemberServiceSettings.ServiceAction}");

			SearchMember_v3Response response = null;

			using (new OperationContextScope(client.InnerChannel))
			{
				HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
				requestMessage.Headers.Add("Authorization", $"Bearer {token}");

				OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] =
					requestMessage;

				response = client.SearchMember_v3_SubscriberIdAsync(
					null,
					facetsSearchConfig,
					subscriberId).Result.SearchMember_v3_SubscriberIdResult;
			}

			_logger.LogDebug("Facets FXI Service request built for " +
				$"'{facetsConfig.ServiceUrl}/{facetsConfig.MemberServiceSettings.ServiceAction}'.");

			IEnumerable<Member> mappedMembers = MapDataModels(response?
					.MEV0_COLL?
					.ToList())?
				.Where(m =>
					m.SubscriberId == subscriberId &&
					m.Suffix == memberSuffix.ToString());

			Member member = mappedMembers?
				.FirstOrDefault();

			if (member == null)
			{
				throw new ArgumentException(
					$"No member found for subscriber id '{subscriberId}' and suffix '{memberSuffix}'.");
			}

			if ((mappedMembers?.Count() ?? 0) > 1)
			{
				throw new ArgumentException(
					$"More than one member found for a given subscriber id '{subscriberId}' and suffix '{memberSuffix}'.");
			}

			return member;
		}

	private WebSvcSearchMember_v3SoapClient GetWebSvcSearchMember_v3SoapClient(
			string endpointAddress)
		{
			BasicHttpsBinding binding = new BasicHttpsBinding(BasicHttpsSecurityMode.Transport);
			binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;

			binding.MaxReceivedMessageSize = int.MaxValue;

			WebSvcSearchMember_v3SoapClient serviceClient =
				new WebSvcSearchMember_v3SoapClient(binding, new EndpointAddress(endpointAddress));

			return serviceClient;
		}

		private IEnumerable<Member> MapDataModels(List<REC_MEME> fxiMembers)
		{
			if ((fxiMembers?.Count ?? 0) < 1)
			{
				return null;
			}

			return fxiMembers
				.Where(m => m != null)
				.Select(m => new Member
				{
					ContrivedKey = m.SBSB_CK,
					DateOfBirth = m.MEME_BIRTH_DT,
					Email = m.SBAD_EMAIL_HOME,
					FirstName = m.MEME_FIRST_NAME,
					Hicn = m.MEME_HICN,
					HomeAddress = new Address
					{
						Line1 = m.SBAD_ADDR1_HOME,
						Line2 = m.SBAD_ADDR2_HOME,
						Line3 = m.SBAD_ADDR3_HOME,
						City = m.SBAD_CITY_HOME,
						State = m.SBAD_STATE_HOME,
						Zip = m.SBAD_ZIP_HOME
					},
					LastName = m.MEME_LAST_NAME,
					MedicaidId = m.MEME_MEDCD_NO,
					MiddleInitial = m.MEME_MID_INIT,
					Phone = m.SBAD_PHONE_HOME,
					SubscriberId = m.SBSB_ID,
					Suffix = m.MEME_SFX
				});
		}
	}
}