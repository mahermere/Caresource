// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   WC.Services.Document
//   CreateDocumentFileLink.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.Document.Dotnet8.Models.v6
{
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;

	public class CreateDocumentFileLink
	{
		[Required]
		public string DocumentType { get; set; }

		[Required]
		public string FileName { get; set; }

		[Required]
		public IDictionary<string, string> Keywords { get; set; }
	}
}