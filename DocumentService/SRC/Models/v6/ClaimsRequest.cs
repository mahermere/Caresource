// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   WC.Services.Document
//   ClaimsRequest.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.Document.Models.v6
{
	using System.Collections.Generic;
	using CareSource.WC.Entities.Requests.Base;
	using CareSource.WC.Entities.Requests.Base.Interfaces;

	public class ClaimsRequest : BaseRequest, IPagingRequest
	{
		public IEnumerable<string> DisplayColumns { get; set; } = new List<string>();
		public Paging Paging { get; set; } = new Paging();
	}
}