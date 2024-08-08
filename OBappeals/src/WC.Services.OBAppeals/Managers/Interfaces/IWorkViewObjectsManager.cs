// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   WorkFlowAndCollab.API.OBAppeals
//   IWorkViewObjectsManager.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.OBAppeals.Managers.Interfaces
{
	using CareSource.WC.Entities.Appeals.Interfaces;
	using CareSource.WC.Entities.Common.Interfaces;

	public interface IWorkViewObjectsManager<TDataModel>
	{
		ISearchResults<TDataModel> Search(
			string id,
			IListWorkViewObjectsRequest requestData);
	}
}