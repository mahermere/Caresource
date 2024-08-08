// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2022. All rights reserved.
// 
//   Workview
//   IRetrieveAdapter.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.WorkView.Adapters.v5
{
	using System.Collections.Generic;
	using CareSource.WC.Services.WorkView.Models.v5;

	public interface IRetrieveAdapter
	{
		/// <summary>
		///    Gets a WorkView object.
		/// </summary>
		/// <param name="workViewApplicationName"></param>
		/// <param name="className"></param>
		/// <param name="objectId"></param>
		/// <returns>
		///    the specific WorkView object requested
		/// </returns>
		WorkViewObject GetWorkviewObject(
			string workViewApplicationName,
			string className,
			long objectId);

		/// <summary>
		///    Searches the WorkView objects.
		/// </summary>
		/// <param name="workviewObject">The WorkView object.</param>
		/// <returns></returns>
		IEnumerable<WorkViewObject> SearchWorkviewObjects(object workviewObject);

		(bool, Dictionary<string, string[]>) ValidateRequest(
			string workviewApplicationName,
			CreateRequest request);
	}
}