// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   WorkView
//   IWorkViewObjectAdapter.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.WorkView.Adapters.v4
{
	using System.Collections.Generic;
	using System.Threading.Tasks;
	using CareSource.WC.Entities.Workview.v2;
	using CareSource.WC.Services.WorkView.Models.v4;

	public interface IWorkViewObjectAdapter<TWorkViewClassDataModel>
	{
		/// <summary>
		///    Creates a new WorkView object.
		/// </summary>
		/// <param name="workviewObjects">The WorkView objects in an array. </param>
		/// <returns>
		///    The newly created WorkView class.
		/// </returns>
		Task<IEnumerable<TWorkViewClassDataModel>> CreateNewObjects(
			IEnumerable<TWorkViewClassDataModel> workviewObjects);
		
		TWorkViewClassDataModel CreateObject(WorkviewObjectPostRequest workviewObject);
		
		TWorkViewClassDataModel UpdateObject(
			WorkviewObjectPostRequest workviewObject);

		/// <summary>
		///    Gets a WorkView object.
		/// </summary>
		/// <param name="workviewObject">The WorkView object.</param>
		/// <returns>
		///    the specific WorkView object requested
		/// </returns>
		TWorkViewClassDataModel GetWorkviewObject(TWorkViewClassDataModel workviewObject);

		/// <summary>
		///    Searches the WorkView objects.
		/// </summary>
		/// <param name="workviewObject">The WorkView object.</param>
		/// <returns></returns>
		IEnumerable<TWorkViewClassDataModel> SearchWorkviewObjects(
			TWorkViewClassDataModel workviewObject);

		IEnumerable<string> GetDataSetValues(DataSetRequest request);
	}
}