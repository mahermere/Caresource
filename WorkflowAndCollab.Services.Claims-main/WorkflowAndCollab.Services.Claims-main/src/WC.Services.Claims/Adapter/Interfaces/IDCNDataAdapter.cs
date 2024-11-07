// ------------------------------------------------------------------------------------------------
//  <copyright>
//    Copyright (c) CareSource, 2020-2022.  All rights reserved.
// 
//    Claims
//    IDCNDataAdapter.cs
//  </copyright>
//  ------------------------------------------------------------------------------------------------

namespace Claims.Adapter.Interfaces
{
	public interface IDCNDataAdapter<out TDCNDataModel>
	{
		TDCNDataModel GetDataByDCN(string microId);
	}
}