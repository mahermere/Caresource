// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   CareSource.WC.Entities
//   GetClaimRequest.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Entities.Claims
{
	using CareSource.WC.Entities.Requests.Base;

	public class GetClaimRequest : BaseRequest<GetClaimData>
	{ }

	public class GetClaimData
	{
		public string ClaimId { get; set; }
	}
}