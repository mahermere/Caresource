// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   WC.Services.Document
//   IClaimsManager.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.Document.Managers.v5
{
	using System.Collections.Generic;
	using CareSource.WC.Services.Document.Models.v5;

	public interface IClaimsManager<out TDataModel>
	{
		IEnumerable<Document> ClaimSearch(
			string claimId,
			ClaimsRequest request);

		IEnumerable<Document> MemberSearch(
			string memberId,
			ClaimsRequest request);

		IEnumerable<Document> ProviderSearch(
			string providerId,
			ClaimsRequest request);
	}
}