// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   Entities
//   WorkviewObject.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Entities.WorkView
{
	using System;
	using System.Collections.Generic;

	public class WorkviewObject
	{
		public long? ApplicationId { get; set; }

		public string ApplicationName { get; set; }

		public long? ClassId { get; set; }

		public string ClassName { get; set; }

		public long? Id { get; set; }

		public string Name { get; set; }

		public string RevisionBy { get; set; }

		public DateTime? RevisionDate { get; set; }

		public string CreatedBy { get; set; }

		public DateTime? CreatedDate { get; set; }

		public List<WorkviewAttribute> Attributes { get; set; }
	}
}