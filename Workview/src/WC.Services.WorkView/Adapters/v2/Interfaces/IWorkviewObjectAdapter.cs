// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   Workview
//   IWorkviewObjectAdapter.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.WorkView.Adapters.v2
{
	using System.Collections.Generic;
	using System.Threading.Tasks;

	public interface IWorkviewObjectAdapter<TWorkViewClassDataModel>
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
	}
}