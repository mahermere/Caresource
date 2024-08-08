// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2021. All rights reserved.
// 
//   ImportProcessor
//   IWorkViewAdapter.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace ImportProcessor.Adapters.v1.Interfaces
{
	using System;
	using System.Collections.Generic;
	using CareSource.WC.Entities.Workview.v2;
	using ImportProcessor.Models.v1;

	public interface IWorkViewAdapter : IDisposable
	{
		long CreateDocumentObject(
			List<WorkviewAttributeRequest> attributes);

		long CreateClaimsObjects(
			long objectId,
			Claim[] claim);
	}
}