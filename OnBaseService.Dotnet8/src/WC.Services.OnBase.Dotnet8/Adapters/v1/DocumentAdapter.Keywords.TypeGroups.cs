// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   WC.Services.OnBase
//   DocumentAdapter.KeywordTypeGroups.cs
// </copyright>
// ------------------------------------------------------------------------------------------------
namespace WC.Services.OnBase.Dotnet8.Adapters.v1
{
    using global::WC.Services.OnBase.Dotnet8.Models.v1;
    using System.Collections.Generic;
	using System.Linq;

	public partial class DocumentAdapter
	{
		public IEnumerable<KeywordTypeGroup> KeywordTypeGroups()
			=> _repo.Application.Core.KeywordRecordTypes
				.Select(
					kw => new KeywordTypeGroup() {Name = kw.Name, Id = kw.ID});
	}
}