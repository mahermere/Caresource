// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   WC.Services.Document
//   DownloadRequest.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.Document.Dotnet8.Models.v6.Requests
{
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
    using WC.Services.Document.Dotnet8.Models.v6.Interfaces;

    public class DownloadRequest : Request, IDownloadRequest
	{
		[Required]
		public long DocumentId { get; set; }

		public IEnumerable<string> DisplayColumns { get; set; } = new List<string>();
	}
}