// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2018. All rights reserved.
// 
//   CareSource.WC.Entities
//   GetEligibilityRequest.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

using CareSource.WC.Entities.Requests.Base;

namespace CareSource.WC.Entities.Eligibility
{
	public class GetEligibilityRequest : BaseRequest<GetEligibilityRequestData>
	{ }

	public class GetEligibilityRequestData
	{
		public long? ContrivedKey { get; set; }
	}
}