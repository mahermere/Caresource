// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   WorkFlowAndCollab.API.OBAppeals
//   MemberGrievancesManager.cs
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
	using CareSource.WC.Services.OBAppeals.Managers.Interfaces;

	public class MemberGrievancesManager : GrievancesManagerBase, IMemberGrievancesManager<Appeal>
	{
		public MemberGrievancesManager(
			IWorkViewObjectsAdapter<WorkViewObjectsHeader> workviewobjectsbroker)
			: base(workviewobjectsbroker)
		{ }

		protected override void SetFilters()
		{
			List<Filter> filters = new List<Filter>(RequestData.Filters.Count() + 1)
			{
				new Filter(
					MemberID,
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
			if (!SearchId.Length.Equals(11))
			{
				throw new Exception("Member Id must be 11 characters");
			}

			if (!Regex.IsMatch(SearchId, @"^\d+$"))
			{
				throw new Exception("Member Id must be numeric");
			}

			base.ValidateRequest();
		}
	}
}