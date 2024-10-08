// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2021. All rights reserved.
// 
//   ImportProcessor
//   IWorkViewAdapter.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.ImportProcessor.Dotnet8.Adapters.v1.Interfaces
{
	using System;
	using System.Collections.Generic;
    using CareSource.WC.Entities.Workview.v2;
    using WC.Services.ImportProcessor.Dotnet8.Models.v1;


	public interface IWorkViewAdapter : IDisposable
	{
		long CreateDocumentObject(List<WorkviewAttributeRequest> attributes);

		long CreateClaimsObjects(
			long objectId,
			Claim[] claim);
	}
}