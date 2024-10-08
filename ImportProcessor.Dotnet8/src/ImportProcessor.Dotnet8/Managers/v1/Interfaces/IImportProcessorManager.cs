// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2021. All rights reserved.
// 
//   ImportProcessor
//   IImportProcessorManager.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.ImportProcessor.Dotnet8.Managers.v1.Interfaces
{
	using System;

	public interface IImportProcessorManager<out TDataModel>
	{
		TDataModel CreateOnBaseObjects(
			string documentType,
			Guid correlationGuid);
	}
}