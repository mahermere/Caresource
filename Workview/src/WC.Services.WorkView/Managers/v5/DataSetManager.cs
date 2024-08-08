// ------------------------------------------------------------------------------------------------
//  <copyright>
//    Copyright (c) CareSource, 2020-2022.  All rights reserved.
// 
//    Workview
//    DataSetManager.cs
//  </copyright>
//  ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.WorkView.Managers.v5
{
	using System.Collections.Generic;
	using CareSource.WC.Services.WorkView.Adapters.v5;
	using Microsoft.Extensions.Logging;
	using CareSource.WC.OnBase.Core.ExtensionMethods;

	public class DataSetManager : IDataSetManager
	{
		private readonly IDataSetAdapter _adapter;
		private readonly ILogger _logger;

		public DataSetManager(ILogger logger,
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
			_logger.LogInformation(
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