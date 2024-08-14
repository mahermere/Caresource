// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   Entities
//   IPagingRequest.cs
// </copyright>
// ------------------------------------------------------------------------------------------------
namespace CareSource.WC.Entities.Requests.Base.Interfaces
{
	public interface IPagingRequest: IBaseRequest
	{
		/// <summary>
		///    Gets or sets the request data of the base request{ t request} class.
		/// </summary>
		Paging Paging { get; set; }
	}
}