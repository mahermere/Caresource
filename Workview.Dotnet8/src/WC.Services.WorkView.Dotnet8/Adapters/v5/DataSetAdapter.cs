// ------------------------------------------------------------------------------------------------
//  <copyright>
//    Copyright (c) CareSource, 2020-2022.  All rights reserved.
// 
//    Workview
//    DataSetAdapter.cs
//  </copyright>
//  ------------------------------------------------------------------------------------------------

namespace WC.Services.WorkView.Dotnet8.Adapters.v5
{
	using System;
	using System.Collections.Generic;
	using System.Configuration;
	using System.Data;
	using System.Data.Common;
	using System.Data.SqlClient;
	//using CareSource.WC.OnBase.Core.Connection.Interfaces;
	//using CareSource.WC.OnBase.Core.ExtensionMethods;
	using WC.Services.WorkView.Dotnet8.Mappers.v5;
	using WC.Services.WorkView.Dotnet8.Models.v5;
	//using Dapper;
	using Hyland.Unity.WorkView;
	using Application = Hyland.Unity.Application;
	using Microsoft.Extensions.Logging;
    using WC.Services.WorkView.Dotnet8.Repository;
    using WC.Services.WorkView.Dotnet8.Mappers.v5.Interfaces;

    public class DataSetAdapter : BaseAdapter, IDataSetAdapter
	{
		private static IConfiguration _configuration;
        public DataSetAdapter(
			log4net.ILog logger,
            IModelMapper<WorkViewObject, WorkViewBaseObject> modelMapper,
            IConfiguration configuration,
			IRepository repo)
			: base(
				modelMapper,
				repo,
				null,
				logger)
		{ _configuration = configuration; }

		public IEnumerable<string> GetDataSet(
			string workViewApplicationName,
			string className,
			long attributeId)
		{
			Class wvClass = GetWvClass(className);

			return GetDataSet(
				workViewApplicationName,
				wvClass,
				attributeId);
		}

		public IEnumerable<string> GetDataSet(
			string workViewApplicationName,
			Class wvClass,
			long attributeId)
		{
			SetWorkViewApplication(workViewApplicationName);

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
				$"	[Application].rmApplicationId = {WvApplication.ID}{newline}" +
				$"	AND Class.classId = {wvClass.ID}{newline}" +
				$"	AND [Attribute].attributeId = '{attributeId}'{newline}" +
				$"	AND [DataSet Values].DataValue IS NOT NULL{newline}" +
				$"ORDER BY{newline}" +
				"	[Attribute Name], [Available Values]";

			Logger.Debug($"SQL Query String: {sql}");

			IEnumerable<string> items = ExecuteSql(sql);

			return items;
		}

		public IEnumerable<string> GetDataSet(
			string workViewApplicationName,
			string className,
			string dataSetName)
		{
			SetWorkViewApplication(workViewApplicationName);

			Class wvClass = GetWvClass(className);

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
				$"	[Application].rmApplicationId = {WvApplication.ID}{newline}" +
				$"	AND Class.classId = {wvClass.ID}{newline}" +
				$"	AND [DataSet].DataSetName = '{dataSetName}'{newline}" +
				$"ORDER BY{newline}" +
				"	[Attribute Name], [Available Values]";

			Logger.Debug($"SQL Query String: {sql}");

			IEnumerable<string> items = ExecuteSql(sql);

			return items;
		}

		private static IEnumerable<string> ExecuteSql(string sql)
		{
			// This should be ok
			List<string> items = new List<string>();

			int timeout =
				Convert.ToInt32(_configuration["OnBaseSettings:OnBase.Connection.Timeout"]);
			string connectionString = _configuration["OnBaseSettings:OnBase.ConnectionString"];

			using (IDbConnection db = new SqlConnection(connectionString))
			{
				db.Open(); // Open the connection explicitly

				using (IDbCommand command = db.CreateCommand())
				{
					command.CommandText = sql;
					command.CommandTimeout = timeout;

					using (DbDataReader dataset = command.ExecuteReader() as DbDataReader)
					{
						if (dataset.HasRows)
						{
							while (dataset.Read())
							{
								items.Add(dataset.GetString(dataset.GetOrdinal("Available Values"))
									.Trim());
							}
						}
					}
				}

				return items;
			}
		}
	}
}