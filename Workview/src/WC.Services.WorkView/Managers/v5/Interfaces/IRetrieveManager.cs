// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   Workview
//   IWorkviewApplicationManager.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.WorkView.Managers.v5
{
	using CareSource.WC.Services.WorkView.Models.v5;

	public interface IRetrieveManager
	{
		WorkViewObject GetWorkviewObject(
			string workViewApplicationName,
			string className,
			long objectId);
	}
}