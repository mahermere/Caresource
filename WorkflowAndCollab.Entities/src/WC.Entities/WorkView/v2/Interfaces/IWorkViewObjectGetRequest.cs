// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   Entities
//   IWorkViewObjectRequest.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Entities.Workview.v2
{
	using System.Collections.Generic;

	internal interface IWorkviewObjectGetRequest
	{
		/// <summary>
		/// Gets or sets the I Work View Object Get Request Object Identifier
		/// </summary>
		long ObjectId { get; set; }

		string ApplicationName { get; set; }

		/// <summary>
		/// Gets or sets the I Work View Object Get Request Class Name
		/// </summary>
		string ClassName { get; set; }

		/// <summary>
		/// Gets or sets the I Work View Object Get Request Filter Name
		/// </summary>
		string FilterName { get; set; }

		/// <summary>
		/// Gets or sets the I Work View Object Get Request Attributes
		/// </summary>
		IEnumerable<string> Attributes { get; set; }

		/// <summary>
		/// Gets or sets the I Work View Object Get Request Filters
		/// </summary>
		IEnumerable<WorkviewFilterRequest> Filters { get; set; }
	}
}