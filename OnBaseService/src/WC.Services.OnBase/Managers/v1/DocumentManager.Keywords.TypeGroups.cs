// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   WC.Services.OnBase
//   DocumentManager.ListKeywordTypeGroups.cs
// </copyright>
// ------------------------------------------------------------------------------------------------
namespace CareSource.WC.Services.OnBase.Managers.v1
{
	using System.Collections.Generic;
	using CareSource.WC.Services.OnBase.Models.v1;

	public partial class DocumentManager
	{
		public IEnumerable<KeywordTypeGroup> ListKeywordTypeGroups()
			=> _adapter.KeywordTypeGroups();
	}
}