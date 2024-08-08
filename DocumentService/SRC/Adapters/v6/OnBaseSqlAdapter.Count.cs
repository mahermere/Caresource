// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   WC.Services.Document
//   OnBaseSqlAdapter.Count.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.Document.Adapters.v6
{
	using System;
	using System.Collections.Generic;
	using System.Data;
	using System.Data.SqlClient;
	using System.Linq;
	using CareSource.WC.OnBase.Core.ExtensionMethods;
	using CareSource.WC.Services.Document.Models.v6;
	using Dapper;
	using ConfigurationManager = System.Configuration.ConfigurationManager;

	public partial class OnBaseSqlAdapter
	{
		public long TotalRecords(IFilteredRequest request)
		{
			_logger.LogDebug(
				$"Starting {nameof(OnBaseAdapter)}.{nameof(TotalRecords)}",
				new Dictionary<string, object>() { { "request", request } });

			SetupInstanceVariables(request);

			(string from, string where) = CreateFromAndWhere(request);

			string select = $"SELECT{_newLine}" +
				$"	COUNT(DISTINCT(Document.ItemNum)) TotalItems{_newLine}";

			string sql = $"{@select} {@from} {@where}{_newLine}";
			long count = 0;

			using (IDbConnection db = new SqlConnection(
				ConfigurationManager.ConnectionStrings["OnBase.ConnectionString"]
					.ConnectionString))
			{
				count = db.QuerySingle<long>(
					sql,
					commandTimeout: Timeout);
			}

			return count;
		}


		public IDictionary<string, int> DocumentTypesCount(IFilteredRequest request)
		{
			_logger.LogDebug(
				$"Starting {nameof(DocumentTypesCount)}",
				new Dictionary<string, object> { { "request", request } });

			SetupInstanceVariables(request);

			(string from, string where) = CreateFromAndWhere(request);

			string docTypeCoalesce =
				$"	COALESCE([{_letterTypeKeyword.Name}].KeyValueChar,DocumentType.ItemTypeName)";

			string select =
				$"SELECT{_newLine}" +
				$"	{docTypeCoalesce} DocumentType,{_newLine}" +
				"	Count(DISTINCT Document.ItemNum) DocumentTypeCount";

			string groupBy =
				$"GROUP BY{_newLine}" +
				$"{docTypeCoalesce}{_newLine}";

			string orderBy =
				$"ORDER BY{_newLine}" +
				$"{docTypeCoalesce}{_newLine}";

			string sql = $"{@select} {@from} {@where} {groupBy} {orderBy}{_newLine}";

			using (IDbConnection db = new SqlConnection(
				ConfigurationManager.ConnectionStrings["OnBase.ConnectionString"]
					.ConnectionString))
			{
				IEnumerable<dynamic> docs = db.Query(
					sql,
					commandTimeout: Timeout);

				return docs.ToDictionary<dynamic, string, int>(
					doc => FindProperCaseDocType(Convert.ToString(doc.DocumentType).Trim()),
					doc => Convert.ToInt32(doc.DocumentTypeCount));
			}
		}
	}
}