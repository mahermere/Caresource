// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   WC.Services.OnBase
//   DocumentTypeGroup.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.OnBase.Dotnet8.Models.v1
{
	using System.Collections.Generic;
    using WC.Services.OnBase.Dotnet8.Models.Base;

    public class DocumentTypeGroup : BaseModel
	{
		public IEnumerable<DocumentType> DocumentTypes { get; set; }
	}
}