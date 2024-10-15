// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   WC.Services.OnBase
//   DocumentManager.Keywords.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.OnBase.Dotnet8.Managers.v1
{
    using global::WC.Services.OnBase.Dotnet8.Models.v1;
    using System.Collections.Generic;

	public partial class DocumentManager
	{
		public IEnumerable<Keyword> ListKeywords()
			=> _adapter.Keywords();

		public IEnumerable<Keyword> ListKeywords(long documentTypeId)
		{
			return _adapter.Keywords(documentTypeId);
		}
	}
}