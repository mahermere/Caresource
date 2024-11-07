// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   CareSource.WC.Core.Integrations.Facets
//   IEligibilityAdapter.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.Eligibility.Adapters.Interfaces
{
	using System.Collections.Generic;

	public interface IEligibilityAdapter<out TEligibilityModel>
	{
		IEnumerable<TEligibilityModel> GetEligibility(long? contrivedKey);
    }
}