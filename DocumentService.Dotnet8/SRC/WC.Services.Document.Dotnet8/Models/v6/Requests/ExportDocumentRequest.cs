// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2023. All rights reserved.
// 
//   WC.Services.Document
//   ExportDocumentRequest.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.Document.Dotnet8.Models.v6.Requests
{
	using System.ComponentModel.DataAnnotations;
    using WC.Services.Document.Dotnet8.Models.v6.Interfaces;

    public class ExportDocumentRequest : SearchRequest, IExportDocumentRequest
	{
		[Required]
		public string Path { get; set; }
	}
}