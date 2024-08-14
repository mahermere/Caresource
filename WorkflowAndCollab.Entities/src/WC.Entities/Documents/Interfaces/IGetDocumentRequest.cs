// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   Caresource.WC.Entities.WC.Entities
//   IGetDocumentRequest.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Entities.Documents.Interfaces
{
	using System.Collections.Generic;
	using CareSource.WC.Entities.Requests.Base.Interfaces;

	public interface IGetDocumentRequest : IBaseRequest
	{
		 IEnumerable<string> DisplayColumns { get; set; }
	}
}