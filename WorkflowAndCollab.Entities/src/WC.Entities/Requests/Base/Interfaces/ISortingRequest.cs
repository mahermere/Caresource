// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   Entities
//   ISortingRequest.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Entities.Requests.Base.Interfaces
{
	public interface ISortingRequest: IBaseRequest
	{
		Sort Sorting { get; set; }
	}
}