// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   CareSource.WC.Core.Integrations.Facets
//   IEligibilityManager.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.Eligibility.Managers.Interfaces
{
	using System.Collections.Generic;

	public interface IEligibilityManager<out TEligibilityModel>
	{
		IEnumerable<TEligibilityModel> GetEligibilities(string memberId);
	}
}