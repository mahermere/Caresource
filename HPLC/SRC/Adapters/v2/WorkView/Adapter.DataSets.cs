// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   WC.Services.Hplc
//   Adapter.DataSets.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.Hplc.Adapters.v2.WorkView
{
	using System;
	using System.Collections.Generic;
	using System.Configuration;
	using System.Data;
	using System.Data.Common;
	using System.Data.SqlClient;
	using System.Linq;
	using CareSource.WC.OnBase.Core.ExtensionMethods;
	using Dapper;
	using Hyland.Unity.WorkView;

	public partial class Adapter
	{
		/// <summary>
		///    Gets the data set.
		/// </summary>
		/// <param name="dataSetName">The dataset name.</param>
		/// <param name="className">The class name.</param>
		/// <returns></returns>
		public IEnumerable<string> GetDataSet(
			string dataSetName,
			string className)
		{
			_logger.LogDebug($"Application Name {_applicationName}");

			Class wvClass  = _workViewApplication.Classes.FirstOrDefault(
				c => c.Name.Equals(
					className,
					StringComparison.CurrentCultureIgnoreCase));

			IEnumerable<string> items = ExecuteSqlDatasetSearch(dataSetName,
				wvClass);

			return items;
		}

		public IEnumerable<string> GetDataSet(
		long classId,
		long attributeId)
		{
			string newline = Environment.NewLine;

			string sql =
				$"SELECT{newline}" +
				$"	[Application].rmApplicationId [Application Id],{newline}" +
				$"	[Application].rmApplicationName [Application Name],{newline}" +
				$"	[Class].ClassId [Class Id],{newline}" +
				$"	[Class].ClassName [Class Name],{newline}" +
				$"	[Attribute].AttributeId [Attribute Id],{newline}" +
				$"	[Attribute].AttributeName [Attribute Name],{newline}" +
				$"	[DataSet].DataSetName [DatSet Name],{newline}" +
				$"	[DataSet Values].DataValue [Available Values]{newline}" +
				$"FROM{newline}" +
				$"	rmApplication [Application] WITH(NOLOCK) {newline}" +
				$"	INNER JOIN rmApplicationClasses [Classes] WITH(NOLOCK) {newline}" +
				$"		ON Application.rmApplicationId = Classes.rmApplicationId{newline}" +
				$"	INNER JOIN rmClass [Class] WITH(NOLOCK) {newline}" +
				$"		ON Classes.classId = Class.classId{newline}" +
				$"	INNER JOIN rmClassAttributes [Class Attributes] WITH(NOLOCK) {newline}" +
				$"		ON Class.classId  = [Class Attributes].classId{newline}" +
				$"	INNER JOIN rmAttribute [Attribute] WITH(NOLOCK) {newline}" +
				$"		ON [Class Attributes].attributeId = Attribute.attributeId{newline}" +
				$"	LEFT JOIN [rmDataSet] [DataSet] WITH(NOLOCK) {newline}" +
				$"		ON [Attribute].DataSetId = DataSet.DataSetId{newline}" +
				$"	LEFT JOIN rmDataSetValue [DataSet Values] WITH(NOLOCK) {newline}" +
				$"		ON DataSet.DataSetId = [DataSet Values].DataSetId{newline}" +
				$"WHERE{newline}" +
				$"	[Application].rmApplicationId = {_workViewApplication.ID}{newline}" +
				$"	AND Class.classId = {classId}{newline}" +
				$"	AND [Attribute].attributeId = '{attributeId}'{newline}" +
				$"	AND [DataSet Values].DataValue IS NOT NULL{newline}" +
				$"ORDER BY{newline}" +
				"	[Attribute Name], [Available Values]";

			IEnumerable<string> items = ExecuteSql(sql);

			return items;
		}

		private IEnumerable<string> ExecuteSqlDatasetSearch(
			string dataSetName,
			Class wvClass)
		{
			string newline = Environment.NewLine;

			string sql =
				  $"SELECT{newline}"
				+ $"	[Application].rmApplicationId [Application Id],{newline}"
				+ $"	[Application].rmApplicationName [Application Name],{newline}"
				+ $"	[Class].ClassId [Class Id],{newline}"
				+ $"	[Class].ClassName [Class Name],{newline}"
				+ $"	[Attribute].AttributeId [Attribute Id],{newline}"
				+ $"	[Attribute].AttributeName [Attribute Name],{newline}"
				+ $"	[DataSet].DataSetName [DatSet Name],{newline}"
				+ $"	[DataSet Values].DataValue [Available Values]{newline}"
				+ $"FROM{newline}"
				+ $"	rmApplication [Application] WITH(NOLOCK) {newline}"
				+ $"	INNER JOIN rmApplicationClasses [Classes] WITH(NOLOCK) {newline}"
				+ $"		ON Application.rmApplicationId = Classes.rmApplicationId{newline}"
				+ $"	INNER JOIN rmClass [Class] WITH(NOLOCK) {newline}"
				+ $"		ON Classes.classId = Class.classId{newline}"
				+ $"	INNER JOIN rmClassAttributes [Class Attributes] WITH(NOLOCK) {newline}"
				+ $"		ON Class.classId  = [Class Attributes].classId{newline}"
				+ $"	INNER JOIN rmAttribute [Attribute] WITH(NOLOCK) {newline}"
				+ $"		ON [Class Attributes].attributeId = Attribute.attributeId{newline}"
				+ $"	LEFT JOIN [rmDataSet] [DataSet] WITH(NOLOCK) {newline}"
				+ $"		ON [Attribute].DataSetId = DataSet.DataSetId{newline}"
				+ $"	LEFT JOIN rmDataSetValue [DataSet Values] WITH(NOLOCK) {newline}"
				+ $"		ON DataSet.DataSetId = [DataSet Values].DataSetId{newline}"
				+ $"WHERE{newline}"
				+ $"	[Application].rmApplicationId = {_workViewApplication.ID}{newline}"
				+ $"	AND Class.classId = {wvClass.ID}{newline}"
				+ $"	AND [DataSet].DataSetName = '{dataSetName}'{newline}"
				+ $"ORDER BY{newline}"
				+ $"	[Attribute Name], [Available Values]";

			List<string> items = new List<string>();

			using (IDbConnection db = new SqlConnection(
				ConfigurationManager.ConnectionStrings["OnBase.ConnectionString"]
					.ConnectionString))
			{
				using (IDataReader dataset = db.ExecuteReader(
					sql,
					commandTimeout: _timeout))
				{
					while (dataset.Read())
					{
						items.Add(
							dataset.GetString(dataset.GetOrdinal("Available Values"))
								.SafeTrim());
					}
				}
			}

			return items;
		}

		/// <summary>
		///    Gets the class as data set.
		/// </summary>
		/// <param name="dataSetName">The data set name.</param>
		/// <param name="className">The class name.</param>
		/// <returns></returns>
		public IDictionary<long, string> GetClassAsDataSet(
			string dataSetName,
			string className)
		{
			Class wvClass = _workViewApplication.Classes.FirstOrDefault(
				c => c.Name.Equals(
					className,
					StringComparison.CurrentCultureIgnoreCase));

			Filter filter = LoadWvClassFilter(
				dataSetName,
				wvClass);

			FilterQuery filterQuery = filter.CreateFilterQuery();

			FilterQueryResultItemList items = filterQuery.Execute(10000);

			return items.ToDictionary(
				e => e.ObjectID,
				e => e.GetFilterColumnValue("ProductName").AlphanumericValue);
		}

		private Filter LoadWvClassFilter(
			string filterName,
			Class wvClass)
		{
			Filter filter = _workViewApplication.Filters
				.FirstOrDefault(
				f => f.Name == filterName && f.Class.Name == wvClass.Name);

			if (filter == null)
			{
				throw new ArgumentException(
					$"Could not find WorkView Class '{wvClass.Name}' Filter '{filterName}'.");
			}

			return filter;
		}

		private static IEnumerable<string> ExecuteSql(string sql)
		{
			// This should be ok
			List<string> items = new List<string>();

			int timeout =
				Convert.ToInt32(ConfigurationManager.AppSettings.Get("OnBase.Connection.Timeout"));

			using (IDbConnection db = new SqlConnection(
				ConfigurationManager.ConnectionStrings["OnBase.ConnectionString"].ConnectionString))
			{
				using (DbDataReader dataset =
					db.ExecuteReader(sql, commandTimeout: timeout) as DbDataReader)
				{
					if (dataset.HasRows)
					{
						while (dataset.Read())
						{
							items.Add(dataset.GetString(dataset.GetOrdinal("Available Values"))
								.SafeTrim());
						}
					}
				}
			}

			return items;
		}
	}
}