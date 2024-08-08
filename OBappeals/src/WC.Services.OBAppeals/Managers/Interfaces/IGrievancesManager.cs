// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   WorkFlowAndCollab.API.OBAppeals
//   IGrievancesManager.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.OBAppeals.Managers.Interfaces
{
	using CareSource.WC.Entities.Appeals;
	using CareSource.WC.Entities.Common.Interfaces;

	public interface IGrievancesManager<TDataModel>
	{
		ISearchResults<TDataModel> Search(string Id, ListAppealsRequest request);

		Appeal GetAppeal(long objectId, WorkviewObjectItemRequest request);
	}
}