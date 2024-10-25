// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   WC.Services.Document
//   DocumentController.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.Document.Dotnet8.Models.v6
{
	using System.Collections.Generic;
	using CareSource.WC.Entities.Requests.Base;
    using WC.Services.Document.Dotnet8.Models.v6.Interfaces;

    //using CareSource.WC.OnBase.Core.Diagnostics;

    public class DocumentKeywordRequest : BaseRequest, IRequest
	{
		public IEnumerable<string> DisplayColumns { get; set; }
	}
}