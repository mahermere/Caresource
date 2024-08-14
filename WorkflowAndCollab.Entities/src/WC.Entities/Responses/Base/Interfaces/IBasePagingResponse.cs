// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   CareSource.WC.Entities.WC.Entities
//   IBasePagingResponse.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Entities.Responses.Base.Interfaces
{
	internal interface IBasePagingResponse<out TResponse> : IBaseResponse<TResponse>
	{
		/// <summary>
		///    Gets the total items of the i base paging response{ t response} class.
		/// </summary>
		int TotalItems { get; }
	}
}