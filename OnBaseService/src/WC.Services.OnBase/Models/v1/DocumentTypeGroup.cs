// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   WC.Services.OnBase
//   DocumentTypeGroup.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.OnBase.Models.v1
{
	using System.Collections.Generic;
	using CareSource.WC.Services.OnBase.Models.Base;

	public class DocumentTypeGroup : BaseModel
	{
		public IEnumerable<DocumentType> DocumentTypes { get; set; }
	}
}