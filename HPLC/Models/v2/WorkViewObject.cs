// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2021. All rights reserved.
// 
//   WC.Services.Hplc
//   WorkViewObject.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.Hplc.Models.v2
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	public class WorkViewObject
	{
		public long? ApplicationId { get; set; }

		public string ApplicationName { get; set; }

		public List<Attribute> Attributes { get; set; }

		public IEnumerable<string> AttributeValues
			=> Attributes.Where(a => a.Value != null).Select(a =>$"{a.Name}:{a.Value}");

		public long? ClassId { get; set; }

		public string ClassName { get; set; }

		public string CreatedBy { get; set; }

		public DateTime CreatedDate { get; set; }

		public long Id { get; set; }

		public string Name { get; set; }

		public string RevisionBy { get; set; }

		public DateTime? RevisionDate { get; set; }

		public IEnumerable<WorkViewObject> Related { get; set; }
	}
}