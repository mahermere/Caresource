// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   WorkFlowAndCollab.API.OBAppeals
//   ProviderAppealsManager.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.OBAppeals.Managers
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using CareSource.WC.Entities.Appeals;
	using CareSource.WC.Entities.Common;
	using CareSource.WC.Services.OBAppeals.Adapters.Interfaces;
	using CareSource.WC.Services.OBAppeals.Managers.Interfaces;

	public class ProviderAppealsManager : AppealsManagerBase, IProviderAppealsManager<Appeal>
	{
		public ProviderAppealsManager(
			IWorkViewObjectsAdapter<WorkViewObjectsHeader> workviewobjectsbroker)
			: base(workviewobjectsbroker)
		{ }

		protected override void SetFilters()
		{
			// create a new list for filters; it has one more than the count of passed in filters so
			// we can add [Provider ID]
			List<Filter> filters = new List<Filter>(RequestData.Filters.Count() + 1)
			{
				new Filter(
					ProviderId,
					SearchId)
			};

			filters.AddRange(RequestData.Filters);

			RequestData.Filters = filters;
		}

		protected override void ValidateRequest()
		{
			if (SearchId.Length > 12)
			{
				throw new Exception("Provider Id must be 12 characters or less");
			}

			base.ValidateRequest();
		}
	}
}