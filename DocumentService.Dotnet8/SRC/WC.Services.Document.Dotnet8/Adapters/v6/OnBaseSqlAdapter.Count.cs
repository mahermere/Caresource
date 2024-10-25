// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   WC.Services.Document
//   OnBaseSqlAdapter.Count.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.Document.Dotnet8.Adapters.v6
{
	using System;
	using System.Collections.Generic;
	using System.Data;
	using System.Data.SqlClient;
	using System.Linq;
    using WC.Services.Document.Dotnet8.Models.v6;
    using WC.Services.Document.Dotnet8.Models.v6.Interfaces;


    public partial class OnBaseSqlAdapter
	{
		public long TotalRecords(IFilteredRequest request)
		{
			_logger.Debug(
				$"Starting {nameof(OnBaseAdapter)}.{nameof(TotalRecords)}"+ new Dictionary<string, object>() { { "request", request } });

			SetupInstanceVariables(request);

			(string from, string where) = CreateFromAndWhere(request);

			string select = $"SELECT{_newLine}" +
				$"	COUNT(DISTINCT(Document.ItemNum)) TotalItems{_newLine}";

			string sql = $"{@select} {@from} {@where}{_newLine}";
			long count = 0;

            using (SqlConnection db = new SqlConnection(_configuration["OnBaseSettings:OnBase.ConnectionString"]))
            {
                using (SqlCommand command = new SqlCommand(sql, db))
                {
                    command.CommandTimeout = Timeout;
                    count = (long)command.ExecuteScalar();
                }
            }

            return count;
        }


		public IDictionary<string, int> DocumentTypesCount(IFilteredRequest request)
		{
			_logger.Debug(
				$"Starting {nameof(DocumentTypesCount)}"+new Dictionary<string, object> { { "request", request } });

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

            //using (IDbConnection db = new SqlConnection(
            //	_configuration["OnBaseSettings:OnBase.ConnectionString"]))

            //{
            //	IEnumerable<dynamic> docs = db.Query(
            //		sql,
            //		commandTimeout: Timeout);

            //	return docs.ToDictionary<dynamic, string, int>(
            //		doc => FindProperCaseDocType(Convert.ToString(doc.DocumentType).Trim()),
            //		doc => Convert.ToInt32(doc.DocumentTypeCount));
            //}
            using (IDbConnection db = new SqlConnection(_configuration["OnBaseSettings:OnBase.ConnectionString"]))
            {
                db.Open();
                var command = db.CreateCommand();
                command.CommandText = sql;
                command.CommandTimeout = Timeout;

                var docs = new Dictionary<string, int>();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string documentType = reader["DocumentType"].ToString().Trim();
                        int documentTypeCount = Convert.ToInt32(reader["DocumentTypeCount"]);

                        docs[FindProperCaseDocType(documentType)] = documentTypeCount;
                    }
                }

                return docs;
            }
        }
	}
}