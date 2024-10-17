// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2022. All rights reserved.
// 
//   Workview
//   IWorkViewApplication.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.WorkView.Dotnet8.Models.v5
{
	using System.Collections.Generic;

	public interface IWorkViewApplication
	{
		string Name { get; set; }
		IEnumerable<WorkViewObject> Related { get; set; }
	}
}