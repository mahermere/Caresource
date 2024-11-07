// ------------------------------------------------------------------------------------------------
//  <copyright>
//    Copyright (c) CareSource, 2020-2022.  All rights reserved.
// 
//    Claims
//    QueueFacetsDapperAdapter.cs
//  </copyright>
//  ------------------------------------------------------------------------------------------------

namespace Claims.Adapter
{
	using System.Collections.Generic;
	using System.Linq;
	using Claims.Adapter.Interfaces;
	using Claims.Models;
	using Dapper;
	using Microsoft.Data.SqlClient;
	using Microsoft.Extensions.Logging;

	public class QueueFacetsDapperAdapter : IQueueAdapter<FacetsQueueMessage>
	{
		private readonly ILogger<QueueFacetsDapperAdapter> _logger;
		private readonly FacetsConfiguration settings = new FacetsConfiguration();

		public QueueFacetsDapperAdapter(
			IConfiguration settingsAdapter,
			ILogger<QueueFacetsDapperAdapter> logger)
		{
			settingsAdapter.GetSection("Facets").Bind(settings);
			_logger = logger;
		}

		public FacetsQueueMessage GetByClaimId(string claimId)
		{
			string connectionString = new SqlConnectionStringBuilder
			{
				DataSource = settings.DataBaseSettings.DataSource,
				InitialCatalog = settings.DataBaseSettings.InitialCatalog,
				UserID = settings.DataBaseSettings.Username,
				Password = settings.DataBaseSettings.Password,
				Encrypt = false,
				TrustServerCertificate = true
			}.ConnectionString;

			string sql = $"{GetByClaimIdBaseSql()} WHERE WQMS_MESSAGE_ID=@ClaimId";

			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				FacetsQueueMessage result = connection
					.Query<FacetsQueueMessage>(sql, new { ClaimId = claimId })
					.FirstOrDefault();

				return result;
			}
		}

		public List<FacetsQueueMessage> GetByClaimId(List<string> claimIds)
		{


			string connectionString = new SqlConnectionStringBuilder
			{
				DataSource = settings.DataBaseSettings.DataSource,
				InitialCatalog = settings.DataBaseSettings.InitialCatalog,
				UserID = settings.DataBaseSettings.Username,
				Password = settings.DataBaseSettings.Password,
				TrustServerCertificate = true
			}.ConnectionString;

			string sql = $"{GetByClaimIdBaseSql()} WHERE WQMS_MESSAGE_ID IN @ClaimIds";

			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				List<FacetsQueueMessage> results = new List<FacetsQueueMessage>();

				for (int i = 1;
						((i - 1) * settings.DataBaseSettings.QuerySize ?? 1) < claimIds.Count;
						i++)
				{
					List<FacetsQueueMessage> result = connection
						.Query<FacetsQueueMessage>(sql
							, new
							{
								ClaimIds = claimIds
									.Skip((i - 1) * settings.DataBaseSettings.QuerySize ?? 1)
									.Take(settings.DataBaseSettings.QuerySize ?? 100)
									.ToArray()
							})
						.ToList();

					results.AddRange(result);
				}

				return results;
			}
		}

		private string GetByClaimIdBaseSql() =>
			"SELECT WQMS.WQDF_QUEUE_ID as [QueueId], " +
			"WQDF.WQDF_DESC as [QueueName], " +
			"WQMS.WQMS_MESSAGE_ID [MessageId], " +
			"WQMS.WROL_ROLE_ID as [RoleId], " +
			"WROL.WROL_ROLE_DESC as [RoleName], " +
			"WQMS.WMGT_MSG_TYPE as [MessageType], " +
			"WQMS.WQMS_STATUS as [Status], " +
			"WQMS.WQMS_TARGET_DT as [TargetDate], " +
			"WQMS.WQMS_ADJ_TARGET_DT [AdjustedTargetDate], " +
			"WQMS.WQMS_RECEIVED_DT as [ReceivedDate]" +
			"FROM FACPRDDB.[dbo].[NWX_WQMS_QUEUE_MSG] WQMS(NOLOCK) " +
			"INNER JOIN FACPRDDB.[dbo].[NWX_WQDF_QUEUE_DEF] WQDF (NOLOCK) ON WQMS.WQDF_QUEUE_ID = WQDF.WQDF_QUEUE_ID " +
			"INNER JOIN FACPRDDB.[dbo].[NWX_WROL_ROLES] WROL (NOLOCK) ON WQMS.WROL_ROLE_ID = WROL.WROL_ROLE_ID ";
	}
}