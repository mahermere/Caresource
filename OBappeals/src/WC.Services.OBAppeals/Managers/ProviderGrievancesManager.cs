// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   WorkFlowAndCollab.API.OBAppeals
//   ProviderGrievancesManager.cs
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

	public class ProviderGrievancesManager : GrievancesManagerBase,
		IProviderGrievancesManager<Appeal>
	{
		public ProviderGrievancesManager(
			IWorkViewObjectsAdapter<WorkViewObjectsHeader> workviewobjectsbroker)
			: base(workviewobjectsbroker)
		{ }

		protected override void SetFilters()
		{
			List<Filter> filters = new List<Filter>(RequestData.Filters.Count() + 1)
			{
				new Filter(
					ProviderID,
					SearchId)
			};

			if (!filters.Any(f => f.Name.Equals("classname")))
			{
				filters.Add(
					new Filter(
						"classname",
						"Grievance"));
			}

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