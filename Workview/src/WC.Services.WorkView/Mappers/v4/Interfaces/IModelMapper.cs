// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   Workview
//   IModelMapper.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.WorkView.Mappers.v4
{
	public interface IModelMapper<TMappedModel1, TMappedModel2>
	{
		TMappedModel2 GetMappedModel(TMappedModel1 original);

		TMappedModel1 GetMappedModel(TMappedModel2 original);
	}
}