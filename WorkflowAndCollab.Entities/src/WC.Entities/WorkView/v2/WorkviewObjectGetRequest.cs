// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   Entities
//   WorkviewObjectRequest.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Entities.Workview.v2
{
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;

	public class WorkviewObjectGetRequest : IWorkviewObjectGetRequest
	{
		public long ObjectId { get; set; }

		/// <summary>
		/// Gets or sets the Workview Object Request Application Name
		/// </summary>
		[Required]
		public string ApplicationName { get; set; }

		/// <summary>
		/// Gets or sets the Workview Object Request Class Name
		/// </summary>
		[Required]
		public string ClassName { get; set; }

		/// <summary>
		/// Gets or sets the Workview Object Request Filter Name
		/// </summary>
		[Required]
		public string FilterName { get; set; }

		/// <summary>
		/// Gets or sets the Workview Object Request Filters
		/// </summary>
		public IEnumerable<WorkviewFilterRequest> Filters { get; set; }

		/// <summary>
		/// Gets or sets the Workview Object Request Attributes
		/// </summary>
		public IEnumerable<string> Attributes { get; set; }
	}
}