// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   WC.Services.Document
//   DownloadRequest.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.Document.Models.v6
{
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;

	public class DownloadRequest : Request, IDownloadRequest
	{
		[Required]
		public long DocumentId { get; set; }

		public IEnumerable<string> DisplayColumns { get; set; } = new List<string>();
	}
}