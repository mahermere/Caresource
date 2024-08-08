// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   WorkFlowAndCollab.Integrations
//   IWVObjectsBroker.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.OBAppeals.Adapters.Interfaces
{
	using CareSource.WC.Entities.Appeals;
	using CareSource.WC.Entities.Appeals.Interfaces;
	using CareSource.WC.Entities.Common.Interfaces;

	public interface IWorkViewObjectsAdapter<TDataModel>
	{
		ISearchResults<TDataModel> Search(IListWorkViewObjectsRequest request);
		WorkViewObjectsHeader GetWVObject(long objectId, WorkviewObjectItemRequest request);
	}
}