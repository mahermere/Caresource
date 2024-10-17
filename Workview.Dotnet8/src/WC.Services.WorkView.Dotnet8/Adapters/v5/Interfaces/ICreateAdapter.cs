// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   WorkView
//   IWorkViewObjectAdapter.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.WorkView.Dotnet8.Adapters.v5
{
	using System.Collections.Generic;
	using System.Threading.Tasks;
	using WC.Services.WorkView.Dotnet8.Models.v5;

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