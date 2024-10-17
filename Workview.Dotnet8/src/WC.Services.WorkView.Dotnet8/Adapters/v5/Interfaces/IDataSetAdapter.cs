// ------------------------------------------------------------------------------------------------
//  <copyright>
//    Copyright (c) CareSource, 2020-2022.  All rights reserved.
// 
//    Workview
//    IDataSetAdapter.cs
//  </copyright>
//  ------------------------------------------------------------------------------------------------

namespace WC.Services.WorkView.Dotnet8.Adapters.v5
{
	using System.Collections.Generic;
	using Hyland.Unity.WorkView;

	public interface IDataSetAdapter
	{
		IEnumerable<string> GetDataSet(
			string workViewApplicationName,
			string className,
			long attributeId);

		IEnumerable<string> GetDataSet(
			string workViewApplicationName,
			string className,
			string dataSetName);

		IEnumerable<string> GetDataSet(
			string workViewApplicationName,
			Class wvClass,
			long attributeId);
	}
}