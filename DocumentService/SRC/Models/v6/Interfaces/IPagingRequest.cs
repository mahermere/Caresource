// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2023. All rights reserved.
// 
//   WC.Services.Document
//   IPagingRequest.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.Document.Models.v6
{
	using CareSource.WC.Entities.Requests.Base;

	public interface IPagingRequest : IRequest
	{
		Paging Paging { get; set; }
	}
}