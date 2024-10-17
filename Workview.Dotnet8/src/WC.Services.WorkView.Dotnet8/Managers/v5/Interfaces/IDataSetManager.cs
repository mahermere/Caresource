// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   Workview
//   IDataSetManager.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.WorkView.Dotnet8.Managers.v5
{
	using System.Collections.Generic;
	using WC.Services.WorkView.Dotnet8.Models.v5;

	public interface IDataSetManager
	{
		IEnumerable<string> GetDataSetValues(
			string workViewApplicationName,
			string className,
			string dataSetName);

	}
}