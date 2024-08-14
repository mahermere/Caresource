// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   Entities
//   WorkviewObject.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Entities.Workview.v2
{
	using System;
	using System.Collections.Generic;

	public class WorkviewObject
	{
		/// <summary>
		/// Gets or sets the Workview Object Application Identifier
		/// </summary>
		public long? ApplicationId { get; set; }

		/// <summary>
		/// Gets or sets the Workview Object Application Name
		/// </summary>
		public string ApplicationName { get; set; }

		/// <summary>
		/// Gets or sets the Workview Object Class Identifier
		/// </summary>
		public long? ClassId { get; set; }

		/// <summary>
		/// Gets or sets the Workview Object Class Name
		/// </summary>
		public string ClassName { get; set; }

		/// <summary>
		/// Gets or sets the Workview Object Filter Name
		/// </summary>
		public string FilterName { get; set; }

		/// <summary>
		/// Gets or sets the Workview Object Identifier
		/// </summary>
		public long? Id { get; set; }

		/// <summary>
		/// Gets or sets the Workview Object Name
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the Workview Object Revision By
		/// </summary>
		public string RevisionBy { get; set; }

		/// <summary>
		/// Gets or sets the Workview Object Revision Date
		/// </summary>
		public DateTime? RevisionDate { get; set; }

		/// <summary>
		/// Gets or sets the Workview Object Created By
		/// </summary>
		public string CreatedBy { get; set; }

		/// <summary>
		/// Gets or sets the Workview Object Created Date
		/// </summary>
		public DateTime? CreatedDate { get; set; }

		/// <summary>
		/// Gets or sets the Workview Object Filters
		/// </summary>
		/// <remarks>
		/// Filters are used to Limit the results set being returned. They must match in case and
		/// whitespace the name of a column in the Filter in the Workview Class in OnBase
		/// </remarks>
		public IEnumerable<WorkviewFilter> Filters { get; set; }

		/// <summary>
		/// Gets or sets the Workview Object Attributes
		/// </summary>
		/// <remarks>
		///	Attributes are the data points you want returned from the query. They must match in
		/// case and whitespace the name of a column in the Filter in the Workview Class in OnBase
		/// </remarks>
		public IEnumerable<WorkviewAttribute> Attributes { get; set; }
	}
}