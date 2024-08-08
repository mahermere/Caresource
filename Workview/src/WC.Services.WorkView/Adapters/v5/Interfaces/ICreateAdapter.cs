// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   WorkView
//   IWorkViewObjectAdapter.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.WorkView.Adapters.v5
{
	using System.Collections.Generic;
	using System.Threading.Tasks;
	using CareSource.WC.Services.WorkView.Models.v5;

	public interface ICreateAdapter
	{
		Task<IEnumerable<WorkViewObject>> AsyncCreateObjects(
			IEnumerable<WorkViewBaseObject> workviewObjects);

		(bool, Dictionary<string, string[]>) ValidateRequest(
			string workviewApplicationName,
			CreateRequest request);

		IEnumerable<WorkViewObject> CreateObjects(
			IEnumerable<WorkViewBaseObject> workviewObjects);
	}
}