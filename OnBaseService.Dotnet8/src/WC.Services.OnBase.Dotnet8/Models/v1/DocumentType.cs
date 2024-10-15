// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   WC.Services.OnBase
//   DocumentType.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.OnBase.Dotnet8.Models.v1
{
	using System.Collections.Generic;
    using WC.Services.OnBase.Dotnet8.Models.Base;

    public class DocumentType : BaseModel
	{
		public IEnumerable<Keyword> Keywords { get; set; }
	}
}