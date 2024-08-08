// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   WC.Services.OnBase
//   DocumentManager.Keywords.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.OnBase.Managers.v1
{
	using System.Collections.Generic;
	using CareSource.WC.Services.OnBase.Models.v1;

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