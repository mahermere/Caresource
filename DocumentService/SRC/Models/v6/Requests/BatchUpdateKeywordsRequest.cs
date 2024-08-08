// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   WC.Services.Document
//   BatchUpdateKeywordsRequest.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.Document.Models.v6
{
	using System.Collections.Generic;
	using CareSource.WC.Entities.Requests.Base;

	/// <summary>
	///    request class used to apply batch keyword update capability to the Document service
	/// </summary>
	public class BatchUpdateKeywordsRequest : BaseRequest<IEnumerable<KeywordUpdate>>,OnBase.Core.Diagnostics.IRequest
	{ }
}