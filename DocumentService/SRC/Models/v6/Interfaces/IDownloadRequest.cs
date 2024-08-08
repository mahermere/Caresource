// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   WC.Services.Document
//   IDownloadRequest.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.Document.Models.v6
{
	using System.Collections.Generic;
	using CareSource.WC.OnBase.Core.Diagnostics;

	public interface IDownloadRequest: IRequest
	{
		long DocumentId { get; set; }
		IEnumerable<string> DisplayColumns { get; set; }
	}
}