// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   WorkFlowAndCollab.API.OBAppeals
//   ClaimAppealsManager.cs
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

	public class ClaimAppealsManager : AppealsManagerBase, IClaimAppealsManager<Appeal>
	{
		public ClaimAppealsManager(
			IWorkViewObjectsAdapter<WorkViewObjectsHeader> workviewobjectsbroker)
			: base(workviewobjectsbroker)
		{ }

		protected override void SetFilters()
		{
			List<Filter> filters = new List<Filter>(RequestData.Filters.Count() + 1)
			{
				new Filter(
					ClaimId,
					SearchId)
			};

			filters.AddRange(RequestData.Filters);

			RequestData.Filters = filters;
		}

		protected override void ValidateRequest()
		{
			if (!SearchId.Length.Equals(12))
			{
				throw new Exception("Claim Id must be 12 characters");
			}

			base.ValidateRequest();
		}
	}
}