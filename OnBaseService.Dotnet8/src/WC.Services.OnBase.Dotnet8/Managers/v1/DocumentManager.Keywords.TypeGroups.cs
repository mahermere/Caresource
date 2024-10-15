// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   WC.Services.OnBase
//   DocumentManager.ListKeywordTypeGroups.cs
// </copyright>
// ------------------------------------------------------------------------------------------------
namespace WC.Services.OnBase.Dotnet8.Managers.v1
{
	using System.Collections.Generic;
    using global::WC.Services.OnBase.Dotnet8.Models.v1;

    public partial class DocumentManager
	{
		public IEnumerable<KeywordTypeGroup> ListKeywordTypeGroups()
			=> _adapter.KeywordTypeGroups();
	}
}