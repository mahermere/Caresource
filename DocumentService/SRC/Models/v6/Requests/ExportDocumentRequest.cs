// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2023. All rights reserved.
// 
//   WC.Services.Document
//   ExportDocumentRequest.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.Document.Models.v6
{
	using System.ComponentModel.DataAnnotations;

	public class ExportDocumentRequest : SearchRequest, IExportDocumentRequest
	{
		[Required]
		public string Path { get; set; }
	}
}