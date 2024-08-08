// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2023. All rights reserved.
// 
//   WC.Services.Document
//   SqlAdapter.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.Document.Adapters.v6
{
	using System;
	using System.Collections.Generic;
	using System.Data;
	using System.Data.SqlClient;
	using System.Linq;
	using CareSource.WC.Entities.Requests.Base;
	using CareSource.WC.OnBase.Core.ExtensionMethods;
	using CareSource.WC.Services.Document.Models.v6;
	using Dapper;
	using Hyland.Unity;
	using Hyland.Unity.WorkView;
	using Microsoft.Extensions.Logging;
	using ConfigurationManager = System.Configuration.ConfigurationManager;
	using Filter = CareSource.WC.Entities.Common.Filter;

	public class SqlAdapter : ISqlAdapter
	{
		private const string LetterTypeKeywordName = "Letter Type";
		private const string MemberFacingKeywordName = "Member Facing";
		private const string ProviderFacingKeywordName = "Provider Facing";
		private const int DualTableDataTypeId = 2;

		private readonly ILogger _logger;
		private readonly IOnBaseAdapter _onBaseAdapter;
		private readonly string _newLine = Environment.NewLine;
		private IEnumerable<string> _distinctFilterNames;
		private readonly int _timeout;
		private readonly IEnumerable<string> _LetterTypes;
		private KeywordType _letterTypeKeyword;
		private IEnumerable<string> _letterTypes;
		private IEnumerable<DocumentType> _documentTypes;

		private bool _memberFacing = false;
		private bool _providerFacing = false;

		public SqlAdapter(
			ILogger logger,
			IOnBaseAdapter onBaseAdapter)
		{
			_logger = logger;
			_onBaseAdapter = onBaseAdapter;
			_timeout = Convert.ToInt32(
				ConfigurationManager.AppSettings.Get("OnBase.Connection.Timeout"));
		}

		public ISearchResult<DocumentHeader> SearchDocuments(ISearchRequest request)
		{

			string methodName = $"{nameof(SqlAdapter)}.{nameof(SearchDocuments)}";
			_logger.LogDebug($"Starting: {methodName}");
			SetupInstanceVariables(request);
			string docTypeWhere = CreateDocumentTypeWhere(request.DocumentTypes);

			string dateWhere = CreateDateWhere(
				request.StartDate,
				request.EndDate);

			List<string> filterSelects = CreateFilterSelects(
				request.Filters,
				dateWhere);

			IEnumerable<KeywordType> displayKeywords = _onBaseAdapter.GetKeywordTypes(
					request.DisplayColumns)
				.Distinct();



			string select = $"WITH {string.Join($",{_newLine}", filterSelects)}";
			string filterJoins = CreateFilterJoins(request.Filters);
			string displayColumns = CreateDisplayColumns(request.DisplayColumns);
			string sorting = "";
			string paging = CreatePaging(request.Paging);


			select +=
				$", Documents AS({_newLine}" +
				$"SELECT{_newLine}" +
				$"	[Document].ItemNum DocumentId,{_newLine}" +
				$"	[Document].ItemDate DocumentDate,{_newLine}" +
				$"	[Document].ItemName DocumentName,{_newLine}" +
				$"	[DocumentType].ItemTypeName DocumentType,{_newLine}" +
				$"{displayColumns}" +
				$"	ROW_NUMBER() OVER(ORDER BY [Document].ItemDate DESC) [Row] {_newLine}" +
				$"FROM {_newLine}" +
				$"	ItemData [Document] with(NoLock) {_newLine}" +
				$"{filterJoins}" +
				$"	INNER JOIN DocType [DocumentType] with(NoLock) {_newLine}" +
				$"		ON [Document].ItemTypeNum = [DocumentType].ItemTypeNum {_newLine}" +
				$"WHERE {_newLine}" +
				$"	Document.Status != '16'{_newLine}" +
				//$"	{docTypeWhere}{_newLine}" +
				$"	{dateWhere}){_newLine}" +
				$"SELECT{_newLine}" +
				$"	*,{_newLine}" +
				$"	(SELECT MAX(Row) FROM Documents) AS [Total Records]{_newLine}" +
				$"FROM {_newLine}	Documents{_newLine}" +
				$"WHERE{_newLine}" +
				$"{paging}{_newLine}" +
				$"{sorting}{_newLine}";


			ISearchResult<DocumentHeader> docs = ExecuteSql(
				select,
				request.DisplayColumns);

			_logger.LogDebug(
				$"Results: {methodName}",
				new Dictionary<string, object>
				{
					{ "Results", docs },
				});

			_logger.LogDebug($"Finished: {methodName}");
			return docs;
		}


		private ISearchResult<DocumentHeader> ExecuteSql(
			string sql,
			IEnumerable<string> displayColumns)
		{
			int totalRecords = 0;
			List<DocumentHeader> items = new List<DocumentHeader>();

			using (IDbConnection db = new SqlConnection(
						ConfigurationManager.ConnectionStrings["OnBase.ConnectionString"]
							.ConnectionString))
			{
				using (IDataReader docs = db.ExecuteReader(
							sql,
							commandTimeout: _timeout))
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
								DocumentType = FindProperCaseDocType(
									docs.GetString(docs.GetOrdinal("DocumentType"))
										.SafeTrim()),
								DisplayColumns = displayColumns?
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
				SuccessRecordCount = totalRecords,
				Documents = items
			};
		}

		private string CreateDateWhere(
			DateTime? startDate,
			DateTime? endDate)
			=> startDate.HasValue && endDate.HasValue
				? "AND Document.ItemDate BETWEEN " +
				$"'{startDate.Value.Date:s}'" +
				$" AND '{endDate.Value.Date.AddDays(1).AddSeconds(-1):s}'{_newLine}"
				: string.Empty;

		private string CreatePaging(Paging paging)
			=> "	[Row] Between " +
				$"{((paging.PageNumber - 1) * paging.PageSize) + 1} AND " +
				$"{paging.PageNumber * paging.PageSize};{_newLine}";

		private string CreateDisplayColumns(IEnumerable<string> requestDisplayColumns)
		{
			if (requestDisplayColumns.SafeAny())
			{
				return string.Join(
						$",{_newLine}",
						requestDisplayColumns.Select(dc => $"	[{dc}]")) +
					$",{_newLine}";
			}

			return string.Empty;
		}

		private string CreateFilterJoins(IEnumerable<Filter> requestFilters)
		{
			string filterJoins = string.Empty;
			string joinType;

			foreach (string f in _distinctFilterNames)
			{
				joinType = requestFilters
					.Where(rf => rf.Name == f)
					.Any(rf => rf.IncludeNull)
					? "LEFT"
					: "INNER";

				filterJoins +=
					$"	{joinType} JOIN [{f}]{_newLine}" +
					$"		ON Document.ItemNum = [{f}].ItemNum {_newLine}";
			}

			return filterJoins;
		}


		private string FindProperCaseDocType(string docType)
			=> _letterTypes.Any(
				lt => lt.Equals(
					docType,
					StringComparison.InvariantCultureIgnoreCase))
				? _letterTypes.First(
					lt => lt.Equals(
						docType,
						StringComparison.InvariantCultureIgnoreCase))
				: docType;

		private string CreateDocumentTypeWhere(IEnumerable<string> docTypeNames)
			=> $"AND [DocumentType].[ItemTypeNum] IN ({string.Join(", ", _documentTypes.Select(dt => dt.ID))})";

		private List<string> CreateFilterSelects(
			IEnumerable<Filter> filters,
			string dateWhere)
		{
			_distinctFilterNames = filters.Select(f => f.Name)
				.Distinct();

			IEnumerable<KeywordType> filterKeywords = _onBaseAdapter.GetKeywordTypes(_distinctFilterNames);

			List<string> selects = new List<string>();

			var lastKwName = string.Empty;

			foreach (string keywordName in _distinctFilterNames)
			{
				KeywordType kt = filterKeywords.First(
					f => f.Name.Equals(
						keywordName,
						StringComparison.CurrentCultureIgnoreCase));

				//Get the Datatype, this is needed because of Dual table Keywords
				long dataType = _onBaseAdapter.GetKeyTypeTableDataType(kt);

				string values =
					string.Join(
						", ",
						filters
							.Where(f => f.Name == keywordName)
							.Select(f => "'" + f.Value + "'"));

				string cpsSelect = GetCpsSelect(
					keywordName,
					values,
					dateWhere);

				string select = string.Empty;
				if (dataType == DualTableDataTypeId) //2 = Dual Table Alpha
				{
					select = GetDualTableSelect(
						kt,
						cpsSelect,
						values,
						dateWhere);
				}
				else
				{
					select =
						GetSingleTableSelect(
							kt,
							cpsSelect,
							values,
							dateWhere);
				}
			
				selects.Add(select);
			}

			return selects;
		}

		/// <summary>
		/// CPS is the Claims Performance Solution
		/// </summary>
		/// <param name="name"></param>
		/// <param name="keywordName"></param>
		/// <param name="values"></param>
		/// <param name="dateWhere"></param>
		/// <returns></returns>
		private string GetCpsSelect(
			string keywordName,
			string values,
			string dateWhere)
		{
			IEnumerable<Class> classes =
				_onBaseAdapter.GetWorkViewClasses(
					ConfigurationManager.AppSettings["WorkView.Application"]);

			long atttibuteId = 0;

			Class claimClass = classes.First(c => c.Name == "Claims");
			Class documentClass = classes.First(c => c.Name == "Documents");
			long fkObjectId = claimClass.Attributes.First(c => c.Name== "LinkToDocumentData").ID;
			long fkDocId = documentClass .Attributes.First(c => c.Name == "DocumentId").ID;

			string attributeColumn;
			string attributeWhere;

			switch (keywordName)
			{
				case "Member ID":
					atttibuteId = claimClass.Attributes.Find("MemberID").ID;
					attributeColumn = $"Claims.attr{atttibuteId} [{keywordName}]";
					attributeWhere = $"AND Claims.attr{atttibuteId} IN({values})";
					break;
				case "Subscriber ID":
					atttibuteId = claimClass.Attributes.Find("SubscriberId").ID;
					attributeColumn = $"Claims.attr{atttibuteId} [{keywordName}]";
					attributeWhere = $"AND Claims.attr{atttibuteId} IN({values})";
					break;
				case "Provider ID":
					atttibuteId = documentClass.Attributes.Find("ProviderId").ID;
					attributeColumn = $"ClaimDocuments.attr{atttibuteId} [{keywordName}]";
					attributeWhere = $"AND ClaimDocuments.attr{atttibuteId} IN({values})";
					break;
				case "Claim Number":
					atttibuteId = claimClass.Attributes.Find("ClaimNumber").ID;
					attributeColumn = $"Claims.attr{atttibuteId} [{keywordName}]";
					attributeWhere = $"AND Claims.attr{atttibuteId} IN({values})";
					break;
				default:
					return string.Empty;
			}

			return
				$"SELECT{_newLine}" +
				$"	ClaimDocuments.attr{fkDocId} [ItemNum],{_newLine}" +
				$"	{attributeColumn}{_newLine}" +
				$"FROM{_newLine}" +
				$"	ItemData [Document] with(NoLock){_newLine}" +
				$"	INNER JOIN hsi.rmObjectInstance{documentClass.ID} AS [ClaimDocuments] with(NoLock){_newLine}" +
				$"		ON [Document].ItemNum = ClaimDocuments.attr{fkDocId}{_newLine}" +
				$"	LEFT JOIN  hsi.rmObjectInstance{claimClass.ID} [Claims] with(NoLock){_newLine}" +
				$"		ON [Claims].fk{fkObjectId} = [ClaimDocuments].ObjectId{_newLine}" +
				$"WHERE{_newLine}" +
				$"	Claims.ActiveStatus = 0{_newLine}" +
				$"	AND Document.Status != '16'{_newLine}" +
				$"	{dateWhere}{_newLine}" +
				$"	{attributeWhere}{_newLine}" +
				$"UNION{_newLine}";
		}

		private string GetSingleTableSelect(
			KeywordType kt,
			string cpsSelect,
			string values,
			string dateWhere)
		{
			string dataType = _onBaseAdapter.GetKeywordValueDbColumnName(kt);
			return $"[{kt.Name}] AS (" +
				$"{cpsSelect}" +
				$"SELECT{_newLine}" +
				$"	Document.ItemNum,{_newLine}" +
				$"	[{kt.Name}].{dataType} [{kt.Name}]" +
				$"FROM{_newLine}" +
				$"	ItemData [Document] with(NoLock){_newLine}" +
				$"	INNER JOIN KeyItem{kt.ID} [{kt.Name}] with(NoLock){_newLine}" +
				$"		ON Document.ItemNum = [{kt.Name}].ItemNum{_newLine}" +
				$"WHERE{_newLine}" +
				$"	[Document].Status != '16'{_newLine}" +
				$"	{dateWhere}{_newLine}" +
				$"	AND [{kt.Name}].{dataType} IN({values}){_newLine})";
		}

		private string GetDualTableSelect(KeywordType kt,
			string cpsSelect,
			string values,
			string dateWhere)
		{
			string dataType = _onBaseAdapter.GetKeywordValueDbColumnName(kt);
			return $"[{kt.Name}] AS(" +
				$"{cpsSelect}" +
				$"SELECT{_newLine}" +
				$"	Document.ItemNum,{_newLine}" +
				$"	[{kt.Name}].{dataType} [{kt.Name}]" +
				$"FROM{_newLine}" +
				$"	ItemData [Document] with(NoLock){_newLine}" +
				$"	INNER JOIN ({_newLine}" +
				$"		SELECT{_newLine}" +
				$"			KeyXItem{kt.ID}.ItemNum ItemNum,{_newLine}" +
				$"			KeyTable{kt.ID}.{dataType} {dataType}{_newLine}" +
				$"		FROM{_newLine}" +
				$"			KeyXItem{kt.ID}{_newLine}" +
				$"			INNER JOIN KeyTable{kt.ID} with(NoLock){_newLine}" +
				$"				ON KeyXItem{kt.ID}.KeywordNum = KeyTable{kt.ID}.KeywordNum) [{kt.Name}]{_newLine}" +
				$"		ON Document.ItemNum = [{kt.Name}].ItemNum{_newLine}" +
				$"WHERE{_newLine}" +
				$"	[Document].Status != '16'{_newLine}" +
				$"	{dateWhere}{_newLine}" +
				$"	AND [{kt.Name}].{dataType} IN({values}){_newLine})";
		}

		private IEnumerable<string> LetterTypeValues()
		{
			Class wvClass = _onBaseAdapter.GetWorkViewClasses(
					ConfigurationManager.AppSettings["WorkView.DocumentAdminConsole.Name"])
				.FirstOrDefault(c =>
					c.Name == ConfigurationManager.AppSettings["WorkView.DocumentAdminConsole.Class"]);

			string attributes = string.Join(
				$",{_newLine}",
				wvClass.Attributes.Select(
					a => $"	attr{a.ID} [{a.Name}]"));

			string sql =
				$"SELECT DISTINCT{_newLine}" +
				$"	{attributes}{_newLine}" +
				$"FROM{_newLine}" +
				$"	hsi.rmObjectInstance{wvClass.ID} LetterTypes with(NoLock){_newLine}";

			string where = $"WHERE{_newLine}" +
				$"	[ActiveStatus] = 0{_newLine}";

			long memberFacingWvId = wvClass.Attributes.First(a => a.Name == "MemberPortal").ID;
			long providerFacingWvId = wvClass.Attributes.First(a => a.Name == "ProviderPortal").ID;

			if (_memberFacing || _providerFacing)
			{
				@where +=
					$"	AND {_newLine}({_newLine} " +
					$"		{(_memberFacing ? "attr" + memberFacingWvId + " = 1" : string.Empty)}{_newLine}" +
					$"		{(_memberFacing && _providerFacing ? "OR " : string.Empty)}" +
					$"		{(_providerFacing ? "attr" + providerFacingWvId + " = 1" : string.Empty)}{_newLine}" +
					$"	){_newLine}";
			}

			sql += @where;

			using (IDbConnection db = new SqlConnection(
						ConfigurationManager.ConnectionStrings["OnBase.ConnectionString"]
							.ConnectionString))
			{
				IEnumerable<dynamic> letterTypes = db.Query(
					sql,
					commandTimeout: _timeout);

				return letterTypes.Select(
						letterType => letterType.LetterType.ToString()
							.Trim())
					.Cast<string>()
					.ToList();
			}
		}

		private void SetupInstanceVariables(ISearchRequest request)
		{
			if (_letterTypeKeyword != null)
			{
				_letterTypeKeyword = _onBaseAdapter.GetKeywordByName(LetterTypeKeywordName);
			}

			_memberFacing = request.Filters.Any(k => k.Name == MemberFacingKeywordName);
			_providerFacing = request.Filters.Any(k => k.Name == ProviderFacingKeywordName);

			if (!_letterTypes.SafeAny())
			{
				_letterTypes = LetterTypeValues()
					.AsList();
			}

			if (!_documentTypes.SafeAny())
			{
				List<string> requestDocTypes = request.DocumentTypes
					?.Where(
						dc => !_letterTypes.Any(
							lt => lt.Equals(
								dc,
								StringComparison.InvariantCultureIgnoreCase)))
					.AsList();

				_documentTypes = _onBaseAdapter.GetDocumentTypes(requestDocTypes);
			}
		}
	}
}
