// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2021. All rights reserved.
// 
//   ImportProcessor
//   WorkViewRequest.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace ImportProcessor.Models.v1
{
	using System;
	using System.Collections.Generic;
	using Newtonsoft.Json;

	public class WorkViewRequest
	{
		public string ApplicationName { get; set; }
		public string ClassName { get; set; }
		public IEnumerable<Attr> Attributes { get; set; }

		[JsonIgnore]
		public Guid CorrelationGuid { get; set; }
	}


	public class Attr
	{
		public string Name { get; set; }
		public string Value { get; set; }
	}
}