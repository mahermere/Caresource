// ------------------------------------------------------------------------------------------------
//  <copyright>
//    Copyright (c) CareSource, 2020-2022.  All rights reserved.
// 
//    Claims
//    ClaimsManager.cs
//  </copyright>
//  ------------------------------------------------------------------------------------------------

namespace Claims.Managers
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Claims.Adapter.Interfaces;
	using Claims.Managers.Interfaces;
	using Claims.Models;
	using Microsoft.Extensions.Logging;

	public class ClaimsManager : IClaimsManager
	{
		private readonly IClaimsAdapter<Claim> _claimsAdapter;
		private readonly IDCNDataAdapter<DCNClaimData> _dcnDataAdapter;
		private readonly ILogger<ClaimsManager> _logger;
		private readonly IMemberAdapter<Member> _memberAdapter;
		private readonly IQueueAdapter<FacetsQueueMessage> _queueAdapter;
		private readonly FacetsConfiguration _facetsConfig;

		public ClaimsManager(
			IMemberAdapter<Member> memberAdapter,
			IClaimsAdapter<Claim> claimsAdapter,
			IQueueAdapter<FacetsQueueMessage> queueAdapter,
			IDCNDataAdapter<DCNClaimData> dcnDataAdapter,
			IConfiguration settingsAdapter,
			ILogger<ClaimsManager> logger)
		{
			_claimsAdapter = claimsAdapter;
			_memberAdapter = memberAdapter;
			_queueAdapter = queueAdapter;

			_facetsConfig = new FacetsConfiguration();

			settingsAdapter.GetSection("Facets").Bind(_facetsConfig);
			_dcnDataAdapter = dcnDataAdapter;
			_logger = logger;
		}

		public Claim GetById(string id)
		{
			Claim claim = _claimsAdapter.GetById(id);

			if (claim != null)
			{
				FacetsQueueMessage queueMessage = _queueAdapter.GetByClaimId(id);
				claim.QueueId = queueMessage?.QueueId;
				claim.QueueName = queueMessage?.QueueName;
				claim.RoleId = queueMessage?.RoleId;
				claim.RoleName = queueMessage?.RoleName;
			}

			return claim;
		}

		public List<Claim> GetByFilter(int? page,
			int? pageSize,
			Claim filter)
		{
			if (!page.HasValue || page.Value < 1)
			{
				page = 1;
				_logger.LogInformation("No page number given, defaulted page number to '1'.");
			}

			if (!pageSize.HasValue)
			{
				pageSize = 100;
				_logger.LogInformation("No page number given, defaulted page size to '100'.");
			}

			if (pageSize > _facetsConfig.ClaimServiceSettings.PageSize)
			{
				throw new ArgumentException(
					$"Page size cannot be bigger than '{_facetsConfig.ClaimServiceSettings.PageSize}'.");
			}

			Member member = _memberAdapter.GetById(filter.SubscriberId, filter.SubscriberSuffix);

			if (member?.ContrivedKey == null)
			{
				throw new ArgumentException(
					$"Could not find member for subscriber id '{filter.SubscriberId}'.");
			}

			filter.ContrivedSubscriberKey = member.ContrivedKey;

			List<Claim> claims = _claimsAdapter.GetByFilter(filter)?
				.Skip((page.Value - 1) * pageSize.Value)
				.Take(pageSize.Value)
				.Where(c => c != null)
				.ToList();

			if ((claims?.Count ?? 0) > 0)
			{
				List<FacetsQueueMessage> queues = _queueAdapter.GetByClaimId(
					claims
						.Select(c => c.ClaimId)
						.ToList());

				foreach (FacetsQueueMessage queueMessage in queues)
				{
					Claim claim = claims?
						.FirstOrDefault(c => c.ClaimId == queueMessage.MessageId);

					if (claim == null)
					{
						continue;
					}

					claim.QueueId = queueMessage?.QueueId;
					claim.QueueName = queueMessage?.QueueName;
					claim.RoleId = queueMessage?.RoleId;
					claim.RoleName = queueMessage?.RoleName;
				}
			}

			return claims;
		}

		public DCNClaimData GetDataByDCN(string id)
		{
			DCNClaimData dcnClaimData = _dcnDataAdapter.GetDataByDCN(id);

			return dcnClaimData;
		}

		public string GetClaimDeniedStatus(
			string id)
		{
			string claimDeniedStatus = _claimsAdapter.GetClaimDeniedStatus(id);

			return claimDeniedStatus;
		}
	}
}