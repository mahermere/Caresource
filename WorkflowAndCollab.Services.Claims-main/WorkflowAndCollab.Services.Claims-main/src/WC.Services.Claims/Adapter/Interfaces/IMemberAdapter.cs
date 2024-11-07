// ------------------------------------------------------------------------------------------------
//  <copyright>
//    Copyright (c) CareSource, 2020-2022.  All rights reserved.
// 
//    Claims
//    IMemberAdapter.cs
//  </copyright>
//  ------------------------------------------------------------------------------------------------

namespace Claims.Adapter.Interfaces
{
	public interface IMemberAdapter<out TMemberModel>
	{
		TMemberModel GetById(string subscriberId,
			int? subscriberSuffix);
	}
}