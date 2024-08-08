// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2021. All rights reserved.
// 
//   ImportProcessor
//   DocTypeNameRequest.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace ImportProcessor.Models.v1
{
	using System;

	public class DocTypeNameRequest
	{
		public long DocumentTypeNumber { get; set; }
		public string CorrelationGuid { get; set; }
		public DateTime RequestDateTime { get; set; }
		public string SourceApplication { get; set; }
		public string UserId { get; set; }
	}
}