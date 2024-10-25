// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   WC.Services.Document
//   BatchUpdateKeywordsRequest.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.Document.Dotnet8.Models.v6.Requests
{
	using System.Collections.Generic;
	using CareSource.WC.Entities.Requests.Base;
    using WC.Services.Document.Dotnet8.Models.v6.Interfaces;

    /// <summary>
    ///    request class used to apply batch keyword update capability to the Document service
    /// </summary>
    public class BatchUpdateKeywordsRequest : BaseRequest<IEnumerable<KeywordUpdate>>,IRequest
	{ }
}