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

	public interface IWorkviewObjectPostRequest
	{
		/// <summary>
		/// Gets or sets the I Work View Object Post Request Object Identifier
		/// </summary>
		long ObjectId { get; set; }

		/// <summary>
		/// Gets or sets the I Work View Object Post Request Application Name
		/// </summary>
		string ApplicationName { get; set; }

		/// <summary>
		/// Gets or sets the I Work View Object Post Request Class Name
		/// </summary>
		string ClassName { get; set; }

		/// <summary>
		/// Gets or sets the I Work View Object Post Request Attributes
		/// </summary>
		IEnumerable<WorkviewAttributeRequest> Attributes { get; set; }
	}
}