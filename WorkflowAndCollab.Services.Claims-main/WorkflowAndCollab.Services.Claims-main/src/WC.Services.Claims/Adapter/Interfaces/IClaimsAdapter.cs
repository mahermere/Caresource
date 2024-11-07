// ------------------------------------------------------------------------------------------------
//  <copyright>
//    Copyright (c) CareSource, 2020-2022.  All rights reserved.
// 
//    Claims
//    IClaimsAdapter.cs
//  </copyright>
//  ------------------------------------------------------------------------------------------------

namespace Claims.Adapter.Interfaces
{
	using System.Collections.Generic;

	public interface IClaimsAdapter<TClaimModel>
	{
		IEnumerable<TClaimModel> GetByFilter(TClaimModel filter);
		TClaimModel GetById(string id);

		string GetClaimDeniedStatus(string id);
	}
}