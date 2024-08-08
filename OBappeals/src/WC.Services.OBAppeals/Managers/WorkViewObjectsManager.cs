// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   WorkFlowAndCollab.API.WorkViewObjects
//   WorkViewObjectsManager.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WorkFlowAndCollab.API.Appeals.Managers
{
	using System.Linq;
	using WorkFlowAndCollab.Integrations.WorkViewObjects.Interfaces;
	using WorkFlowAndCollab.API.Appeals.Managers.Interfaces;
	using CareSource.WC.Entities.WorkView;
	using CareSource.WC.Entities.WorkView.Interfaces;
	using CareSource.WC.Entities.Common.Interfaces;
	using CareSource.WC.OnBase.Core.ExtensionMethods;

	public abstract class WorkViewObjectsManager : IWorkViewObjectsManager<WorkViewObjectsHeader>
	{
		protected WorkViewObjectsManager(
			IWorkViewObjectsBroker<WorkViewObjectsHeader> workviewobjectsbroker)
			=> WorkViewObjectsBroker = workviewobjectsbroker;

		protected IWorkViewObjectsBroker<WorkViewObjectsHeader> WorkViewObjectsBroker { get; }

		protected string SearchId { get; set; }

		protected IListWorkViewObjectsRequest RequestData { get; set; }

		public ISearchResults<WorkViewObjectsHeader> Search(string searchId, IListWorkViewObjectsRequest requestData)
		{
			SearchId = searchId.SafeTrim();
			RequestData = requestData;

			ValidateRequest();
			SetFilters();
			SetDisplayColumns();

			ISearchResults<WorkViewObjectsHeader> objects = WorkViewObjectsBroker.Search(requestData);

			if (!objects.Results.Any())
			{
				//throw new NoDocumentsException($"No Grievance or Appeals found for Id: {SearchId}.");
			}

			return objects;
		}

		protected abstract void SetDisplayColumns();

		/// <summary>
		///    Sets the filters.
		/// </summary>
		protected abstract void SetFilters();

		protected virtual void ValidateRequest()
		{ }
	}
}