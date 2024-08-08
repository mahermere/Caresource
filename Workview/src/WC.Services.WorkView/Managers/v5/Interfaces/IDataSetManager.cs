// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   Workview
//   IDataSetManager.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.WorkView.Managers.v5
{
	using System.Collections.Generic;
	using CareSource.WC.Services.WorkView.Models.v5;

	public interface IDataSetManager
	{
		IEnumerable<string> GetDataSetValues(
			string workViewApplicationName,
			string className,
			string dataSetName);

	}
}