// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2021. All rights reserved.
// 
//   WC.Services.Hplc
//   BaseWorkViewEntity.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.Hplc.Models.v2
{
	using System;
	using Newtonsoft.Json;

	public class BaseWorkViewEntity
	{
		[JsonIgnore]
		public long? ApplicationId { get; set; }

		[JsonIgnore]
		public string ApplicationName { get; set; }

		[JsonIgnore]
		public long? ClassId { get; set; }

		[JsonIgnore]
		public string ClassName { get; set; }

		[JsonIgnore]
		public string CreatedBy { get; set; }

		[JsonIgnore]
		public DateTime CreatedDate { get; set; }

		public long Id { get; set; }

		[JsonIgnore]
		public string Name { get; set; }

		[JsonIgnore]
		public string RevisionBy { get; set; }

		[JsonIgnore]
		public DateTime? RevisionDate { get; set; }
	}
}