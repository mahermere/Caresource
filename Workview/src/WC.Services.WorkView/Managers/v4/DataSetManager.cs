// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   Workview
//   DataSetManager.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.WorkView.Managers.v4
{
	using System.Collections.Generic;
	using CareSource.WC.Entities.Workview.v2;
	using CareSource.WC.Services.WorkView.Adapters.v4;
	using CareSource.WC.Services.WorkView.Models.v4;

	public class DataSetManager : IDataSetManager
	{
		private readonly IWorkViewObjectAdapter<WorkviewObject> _workviewObjectAdapter;

		public DataSetManager(IWorkViewObjectAdapter<WorkviewObject> workviewObjectAdapter)
		{
			_workviewObjectAdapter = workviewObjectAdapter;
		}

		public IEnumerable<string> GetDataSetValues(DataSetRequest request)
		{
			return _workviewObjectAdapter.GetDataSetValues(request);
		}
	}
}