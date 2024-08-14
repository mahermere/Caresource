// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   Entities
//   WorkviewBatch.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Entities.Workview.v2
{
	using System.Collections.Generic;

	public class WorkviewObjectBatchRequest 
	{
		/// <summary>
		/// Gets or sets the Workview Object Batch Request Workview Objects
		/// </summary>
		public IEnumerable<WorkviewObjectPostRequest> WorkviewObjects { get; set; }
	}
}