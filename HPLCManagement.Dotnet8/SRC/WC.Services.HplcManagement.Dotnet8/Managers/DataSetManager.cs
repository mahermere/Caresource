// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   WC.Services.Hplc
//   DataSetManager.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.HplcManagement.Dotnet8.Managers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Extensions.Caching.Memory;
   // using CareSource.WC.OnBase.Core.Diagnostics.Interfaces;
   // using CareSource.WC.OnBase.Core.ExtensionMethods;
    using WC.Services.HplcManagement.Dotnet8;
    using WC.Services.HplcManagement.Dotnet8.Adapters.WorkView;
    using WC.Services.HplcManagement.Dotnet8.Managers.Interfaces;
    using WC.Services.HplcManagement.Dotnet8.Models;
    
    /// <summary>
    /// Data and functions describing a CareSource.WC.Services.Hplc.Managers.v1.DataSetManager object.
    /// </summary>
    /// <seealso cref="IDataSetManager" />
    public class DataSetManager : IDataSetManager
    {
       
        private readonly log4net.ILog _logger;
        private readonly IAdapter _adapter;
        private readonly IMemoryCache _memoryCache;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataSetManager"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="adapter">The adapter.</param>
        /// <param name="memoryCache">The memory cache.</param>
        public DataSetManager(
            log4net.ILog logger,
            IAdapter adapter,
            IMemoryCache memoryCache)
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
        public IEnumerable<string> GetDataSet(string dataSetName)
        {
            if (String.IsNullOrWhiteSpace(dataSetName))
            {
                throw new ArgumentNullException(
                    nameof(dataSetName),
                    "A DataSet Name is Required");
            }

            string v1CacheName = $"V1 - {dataSetName}";
            IEnumerable<string> dataSet = (IEnumerable<string>)_memoryCache.Get(v1CacheName);

            if (dataSet == null)
            {
                _logger.Debug($"{dataSetName} not in Cache, retrieving form DB.");
                switch (dataSetName)
                {
                    case Constants.DataSetNames.ApiProducts:
                        IDictionary<long, string> data = _adapter.GetClassAsDataSet(
                            dataSetName,
                            "Product");

                       _memoryCache.Set(
                            $"{v1CacheName} ObjectIds",
                            data, 
                            new MemoryCacheEntryOptions());

                        dataSet = data.Select(e => e.Value);
                        break;

                    case Constants.DataSetNames.ProviderType:
                    case Constants.DataSetNames.ActionType:
                    case Constants.DataSetNames.HplcProviderLanguage:
                        dataSet = _adapter.GetDataSet(
                            dataSetName,
                            "Provider");
                        break;

                    case Constants.DataSetNames.PhoneTypeProvider:
                        dataSet = _adapter.GetDataSet(
                            dataSetName,
                            "Phone");
                        break;

                    case Constants.DataSetNames.AddressType:
                    case Constants.DataSetNames.ActionTypeProviderLocations:
                        dataSet = _adapter.GetDataSet(
                            dataSetName,
                            "Location");
                        break;

                    case Constants.DataSetNames.HealthPartnerAgreementType:
                    case Constants.DataSetNames.ProviderMaintenanceRequestTypes:
                    case Constants.DataSetNames.State:
                        dataSet = _adapter.GetDataSet(
                            dataSetName,
                            "Request");
                        break;
                }

                if (!dataSet.SafeAny())
                {
                    throw new KeyNotFoundException($"No DataSet found matching name [{dataSetName}]");
                }

                dataSet = dataSet.Distinct().OrderBy(d => d);

                // _memoryCache.Add(
                //     new CacheItem(
                //         v1CacheName,
                //         dataSet),
                //     null);
                _memoryCache.Set(
                    v1CacheName,
                    dataSet,
                    new MemoryCacheEntryOptions { SlidingExpiration = TimeSpan.FromHours(1) });
            }

            return dataSet;
        }
    }
}