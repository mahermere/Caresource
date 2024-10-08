﻿// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2021. All rights reserved.
// 
//   WC.Services.Document
//   OnBaseSqlAdapter.Search.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.Document.Adapters.v6
{
	using System;
	using System.Collections.Generic;
	using System.Data;
	using System.Data.SqlClient;
	using System.Linq;
	using System.Reflection;
	using CareSource.WC.Entities.Documents;
	using CareSource.WC.OnBase.Core.ExtensionMethods;
	using CareSource.WC.Services.Document.Models.v6;
	using Dapper;
	using Hyland.Unity;
	using ConfigurationManager = System.Configuration.ConfigurationManager;
	using DocumentHeader = CareSource.WC.Services.Document.Models.v6.DocumentHeader;

	public partial class OnBaseSqlAdapter
	{
		public ISearchResult<DocumentHeader> Search(ISearchRequest request)
		{
			_logger.LogDebug("starting Search Document");

			SetupInstanceVariables(request);

			(string kwFrom, string kwWhere, string kwSelect) kwStatements =
				CreateKeywordStatements(request);

			string sql;

			if (request.Filters.Any(f => f.Name == "Claim Number") ||
				request.DisplayColumns.Any(dc => dc == "Claim Number") ||
				request.Filters.Any(f => f.Name == "Subscriber ID") ||
				request.DisplayColumns.Any(dc => dc == "Subscriber ID") ||
				request.Filters.Any(f => f.Name == "Member ID") ||
				request.DisplayColumns.Any(dc => dc == "Member ID") ||
				request.Filters.Any(f => f.Name == "Provider ID") ||
				request.DisplayColumns.Any(dc => dc == "Provider ID"))
			{
				string orderBy = CreateOrderByClauseWorkView(request.Sorting);
				string rowSelect = GetRowSelect(
					request.DisplayColumns.Any(),
					orderBy);

				(string wvFrom, string wvWhere, string wvSelect) wvStatements =
					CreateWorkViewStatements(request);

				string select =
					$"		DocumentId,{_newLine}" +
					$"		DocumentDate,{_newLine}" +
					$"		DocumentName,{_newLine}" +
					$"		DocumentType,{_newLine}" +
					string.Join(
						$", {_newLine}",
						request.DisplayColumns.Select(d => $"		[{d}]")) +
					rowSelect;

				sql = $"With Documents as({_newLine}" +
					$"	SELECT{_newLine}" +
					$"{select}{_newLine}" +
					$"	FROM({_newLine}" +
					$"		{kwStatements.kwSelect}{_newLine}" +
					$"		{kwStatements.kwFrom}{_newLine}" +
					$"		{kwStatements.kwWhere}{_newLine}" +
					$"	UNION{_newLine}" +
					$"		{wvStatements.wvSelect}{_newLine}" +
					$"		{wvStatements.wvFrom}{_newLine}" +
					$"		{wvStatements.wvWhere}{_newLine}" +
					$"		) AS Docs{_newLine}" +
					$"	) {_newLine}" +
					$"SELECT{_newLine}" +
					$"	*,{_newLine}" +
					$"	(SELECT MAX(Row) FROM Documents) AS [Total Records]{_newLine}" +
					$"FROM{_newLine}" +
					$"	Documents{_newLine}" +
					$"WHERE{_newLine}" +
					"	Row Between " +
					$"{((request.Paging.PageNumber - 1) * request.Paging.PageSize) + 1} AND " +
					$"{request.Paging.PageNumber * request.Paging.PageSize};{_newLine}";
			}
			else
			{
				string orderBy = CreateOrderByClause(request.Sorting);
				string rowSelect = GetRowSelect(
					request.DisplayColumns.Any(),
					orderBy);

				sql = $"With Documents as({_newLine}" +
					$"{kwStatements.kwSelect}{(request.DisplayColumns.Any() ? "": ",")}" +
					$"{rowSelect}{kwStatements.kwFrom}{kwStatements.kwWhere}){_newLine}" +
					$"SELECT{_newLine}" +
					$"	*,{_newLine}" +
					$"	(SELECT MAX(Row) FROM Documents) AS [Total Records]{_newLine}" +
					$"FROM{_newLine}" +
					$"	Documents{_newLine}" +
					$"WHERE{_newLine}" +
					"	Row Between " +
					$"{((request.Paging.PageNumber - 1) * request.Paging.PageSize) + 1} AND " +
					$"{request.Paging.PageNumber * request.Paging.PageSize};{_newLine}";
			}

			List<DocumentHeader> items = new List<DocumentHeader>();

			int totalRecords = 0;

			using (IDbConnection db = new SqlConnection(
				ConfigurationManager.ConnectionStrings["OnBase.ConnectionString"]
					.ConnectionString))
			{
				using (IDataReader docs = db.ExecuteReader(
					sql,
					commandTimeout: Timeout))
				{
					while (docs.Read())
					{
						totalRecords = Convert.ToInt32(docs.GetValue(docs.GetOrdinal("Total Records")));

						items.Add(
							new DocumentHeader
							{
								DocumentName = docs.GetString(docs.GetOrdinal("DocumentName"))
									.SafeTrim(),
								DocumentId = Convert.ToInt64(docs.GetValue(docs.GetOrdinal("DocumentId"))),
								DocumentDate = docs.GetDateTime(docs.GetOrdinal("DocumentDate")),
								DocumentType = FindProperCaseDocType(docs.GetString(docs.GetOrdinal("DocumentType"))
									.SafeTrim()),
								DisplayColumns = request.DisplayColumns
									.ToDictionary(
										dc => dc,
										dc => docs.GetValue(docs.GetOrdinal(dc))
											.ToString()
											.SafeTrim() as object)
							});
					}
				}
			}

			return new SearchResult
			{
				Documents = items.AsEnumerable(),
				SuccessRecordCount = totalRecords
			};
		}

		private (string kwFrom, string kwWhere, string kwSelect) CreateKeywordStatements(
			ISearchRequest request)
		{
			(string kwFrom, string kwWhere) = CreateFromAndWhereKeyword(request);

			IEnumerable<KeywordType> displayColumns =
				_onBaseAdapter.GetKeywordTypes(request.DisplayColumns);

			string kwSelect = CreateSelectStatements(displayColumns);

			return (kwFrom, kwWhere, kwSelect);
		}

		private string CreateSelectStatements(
			IEnumerable<KeywordType> displayColumns)
		{
			return $"SELECT{_newLine}" +
				GetItemDataSelect() +
				GetDocumentTypeSelect() +
				GetDisplayColumnsSelect();


			string GetDisplayColumnsSelect()
			{
				return displayColumns.Any()
					? $",{_newLine} " +
					string.Join(
						$",{_newLine}",
						displayColumns
							.Select(
								dc =>
								{
									switch (dc.Name)
									{
										case "Claim Number":
											return $"	[{dc.Name}].KeyValueChar [{dc.Name}]";
										case "Member Facing":
										case "Provider Facing":
											return $"'' [{dc.Name}]";
										default:
											int datatype = Convert.ToInt32(
												dc.GetType()
													.GetField(
														"_dataType",
														BindingFlags.NonPublic | BindingFlags.Instance)
													.GetValue(dc));

											return
												$"	[{dc.Name}].{_columnDataTypeMaps[datatype]} [{dc.Name}]";
									}
								}))
					: _newLine;
			}
		}

		private string CreateSelectStatementsWorkView(
			IEnumerable<KeywordType> displayColumns)
		{
			return $"SELECT{_newLine}" +
				GetItemDataSelect() +
				GetDocumentTypeSelect() +
				GetDisplayColumnsSelect();

			string GetDisplayColumnsSelect()
			{
				return displayColumns.Any()
					? $",{_newLine}" +
					string.Join(
						$",{_newLine}",
						displayColumns
							.Select(
								dc =>
								{
									switch (dc.Name)
									{
										case "Claim Number":
											return
												$"		COALESCE([{dc.Name}].KeyValueChar, [ClaimNumber]) [{dc.Name}]";
										case "Subscriber ID":
											return
												$"		COALESCE([{dc.Name}].KeyValueChar, [SubscriberId]) [{dc.Name}]";
										case "Member Suffix":
											return
												$"		COALESCE([{dc.Name}].KeyValueChar, [MemberSuffix]) [{dc.Name}]";
										case "Member ID":
											return
												$"		COALESCE([{dc.Name}].KeyValueChar, [MemberId]) [{dc.Name}]";
										case "Provider ID":
											return
												$"		COALESCE([{dc.Name}].KeyValueChar, [ProviderId]) [{dc.Name}]";
										case "Member Facing":
										case "Provider Facing":
											return $"		'' [{dc.Name}]";
										default:
											int dataType = Convert.ToInt32(
												dc.GetType()
													.GetField(
														"_dataType",
														BindingFlags.NonPublic | BindingFlags.Instance)
													?.GetValue(dc));

											return
												$"		[{dc.Name}].{_columnDataTypeMaps[dataType]} [{dc.Name}]";
									}
								}))
					: _newLine;
			}
		}

		private (string wvFrom, string wvWhere, string wvSelect) CreateWorkViewStatements(
			ISearchRequest request)
		{
			(string wvFrom, string wvWhere) = CreateFromAndWhereWorkView(request);

			IEnumerable<KeywordType> displayColumns =
				_onBaseAdapter.GetKeywordTypes(request.DisplayColumns);

			string wvSelect = CreateSelectStatementsWorkView(
				displayColumns);

			return (wvFrom, wvWhere, wvSelect);
		}

		private string GetDocumentTypeSelect()
			=> $"	COALESCE([{_letterTypeKeyword.Name}].KeyValueChar," +
				"DocumentType.ItemTypeName) DocumentType";

		private string GetItemDataSelect()
			=> $"	Document.ItemNum DocumentId,{_newLine}" +
				$"	Document.ItemDate DocumentDate,{_newLine}" +
				$"	Document.ItemName DocumentName,{_newLine}";

		private string GetRowSelect(
			bool displayColumns,
			string orderBy)
			=> $"{(displayColumns ? $",{_newLine}" : string.Empty)}" +
				$"		ROW_NUMBER() OVER({orderBy}) [Row]{_newLine}";
	}
}