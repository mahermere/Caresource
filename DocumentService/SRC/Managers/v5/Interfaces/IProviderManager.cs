// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   WC.Services.Document
//   IProviderManager.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.Document.Managers.v5
{
	public interface IProviderManager: IDocumentManager
	{
		string ProviderId { get; set; }

		string ProviderTin { get; set; }
	}
}