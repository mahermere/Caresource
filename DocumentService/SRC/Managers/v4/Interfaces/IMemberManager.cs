// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   WC.Services.Document
//   IMemberManager.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.Document.Managers.v4
{
	using System.Collections.Generic;
	using CareSource.WC.Services.Document.Models.v4;

	public interface IMemberManager
	{
		IDictionary<string, int> GetDocumentTypesCount(
			string memberId,
			DocumentTypesCountRequest request);
	}
}