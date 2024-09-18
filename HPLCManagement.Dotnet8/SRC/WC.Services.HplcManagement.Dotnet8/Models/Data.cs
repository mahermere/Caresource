// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2023. All rights reserved.
// 
//   WC.Services.HplcManagement
//   Data.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.HplcManagement.Dotnet8.Models
{
    using System.Collections.Generic;

    public class Data : BaseWorkViewEntity
    {
        public Dictionary<string, string> Properties { get; set; }
            = new Dictionary<string, string>();

        public IEnumerable<Data> Related { get; set; }
            = new List<Data>();
    }
}