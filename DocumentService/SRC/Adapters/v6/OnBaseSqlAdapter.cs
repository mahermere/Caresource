// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   WC.Services.Document
//   OnBaseSqlAdapter.cs
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
	using CareSource.WC.Entities.Requests.Base;
	using CareSource.WC.Services.Document.Models.v6;
	using Dapper;
	using Hyland.Unity;
	using Hyland.Unity.WorkView;
	using Microsoft.Extensions.Logging;
	using ConfigurationManager = System.Configuration.ConfigurationManager;
	using DocumentHeader = CareSource.WC.Services.Document.Models.v6.DocumentHeader;
	using Filter = CareSource.WC.Entities.Common.Filter;

	public partial class OnBaseSqlAdapter : IOnBaseSqlAdapter<DocumentHeader>
	{
		private const string LetterTypeKeywordName = "Letter Type";
		private const string MemberFacingKeywordName = "Member Facing";
		private const string ProviderFacingKeywordName = "Provider Facing";
		private const int MultiTableKeywordTypeId = 2;

		private readonly Dictionary<int, string> _columnDataTypeMaps = new Dictionary<int, string>
		{
			{ 1, "KeyValueBig" },
			{ MultiTableKeywordTypeId, "KeyValueChar" },
			{ 3, "KeyValueCurr"},
			{ 4, "KeyValueDate" },
			{ 5, "KeyValueFloat" },
			{ 6, "KeyValueSmall" },
			{ 9, "KeyValueTod" },
			{ 10, "KeyValueChar" },
			{ 11, "KeyValueCurr"},
			{ 12, "KeyValueChar" },
			{ 13, "KeyValueChar" }
		};

		private readonly ILogger _logger;
		private readonly string _newLine = Environment.NewLine;
		private readonly IOnBaseAdapter _onBaseAdapter;
		private List<string> _letterTypes;
		private IEnumerable<DocumentType> _documentTypes;

		private KeywordType _letterTypeKeyword;
		private long _memberFacingWvId;
		private long _providerFacingWvId;
		private long _letterNameWvId;
		private long _dacClassWvId;

		/// <summary>
		///    Initializes a new instance of the <see cref="OnBaseSqlAdapter" /> class.
		/// </summary>
		/// <param name="onBaseAdapter">The on base adapter.</param>
		/// <param name="logger">The logger.</param>
		public OnBaseSqlAdapter(
			IOnBaseAdapter onBaseAdapter,
			ILogger logger)
		{
			_logger = logger;
			_onBaseAdapter = onBaseAdapter;
		}

		private int Timeout => Convert.ToInt32(
			ConfigurationManager.AppSettings.Get("OnBase.Connection.Timeout"));


		/// <summary>
		///    Combines the Filter, order by and display columns into one list for use creating the
		///    from clauses.
		/// </summary>
		/// <param name="displayColumns">The display columns.</param>
		/// <param name="filters">The filters.</param>
		/// <param name="sort">The sort.</param>
		/// <returns></returns>
		private IEnumerable<KeywordType> CombineColumns(
			IEnumerable<KeywordType> displayColumns,
			IEnumerable<KeywordType> filters,
			KeywordType sort)
		{
			List<KeywordType> kws = new List<KeywordType>();
			if (displayColumns != null &&
				displayColumns.Any())
			{
				kws.AddRange(displayColumns);
			}

			kws.AddRange(filters);

			if (sort != null)
			{
				kws.Add(sort);
			}

			return kws.Distinct().Where(f => f.Name != "Letter Type");
		}


		private (string from, string where) CreateFromAndWhere(
			IFilteredRequest request)
		{
			List<Filter> requestLetterTypes = request.DocumentTypes
				?.Where(dc => _letterTypes.Contains(dc))
				.Select(
					k => new Filter(
						_letterTypeKeyword.Name,
						k))
				.AsList();

			List<Filter> filters = request.Filters.AsList();

			if (requestLetterTypes != null)
			{
				filters.AddRange(requestLetterTypes);
			}

			IEnumerable<KeywordType> keywords =
				_onBaseAdapter.GetKeywordTypes(filters.Select(f => f.Name));

			// Combine Items for From Clauses, need all display columns, any sort and all filter
			IEnumerable<KeywordType> fromColumns = CombineColumns(
				null,
				keywords,
				null);

			string from = CreateFromClausesKeyword(fromColumns);

			string where = CreateWhereClausesKeyword(
				filters,
				keywords,
				_documentTypes,
				request.StartDate,
				request.EndDate);

			return (from, where);
		}

		private (string from, string where) CreateFromAndWhereKeyword(
			ISearchRequest request)
		{
			List<Filter> requestLetterTypes = request.DocumentTypes
				?.Where(
					dc => _letterTypes.Any(
						lt => lt.Equals(
							dc,
							StringComparison.InvariantCultureIgnoreCase)))
				.Select(
					k => new Filter(
						_letterTypeKeyword.Name,
						k))
				.AsList();

			List<Filter> filters = request.Filters.AsList();

			if (requestLetterTypes != null)
			{
				filters.AddRange(requestLetterTypes);
			}

			IEnumerable<KeywordType> keywords =
				_onBaseAdapter.GetKeywordTypes(filters.Select(f => f.Name));

			// Combine Items for From Clauses, need all display columns, any sort and all filter
			IEnumerable<KeywordType> fromColumns = CombineColumns(
				_onBaseAdapter.GetKeywordTypes(request.DisplayColumns),
				keywords,
				GetSortKeywordType(request.Sorting));

			string from = CreateFromClausesKeyword(fromColumns);

			string where = CreateWhereClausesKeyword(
				filters,
				keywords,
				_documentTypes,
				request.StartDate,
				request.EndDate);

			return (from, where);
		}

		private (string from, string where) CreateFromAndWhereWorkView(
			ISearchRequest request)
		{
			List<Filter> requestLetterTypes = request.DocumentTypes
				?.Where(dc => _letterTypes.Contains(dc))
				.Select(
					k => new Filter(
						_letterTypeKeyword.Name,
						k))
				.AsList();

			List<Filter> filters = request.Filters.AsList();

			if (requestLetterTypes != null)
			{
				filters.AddRange(requestLetterTypes);
			}

			IEnumerable<KeywordType> keywords =
				_onBaseAdapter.GetKeywordTypes(filters.Select(f => f.Name));

			// Combine Items for From Clauses, need all display columns, any sort and all filter
			IEnumerable<KeywordType> fromColumns = CombineColumns(
				_onBaseAdapter.GetKeywordTypes(request.DisplayColumns),
				keywords,
				GetSortKeywordType(request.Sorting));

			string from = CreateFromClausesWorkView(fromColumns);

			string where = CreateWhereClausesWorkView(
				request.Filters,
				keywords,
				_documentTypes,
				request.StartDate,
				request.EndDate);

			return (from, where);
		}

		private string CreateFromClausesKeyword(
			IEnumerable<KeywordType> fromColumns)
		{
			IEnumerable<KeywordType> facingKeywords =
				fromColumns.Where(
					k => k.Name == MemberFacingKeywordName || k.Name == ProviderFacingKeywordName);

			return $"FROM{_newLine}" +
				$"	ItemData Document with(NoLock){_newLine}" +
				$"	INNER JOIN DocType DocumentType with(NoLock){_newLine}" +
				$"		ON Document.ItemTypeNum = DocumentType.ItemTypeNum{_newLine}" +
				FacingKeywordFrom(facingKeywords) +
				GetKeywordJoins() +
				_newLine;

			string GetKeywordJoins()
			{
				return string.Join(
					_newLine,
					fromColumns
						.Where(
							kt => kt.Name != MemberFacingKeywordName &&
								kt.Name != ProviderFacingKeywordName)
						.Select(
							dc =>
								// magic number 2, indicates that the Keywords is multi table.
								GetKeywordDataType(dc) == MultiTableKeywordTypeId
									? $"	LEFT JOIN ({_newLine}" +
									$"		SELECT{_newLine}" +
									$"			KeyXItem{dc.ID}.ItemNum ItemNum,{_newLine}" +
									$"			KeyTable{dc.ID}.KeyValueChar KeyValueChar{_newLine}" +
									$"		FROM{_newLine}" +
									$"			KeyXItem{dc.ID} INNER JOIN KeyTable{dc.ID} with(NoLock){_newLine}" +
									$"				ON KeyXItem{dc.ID}.KeywordNum = KeyTable{dc.ID}.KeywordNum)" +
									$" [{dc.Name}]{_newLine}" +
									$"		ON Document.ItemNum = [{dc.Name}].ItemNum"
									: $"	LEFT JOIN KeyItem{dc.ID} [{dc.Name}] with(NoLock){_newLine}" +
									$"		ON Document.ItemNum = [{dc.Name}].ItemNum"));
			}
		}

		private string FacingKeywordFrom(IEnumerable<KeywordType> facingKeywords)
			=> (facingKeywords.Any()
					? $"	INNER JOIN ItemTypeXKeyword DocTypeKeywords with(NoLock){_newLine}" +
					$"		ON DocTypeKeywords.ItemTypeNum = DocumentType.ItemTypeNum{_newLine}" +
					"			AND DocTypeKeywords.KeyTypeNum in (" +
					$"{string.Join(", ", facingKeywords.Select(k => k.ID))}){_newLine}"
					: string.Empty) +
				$"	LEFT JOIN{_newLine}" +
				$"		({_newLine}" +
				$"		SELECT DISTINCT{_newLine}" +
				$"			[Letter Type].ItemNum [ItemNum],{_newLine}" +
				$"			LTRIM(RTRIM([Letter Types].attr{_letterNameWvId})) [KeyValueChar]{_newLine}" +
				$"		FROM{_newLine}" +
				$"			hsi.rmObjectInstance{_dacClassWvId} [Letter Types] with(NoLock){_newLine}" +
				$"			INNER JOIN KeyItem{_letterTypeKeyword.ID} [Letter Type] with(NoLock){_newLine}" +
				$"				ON KeyValueChar = [Letter Types].attr{_letterNameWvId}{_newLine}" +
				$"		WHERE{_newLine}" +
				$"			[ActiveStatus] = 0 {_newLine}" +
				(facingKeywords.Any()
					? $"			AND{_newLine}" +
						(facingKeywords.Any(kw => kw.Name == "Member Facing")
						? $"			[Letter Types].attr{_memberFacingWvId} = 1{_newLine}"
						: string.Empty) +
					(facingKeywords.Count() > 1
						? $"			OR{_newLine}"
						: string.Empty) +
					(facingKeywords.Any(kw => kw.Name == "Provider Facing")
						? $"			[Letter Types].attr{_providerFacingWvId} = 1{_newLine}"
						: string.Empty)
					: string.Empty) +
				$"	) As [Letter Type]{_newLine}" +
				$"		ON [Letter Type].ItemNum = Document.ItemNum{_newLine}";

		private string CreateWhereClausesKeyword(
			IEnumerable<Filter> filters,
			IEnumerable<KeywordType> keywordFilters,
			IEnumerable<DocumentType> documentTypes,
			DateTime? startDate,
			DateTime? endDate)
		{
			return $"WHERE{_newLine}" +
				$"	Document.Status != '16'{_newLine}" +
				CreateDocumentTypeWhere() +
				CreateDocumentDateWhere() +
				CreateFiltersWhere() +
				_newLine;

			string CreateFiltersWhere()
			{
				List<string> distinctFilters = filters
					.Where(
						f =>
							f.Name != MemberFacingKeywordName &&
							f.Name != ProviderFacingKeywordName)
					.Select(f => f.Name)
					.Distinct()
					.AsList();

				string where = string.Empty;
				foreach (string filter in distinctFilters)
				{
					string inClause = string.Join(
						", ",
						filters
							.Where(f => f.Name == filter)
							.Select(f => "'" + f.Value + "'"));

					bool includeNull = filters.Any(f => f.Name == filter && f.IncludeNull);

					KeywordType kw = keywordFilters.First(k => k.Name == filter);
					int dataType = GetKeywordDataType(kw);

					where += $"		AND ([{filter}].{_columnDataTypeMaps[dataType]} IN ({inClause})" +
						(includeNull
							? $"{_newLine}		OR [{filter}].{_columnDataTypeMaps[dataType]} is NULL)" +
							$"{_newLine}"
							: $"){_newLine}");
				}

				where +=
					"		AND COALESCE([Letter Type].KeyValueChar,DocumentType.ItemTypeName) <> 'Guiding Care Letters'";
				return where;
			}

			string CreateDocumentDateWhere()
			{
				return startDate.HasValue && endDate.HasValue
					? "	AND Document.ItemDate BETWEEN " +
					$"'{startDate.Value.Date:s}'" +
					$" AND '{endDate.Value.Date.AddDays(1).AddSeconds(-1):s}'{_newLine}"
					: string.Empty;
			}

			string CreateDocumentTypeWhere()
			{
				return documentTypes.Any()
					? "	AND Document.ItemTypeNum in(" +
					$"{string.Join(", ", documentTypes.Select(dt => dt.ID))}){_newLine}"
					: string.Empty;
			}
		}


		private string CreateFromClausesWorkView(
			IEnumerable<KeywordType> fromColumns)
		{
			IEnumerable<KeywordType> facingKeywords =
				fromColumns.Where(k => k.Name == MemberFacingKeywordName || k.Name == ProviderFacingKeywordName);

			return $"FROM{_newLine}" +
				$"		ItemData Document with(NoLock){_newLine}" +
				$"		INNER JOIN DocType DocumentType with(NoLock){_newLine}" +
				$"			ON Document.ItemTypeNum = DocumentType.ItemTypeNum{_newLine}" +
				FacingKeywordFrom(facingKeywords) +
				GetWorkViewClaimsFrom() +
				GetKeywordJoins() +
				_newLine;

			string GetWorkViewClaimsFrom()
			{
				string workViewClaimsFrom = String.Empty;
				IEnumerable<Class> classes =
					_onBaseAdapter.GetWorkViewClasses(
						ConfigurationManager.AppSettings["WorkView.Application"]);

				Class documentClass = classes.FirstOrDefault(c => c.Name == "Documents");
				if (documentClass == null)
				{
					throw new ArgumentException("No Class Found with Name [Documents]");
				}

				Class claimClass = classes.FirstOrDefault(c => c.Name == "Claims");
				if (claimClass == null)
				{
					throw new ArgumentException("No Class Found with Name [Claims]");
				}

				long linkToDocumentDataId
					= claimClass.Attributes.First(a => a.Name == "LinkToDocumentData")
						.ID;

				string claimsAttributes = string.Join(
					$",{_newLine}",
					claimClass.Attributes.Where(a => !a.Name.Contains("LinkTo"))
						.Select(
							a => $"				Claims.attr{a.ID} [{a.Name}]"));

				string claimDocumentAttributes = string.Join(
					$",{_newLine}",
					documentClass.Attributes.Select(
						a => $"				ClaimDocuments.attr{a.ID} [{a.Name}]"));

				workViewClaimsFrom =
					$"		LEFT JOIN ({_newLine}" +
					$"			SELECT{_newLine}" +
					$"{claimsAttributes},{_newLine}" +
					$"{claimDocumentAttributes}{_newLine}" +
					$"			FROM{_newLine}" +
					$"				hsi.rmObjectInstance{claimClass.ID} Claims{_newLine}" +
					$"				LEFT JOIN hsi.rmObjectInstance{documentClass.ID} AS ClaimDocuments" +
					$"{_newLine}" +
					$"					ON Claims.fk{linkToDocumentDataId} = ClaimDocuments.ObjectId" +
					$"{_newLine}" +
					$"			WHERE(Claims.ActiveStatus = 0){_newLine}" +
					$"		) RnlClaims ON Document.ItemNum = RnlClaims.DocumentId{_newLine}";


				return workViewClaimsFrom;
			}

			string GetKeywordJoins()
			{
				return string.Join(
					_newLine,
					fromColumns
						.Where(
							kt => kt.Name != MemberFacingKeywordName &&
								kt.Name != ProviderFacingKeywordName)
						.Select(
							dc =>
								GetKeywordDataType(dc) == MultiTableKeywordTypeId
									? $"		LEFT JOIN ({_newLine}" +
									$"			SELECT{_newLine}" +
									$"				KeyXItem{dc.ID}.ItemNum ItemNum,{_newLine}" +
									$"				KeyTable{dc.ID}.KeyValueChar KeyValueChar{_newLine}" +
									$"			FROM{_newLine}" +
									$"				KeyXItem{dc.ID} INNER JOIN KeyTable{dc.ID} with(NoLock){_newLine}" +
									$"					ON KeyXItem{dc.ID}.KeywordNum = KeyTable{dc.ID}.KeywordNum)" +
									$" [{dc.Name}]{_newLine}" +
									$"			ON Document.ItemNum = [{dc.Name}].ItemNum"
									: $"		LEFT JOIN KeyItem{dc.ID} [{dc.Name}] with(NoLock){_newLine}" +
									$"			ON Document.ItemNum = [{dc.Name}].ItemNum"));
			}
		}

		private string CreateWhereClausesWorkView(
			IEnumerable<Filter> filters,
			IEnumerable<KeywordType> keywordFilters,
			IEnumerable<DocumentType> documentTypes,
			DateTime? startDate,
			DateTime? endDate)
		{
			return
				$"	WHERE{_newLine}" +
				$"		Document.Status != '16'{_newLine}" +
				CreateDocumentTypeWhere() +
				CreateDocumentDateWhere() +
				CreateFiltersWhere() +
				_newLine;

			string CreateFiltersWhere()
			{
				List<string> distinctFilters = filters
					.Where(
						f =>
							f.Name != MemberFacingKeywordName &&
							f.Name != ProviderFacingKeywordName)
					.Select(f => f.Name)
					.Distinct()
					.AsList();

				string where = string.Empty;
				foreach (string filter in distinctFilters)
				{
					string inClause = string.Join(
						", ",
						filters
							.Where(f => f.Name == filter)
							.Select(f => "'" + f.Value + "'"));

					bool includeNull = filters.Any(f => f.Name == filter && f.IncludeNull);

					KeywordType kw = keywordFilters.First(k => k.Name == filter);
					int dataType = GetKeywordDataType(kw);

					string w;
					switch (filter)
					{
						case "Claim Number":
							w = $"		AND ([ClaimNumber] IN ({inClause})){_newLine}";
							break;
						case "Subscriber ID":
							w = $"		AND ([SubscriberId] IN ({inClause})){_newLine}";
							break;
						case "Member Suffix":
							w = $"		AND ([MemberSuffix] IN ({inClause})){_newLine}";
							break;
						case "Member ID":
							w = $"		AND ([MemberID] IN ({inClause})){_newLine}";
							break;
						case "Provider ID":
							w = $"		AND ([ProviderID] IN ({inClause})){_newLine}";
							break;
						default:
							w =
								$"		AND ([{filter}].{_columnDataTypeMaps[dataType]} IN ({inClause})" +
								(includeNull
									? $"{_newLine}			OR [{filter}].{_columnDataTypeMaps[dataType]} is NULL)" +
									$"{_newLine}"
									: $"){_newLine}");
							break;
					}

					where += w;
				}
				where +=
					"		AND COALESCE([Letter Type].KeyValueChar,DocumentType.ItemTypeName) <> 'Guiding Care Letters'";
				return where;
			}

			string CreateDocumentDateWhere()
			{
				return startDate.HasValue && endDate.HasValue
					? "		AND Document.ItemDate BETWEEN " +
					$"'{startDate.Value.Date:s}'" +
					$" AND '{endDate.Value.Date.AddDays(1).AddSeconds(-1):s}'{_newLine}"
					: string.Empty;
			}

			string CreateDocumentTypeWhere()
			{
				return documentTypes.Any()
					? "		AND Document.ItemTypeNum in(" +
					$"{string.Join(", ", documentTypes.Select(dt => dt.ID))}){_newLine}"
					: string.Empty;
			}
		}

		private static int GetKeywordDataType(KeywordType dc)
			=> Convert.ToInt32(
				typeof(KeywordType)
					.GetField(
						"_dataType",
						BindingFlags.NonPublic | BindingFlags.Instance)
					?.GetValue(dc));

		private string CreateOrderByClauseWorkView(Sort sort)
		{
			KeywordType sortKeywordType = GetSortKeywordType(sort);

			const string constSort = "	DocumentDate DESC";

			if (sortKeywordType == null)
			{
				return
					$"ORDER BY {_newLine}" +
					$"	{constSort}";
			}
			
			string ascDesc = string.Empty;

			if (!sort.SortAscending)
			{
				ascDesc = " DESC";
			}

			switch (sort.ColumnName)
			{
				case "DocumentName":
					return
						$"ORDER BY{_newLine}" +
						$"	DocumentName{ascDesc}, {constSort}";
				case "DocumentType":
					return
						$"ORDER BY{_newLine}" +
						$"	DocumentType {ascDesc}, {constSort}";
				case "DocumentDate":
					return
						$"ORDER BY{_newLine}" +
						$"	{constSort}";
				default:
					return
						$"ORDER BY{_newLine}" +
						$"	[{sortKeywordType.Name}]{ascDesc}, " +
						$"{constSort}";
			}
		}

		private string CreateOrderByClause(Sort sort)
		{
			const string constSort = "	Document.ItemDate DESC";

			if (sort == null)
			{
				return $"ORDER BY{_newLine}" +
					$"	{constSort}";
			}
			
			string ascDesc = string.Empty;

			if (!sort.SortAscending)
			{
				ascDesc = " DESC";
			}

			switch (sort.ColumnName)
			{
				case "DocumentName":
					return
						$"ORDER BY{_newLine}" +
						$"	Document.ItemName{ascDesc}, {constSort}";
				case "DocumentType":
					return
						$"ORDER BY{_newLine}" +
						$"	COALESCE([Letter Type].KeyValueChar, DocumentType.ItemTypeName){ascDesc}, " +
						$"{constSort}";
				case "DocumentDate":
					return
						$"ORDER BY{_newLine}	{constSort}";
			
				default:

					KeywordType sortKeywordType = GetSortKeywordType(sort);

					if (sortKeywordType == null)
					{
						return $"ORDER BY{_newLine}" +
							$"	{constSort}";
					}

					int dataType = GetKeywordDataType(sortKeywordType);

					return
						$"ORDER BY{_newLine}" +
						$"	[{sort.ColumnName}].{_columnDataTypeMaps[dataType]}{ascDesc}, " +
						$"{constSort}";
			}
		}

		private KeywordType GetSortKeywordType(Sort sort)
			=> sort == null ||
				sort.ColumnName == "DocumentName" ||
				sort.ColumnName == "DocumentType" ||
				sort.ColumnName == "DocumentDate"
					? null
					: _onBaseAdapter.GetKeywordByName(sort.ColumnName);

		private IEnumerable<string> LetterTypeValues(
			bool memberFacing,
			bool providerFacing)
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
				$"[ActiveStatus] = 0{_newLine}";

			_memberFacingWvId = wvClass.Attributes.First(a => a.Name == "MemberPortal").ID;
			_providerFacingWvId = wvClass.Attributes.First(a => a.Name == "ProviderPortal").ID;
			_letterNameWvId = wvClass.Attributes.First(a => a.Name == "LetterType").ID;
			_letterNameWvId = wvClass.Attributes.First(a => a.Name == "LetterType").ID;
			_dacClassWvId = wvClass.ID;

			if (memberFacing || providerFacing)
			{
				

				@where +=
					$"	AND {_newLine}({_newLine} "+
					$"		{(memberFacing ? "attr" + _memberFacingWvId + " = 1" : string.Empty)}{_newLine}" +
					$"		{(memberFacing && providerFacing ? "OR " : string.Empty)}" +
					$"		{(providerFacing ? "attr" + _providerFacingWvId + " = 1" : string.Empty)}{_newLine}" +
					$"	){_newLine}";
			}

			sql += @where;

			using (IDbConnection db = new SqlConnection(
				ConfigurationManager.ConnectionStrings["OnBase.ConnectionString"]
					.ConnectionString))
			{
				IEnumerable<dynamic> letterTypes = db.Query(
					sql,
					commandTimeout: Timeout);

				return letterTypes.Select(
						letterType => letterType.LetterType.ToString()
							.Trim())
					.Cast<string>()
					.ToList();
			}
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

		private void SetupInstanceVariables(IFilteredRequest request)
		{
			_letterTypeKeyword = _onBaseAdapter.GetKeywordByName(LetterTypeKeywordName);

			_letterTypes = LetterTypeValues(
					request.Filters.Any(k => k.Name == MemberFacingKeywordName),
					request.Filters.Any(k => k.Name == ProviderFacingKeywordName))
				.AsList();

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