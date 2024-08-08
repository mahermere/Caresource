// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   WC.Services.Document
//   BatchUpdateKeywordsRequest.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.Document.Models
{
	using System.Collections.Generic;
	using CareSource.WC.Entities.Requests.Base;
	using CareSource.WC.OnBase.Core.Diagnostics;

	/// <summary>
	///  request class used to apply batch keyword update capability to the Document service
	/// </summary>
	public class BatchUpdateKeywordsRequest : BaseRequest<IEnumerable<KeywordUpdate>>, IRequest
	{

	}
}