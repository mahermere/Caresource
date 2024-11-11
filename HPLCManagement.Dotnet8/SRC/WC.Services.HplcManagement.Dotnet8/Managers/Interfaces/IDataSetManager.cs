﻿// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   WC.Services.Hplc
//   IDataSetManager.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.HplcManagement.Dotnet8.Managers.Interfaces
{
    using System.Collections.Generic;

    /// <summary>
    /// 
    /// </summary>
    public interface IDataSetManager
    {
        /// <summary>
        /// Gets the data set.
        /// </summary>
        /// <param name="dataSetName">The data set name.</param>
        /// <returns></returns>
        IEnumerable<string> GetDataSet(string dataSetName);
    }
}