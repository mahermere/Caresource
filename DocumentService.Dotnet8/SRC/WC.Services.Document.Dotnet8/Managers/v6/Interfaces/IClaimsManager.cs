// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   WC.Services.Document
//   IClaimsManager.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.Document.Dotnet8.Managers.v6.Interfaces
{
	using System.Collections.Generic;
	using WC.Services.Document.Dotnet8.Models.v6;

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