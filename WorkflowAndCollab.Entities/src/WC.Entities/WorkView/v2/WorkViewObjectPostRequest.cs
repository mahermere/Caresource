// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   Entities
//   WorkViewObjectPostRequest.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Entities.Workview.v2
{
	using System.Collections.Generic;

	public class WorkviewObjectPostRequest : IWorkviewObjectPostRequest
	{
		/// <summary>
		///    Gets or sets the I Work View Object Post Request Object Identifier
		/// </summary>
		public long ObjectId { get; set; }

		/// <summary>
		///    Gets or sets the I Work View Object Post Request Application Name
		/// </summary>
		public string ApplicationName { get; set; }

		/// <summary>
		///    Gets or sets the I Work View Object Post Request Class Name
		/// </summary>
		public string ClassName { get; set; }

		/// <summary>
		///    Gets or sets the I Work View Object Post Request Attributes
		/// </summary>
		public IEnumerable<WorkviewAttributeRequest> Attributes { get; set; }
	}
}