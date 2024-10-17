// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   Workview
//   IWorkviewApplicationManager.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.WorkView.Dotnet8.Managers.v5
{
	using WC.Services.WorkView.Dotnet8.Models.v5;

	public interface IRetrieveManager
	{
		WorkViewObject GetWorkviewObject(
			string workViewApplicationName,
			string className,
			long objectId);
	}
}