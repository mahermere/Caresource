// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2023. All rights reserved.
// 
//   WC.Services.Document
//   DocumentHeader.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.Document.Dotnet8.Models.v6
{
	using System;
	using System.Collections.Generic;

	public class DocumentHeader
	{
		public IDictionary<string, object> DisplayColumns { get; set; }
		public DateTime DocumentDate { get; set; }
		public long DocumentId { get; set; }
		public string DocumentName { get; set; }
		public string DocumentType { get; set; }
	}
}