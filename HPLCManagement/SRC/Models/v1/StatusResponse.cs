// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2023. All rights reserved.
// 
//   WC.Services.Hplc
//   StatusResponse.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace HplcManagement.Models.v1
{
	using System;
	using System.Collections.Generic;

	public class StatusResponse
	{
		public long RequestId { get; set; }
		public string RequestType { get; set; }
		public long? ApplicationId { get; set; }
		public DateTime RequestDate { get; set; }
		public IEnumerable<Data> Providers { get; set; }
	}
}