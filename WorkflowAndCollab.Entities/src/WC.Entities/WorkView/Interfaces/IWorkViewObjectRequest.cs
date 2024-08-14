// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   Entities
//   IWorkViewObjectRequest.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Entities.WorkView.Interfaces
{
	using System.Collections.Generic;

	internal interface IWorkViewObjectRequest
	{
		long ObjectId { get; set; }

		string ApplicationName { get; set; }

		string ClassName { get; set; }

		List<WorkviewAttributeRequest> Attributes { get; set; }
	}
}