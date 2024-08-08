// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   Workview
//   IDataSetManager.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.WorkView.Managers.v4
{
	using System.Collections.Generic;
	using CareSource.WC.Services.WorkView.Models.v4;

	public interface IDataSetManager
	{
		IEnumerable<string> GetDataSetValues(DataSetRequest request);

	}
}