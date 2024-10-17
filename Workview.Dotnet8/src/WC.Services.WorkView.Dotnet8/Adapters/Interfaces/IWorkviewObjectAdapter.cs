// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   Workview
//   IWorkviewObjectAdapter.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.WorkView.Dotnet8.Adapters
{
	using System.Collections.Generic;

	public interface IWorkviewObjectAdapter<TWorkViewClassDataModel>
	{
		/// <summary>
		///    Creates a new workview object.
		/// </summary>
		/// <param name="workviewObject">The workview object. </param>
		/// <returns>
		///    The newly created workview class.
		/// </returns>
		TWorkViewClassDataModel CreateNewObject(TWorkViewClassDataModel workviewObject);

		/// <summary>
		///    Gets a workview object.
		/// </summary>
		/// <param name="workviewObject">The workview object.</param>
		/// <returns>
		///    the specific workview object requested
		/// </returns>
		TWorkViewClassDataModel GetWorkviewObject(TWorkViewClassDataModel workviewObject);

		/// <summary>
		///    Searches the workview objects.
		/// </summary>
		/// <param name="workviewObject">The workview object.</param>
		/// <returns></returns>
		IEnumerable<TWorkViewClassDataModel> SearchWorkviewObjects(
			TWorkViewClassDataModel workviewObject);
	}
}