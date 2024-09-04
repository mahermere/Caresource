// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2023. All rights reserved.
// 
//   WC.Services.Hplc
//   StatusResponse.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.Hplc.Dotnet8.Models
{
    using System;
    using System.Collections.Generic;

    public class StatusResponse
    {
        public long RequestId { get; set; }
        public string RequestType { get; set; }
        public long? ApplicationId { get; set; }
        public DateTime RequestDate { get; set; }
        public IEnumerable<Provider> Providers { get; set; }
    }
}