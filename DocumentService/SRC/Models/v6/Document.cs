// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   WC.Services.Document
//   Document.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.Document.Models.v6
{
	using System;
	using System.Collections.Generic;

	public class Document
	{
		public Dictionary<string, string> DisplayColumns { get; set; }
		public string FileData { get; set; }
		public string Filename { get; set; }
		public long Id { get; set; }
		public string Name { get; set; }
		public string Type { get; set; }
		public DateTime DocumentDate { get; set; }
	}
}