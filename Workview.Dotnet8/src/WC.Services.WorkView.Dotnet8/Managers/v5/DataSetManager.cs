// ------------------------------------------------------------------------------------------------
//  <copyright>
//    Copyright (c) CareSource, 2020-2022.  All rights reserved.
// 
//    Workview
//    DataSetManager.cs
//  </copyright>
//  ------------------------------------------------------------------------------------------------

namespace WC.Services.WorkView.Dotnet8.Managers.v5
{
	using System.Collections.Generic;
	using WC.Services.WorkView.Dotnet8.Adapters.v5;
	using Microsoft.Extensions.Logging;
	//using CareSource.WC.OnBase.Core.ExtensionMethods;

	public class DataSetManager : IDataSetManager
	{
		private readonly IDataSetAdapter _adapter;
		private readonly log4net.ILog _logger;

		public DataSetManager(log4net.ILog logger,
			IDataSetAdapter adapter)
		{
			_logger = logger;
			_adapter = adapter;
		}

		public IEnumerable<string> GetDataSetValues(
			string workViewApplicationName,
			string className,
			string dataSetName)
		{
			_logger.Info(
				$"Starting {nameof(GetDataSetValues)}" +
				$" for {workViewApplicationName}.{className}.{dataSetName}");

			IEnumerable<string> values = _adapter.GetDataSet(
				workViewApplicationName,
				className,
				dataSetName);

			return values;
		}
	}
}