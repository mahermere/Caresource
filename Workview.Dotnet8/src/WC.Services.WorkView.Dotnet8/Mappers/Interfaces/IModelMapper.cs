// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   Workview
//   IModelMapper.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.WorkView.Dotnet8.Mappers.Interfaces
{
	public interface IModelMapper<TMappedModel1, TMappedModel2>
	{
		TMappedModel2 GetMappedModel(TMappedModel1 original);

		TMappedModel1 GetMappedModel(TMappedModel2 original);
	}
}