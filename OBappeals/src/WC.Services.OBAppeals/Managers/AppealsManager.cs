// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   WorkFlowAndCollab.API.OBAppeals
//   AppealsManager.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.OBAppeals.Managers
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text.RegularExpressions;
	using CareSource.WC.Entities.Appeals;
	using CareSource.WC.Entities.Common;
	using CareSource.WC.Services.OBAppeals.Adapters.Interfaces;

	public class AppealsManager : AppealsManagerBase
	{
		public AppealsManager(IWorkViewObjectsAdapter<WorkViewObjectsHeader> workViewObjectsBroker)
			: base(workViewObjectsBroker)
		{ }

		protected override void SetFilters()
		{
			List<Filter> filters = new List<Filter>(RequestData.Filters.Count() + 1)
			{
				new Filter(
					FacetsGrievanceID,
					SearchId)
			};

			filters.AddRange(RequestData.Filters);

			RequestData.Filters = filters;
		}

		protected override void ValidateRequest()
		{
			if (!SearchId.Length.Equals(12))
			{
				throw new Exception("Appeal Id must be 12 characters");
			}

			if (!Regex.IsMatch(SearchId, @"^\d+$"))
			{
				throw new Exception("Appeal Id must be numeric");
			}

			base.ValidateRequest();
		}
	}
}