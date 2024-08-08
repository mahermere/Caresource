// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   WC.Services.Hplc
//   IAdapter.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.Hplc.Adapters.v2.WorkView
{
	using System.Collections.Generic;
	using WC.Services.Hplc.Models.v2;

	/// <summary>
	/// </summary>
	public interface IAdapter 
	{
		/// <summary>
		///    Creates the request.
		/// </summary>
		/// <param name="request">The request.</param>
		/// <returns></returns>
		long CreateRequest(WorkViewObject request);
		
		/// <returns></returns>
		IDictionary<long, string> GetClassAsDataSet(
			string dataSetName,
			string className);

		/// <summary>
		///    Gets the data set.
		/// </summary>
		/// <param name="dataSetName">The data set name.</param>
		/// <param name="className">The class name.</param>
		/// <returns></returns>
		IEnumerable<string> GetDataSet(
			string dataSetName,
			string className);

		/// <summary>
		///    Gets the object.
		/// </summary>
		/// <param name="objectId">The object identifier.</param>
		/// <returns></returns>
		WorkViewObject GetObject(long objectId);

		/// <summary>
		///    Searches the specified class name.
		/// </summary>
		/// <param name="className">The class name.</param>
		/// <param name="filterName">The filter name.</param>
		/// <param name="attributes">The attributes.</param>
		/// <param name="filters">The filters.</param>
		/// <returns></returns>
		IEnumerable<WorkViewObject> Search(
			string className,
			string filterName,
			string[] attributes,
			IDictionary<string, string> filters);

		/// <summary>
		///    Searches the specified class name.
		/// </summary>
		/// <param name="className">The class name.</param>
		/// <param name="filterName">The filter name.</param>
		/// <param name="attributes">The attributes.</param>
		/// <param name="filters">The filters.</param>
		/// <returns></returns>
		IEnumerable<WorkViewObject> HieSearch(
			string className,
			string filterName,
			string[] attributes,
			IDictionary<string, string> filters);
	}
}