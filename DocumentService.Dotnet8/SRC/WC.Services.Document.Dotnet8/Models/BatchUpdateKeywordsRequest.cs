// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   WC.Services.Document
//   BatchUpdateKeywordsRequest.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.Document.Dotnet8.Models
{
	using System.Collections.Generic;
	using CareSource.WC.Entities.Requests.Base;
    using WC.Services.Document.Dotnet8.Models.v6;
    using WC.Services.Document.Dotnet8.Models.v6.Interfaces;

    //using CareSource.WC.OnBase.Core.Diagnostics;

    /// <summary>
    ///  request class used to apply batch keyword update capability to the Document service
    /// </summary>
    public class BatchUpdateKeywordsRequest : BaseRequest<IEnumerable<KeywordUpdate>>, IRequest
	{

	}
}