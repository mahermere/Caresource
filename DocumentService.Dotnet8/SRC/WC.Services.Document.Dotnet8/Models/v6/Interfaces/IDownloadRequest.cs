// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   WC.Services.Document
//   IDownloadRequest.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.Document.Dotnet8.Models.v6.Interfaces
{
	using System.Collections.Generic;
	//using CareSource.WC.OnBase.Core.Diagnostics;

	public interface IDownloadRequest: IRequest
	{
		long DocumentId { get; set; }
		IEnumerable<string> DisplayColumns { get; set; }
	}
}