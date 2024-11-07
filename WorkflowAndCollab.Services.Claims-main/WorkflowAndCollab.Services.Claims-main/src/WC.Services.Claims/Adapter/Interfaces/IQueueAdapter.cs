// ------------------------------------------------------------------------------------------------
//  <copyright>
//    Copyright (c) CareSource, 2020-2022.  All rights reserved.
// 
//    Claims
//    IQueueAdapter.cs
//  </copyright>
//  ------------------------------------------------------------------------------------------------

namespace Claims.Adapter.Interfaces
{
	using System.Collections.Generic;

	public interface IQueueAdapter<TQueueModel>
	{
		TQueueModel GetByClaimId(string claimId);

		List<TQueueModel> GetByClaimId(List<string> claimIds);
	}
}