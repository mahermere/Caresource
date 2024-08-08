// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2022. All rights reserved.
// 
//   Workview
//   Class1.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.WorkView.Models.v5
{
	using System;
	using System.Collections.Generic;
	public class WorkViewObject : WorkViewBaseObject
	{
		public string CreatedBy { get; set; }

		public DateTime CreatedDate { get; set; }

		public long Id { get; set; }

		public string Name { get; set; }

		public string RevisionBy { get; set; }

		public DateTime? RevisionDate { get; set; }

		public new IEnumerable<WorkViewObject> Related { get; set; } = new List<WorkViewObject>();
	}
}