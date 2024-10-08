// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2021. All rights reserved.
// 
//   ImportProcessor
//   WorkViewRequest.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.ImportProcessor.Dotnet8.Models.v1
{
	using System;
	using System.Collections.Generic;
	using System.Text.Json.Serialization;

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