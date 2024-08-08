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

	public interface ISearchAdapter
	{
		/// <summary>
		///    Gets a WorkView object.
		/// </summary>
		/// <param name="workViewApplicationName">The application to search</param>
		/// <param name="request">The parameters to search for</param>
		/// <returns>
		///    A list of items matching the criteria.
		/// </returns>
		IEnumerable<WorkViewObject> Search(
			string workViewApplicationName,
			SearchRequest request);

		(bool, Dictionary<string, string[]>) ValidateRequest(
			string workviewApplicationName,
			SearchRequest request);
	}
}