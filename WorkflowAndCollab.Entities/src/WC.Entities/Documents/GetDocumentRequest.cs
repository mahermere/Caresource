// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   Caresource.WC.Entities.WC.Entities
//   GetDocumentRequest.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Entities.Documents
{
	using System.Collections.Generic;
	using CareSource.WC.Entities.Documents.Interfaces;
	using CareSource.WC.Entities.Requests.Base;

	public class GetDocumentRequest
		: BaseRequest,
			IGetDocumentRequest
	{
		public IEnumerable<string> DisplayColumns { get; set; }
			= new HashSet<string>();

	}
}