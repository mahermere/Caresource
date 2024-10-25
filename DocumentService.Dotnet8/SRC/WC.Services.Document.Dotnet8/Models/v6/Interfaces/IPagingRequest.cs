// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2023. All rights reserved.
// 
//   WC.Services.Document
//   IPagingRequest.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.Document.Dotnet8.Models.v6.Interfaces
{
	using CareSource.WC.Entities.Requests.Base;

	public interface IPagingRequest : IRequest
	{
		Paging Paging { get; set; }
	}
}