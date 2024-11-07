// ------------------------------------------------------------------------------------------------
//  <copyright>
//    Copyright (c) CareSource, 2020-2022.  All rights reserved.
// 
//    Claims
//    IClaimsManager.cs
//  </copyright>
//  ------------------------------------------------------------------------------------------------

namespace Claims.Managers.Interfaces
{
	using System.Collections.Generic;
	using Claims.Models;

	public interface IClaimsManager
	{
		List<Claim> GetByFilter(int? page,
			int? pageSize,
			Claim filter);

		Claim GetById(string id);

		string GetClaimDeniedStatus(
			string id);

		DCNClaimData GetDataByDCN(string id);
	}
}