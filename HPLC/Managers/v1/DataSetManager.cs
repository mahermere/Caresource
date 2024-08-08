// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   WC.Services.Hplc
//   DataSetManager.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.Hplc.Managers.v1
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Runtime.Caching;
	using CareSource.WC.OnBase.Core.Diagnostics.Interfaces;
	using CareSource.WC.OnBase.Core.ExtensionMethods;
	using WC.Services.Hplc.Adapters.v1.WorkView;

	/// <summary>
	/// Data and functions describing a CareSource.WC.Services.Hplc.Managers.v1.DataSetManager object.
	/// </summary>
	/// <seealso cref="IDataSetManager" />
	public class DataSetManager : IDataSetManager
	{
		private readonly ILogger _logger;
		private readonly IAdapter _adapter;
		private readonly ObjectCache _memoryCache;

		/// <summary>
		/// Initializes a new instance of the <see cref="DataSetManager"/> class.
		/// </summary>
		/// <param name="logger">The logger.</param>
		/// <param name="adapter">The adapter.</param>
		/// <param name="memoryCache">The memory cache.</param>
		public DataSetManager(
			ILogger logger,
			IAdapter adapter,
			MemoryCache memoryCache)
		{
			_logger = logger;
			_adapter = adapter;
			_memoryCache = memoryCache;
		}

		/// <summary>
		/// Gets the data set.
		/// </summary>
		/// <param name="dataSetName">The data set name.</param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException">dataSetName - A DataSet Name is Required</exception>
		/// <exception cref="KeyNotFoundException">No DataSet found matching name [{dataSetName}]</exception>
		public IEnumerable<string> GetDataSet(
			string dataSetName)
		{
			if (dataSetName.IsNullOrWhiteSpace())
			{
				throw new ArgumentNullException(
					nameof(dataSetName),
					"A DataSet Name is Required");
			}

			IEnumerable<string> dataSet = (IEnumerable<string>)_memoryCache.Get(dataSetName);

			if (dataSet == null)
			{
				_logger.LogDebug($"{dataSetName} not in Cache, retrieving form DB.");
				switch (dataSetName)
				{
					case "API - Products":
						IDictionary<long, string> data = _adapter.GetClassAsDataSet(
							dataSetName,
							"Product",
							new Dictionary<string, string>());

						_memoryCache.Add(
							new CacheItem(
								$"{dataSetName} ObjectIds",
								data),
							null);

						dataSet = data.Select(e => e.Value);
						break;

					case "ProviderType":
					case "ActionType":
						dataSet = _adapter.GetDataSet(
							dataSetName,
							"Provider");
						break;

					case "Phone Type - Provider":
						dataSet = _adapter.GetDataSet(
							dataSetName,
							"Phone");
						break;

					case "Address Type":
					case "Action Type - Provider Locations":
						dataSet = _adapter.GetDataSet(
							dataSetName,
							"Location");
						break;

					case "Provider Maintenance Request Types":
					case "State":
						dataSet = _adapter.GetDataSet(
							dataSetName,
							"Request");
						break;
				}

				if (dataSet == null
					|| !dataSet.Any())
				{
					throw new KeyNotFoundException($"No DataSet found matching name [{dataSetName}]");
				}

				_memoryCache.Add(
					new CacheItem(
						dataSetName,
						dataSet),
					null);
			}

			return dataSet;
		}
	}
}