// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   WC.Services.OnBase
//   DocumentAdapter.KeywordTypeGroups.cs
// </copyright>
// ------------------------------------------------------------------------------------------------
namespace CareSource.WC.Services.OnBase.Adapters.v1
{
	using System.Collections.Generic;
	using System.Linq;
	using CareSource.WC.Services.OnBase.Models.v1;

	public partial class DocumentAdapter
	{
		public IEnumerable<KeywordTypeGroup> KeywordTypeGroups()
			=> _applicationCore.Application.Core.KeywordRecordTypes
				.Select(
					kw => new KeywordTypeGroup() {Name = kw.Name, Id = kw.ID});
	}
}