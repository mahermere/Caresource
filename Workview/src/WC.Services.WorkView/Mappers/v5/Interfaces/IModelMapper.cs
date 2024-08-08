// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   Workview
//   IModelMapper.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.WorkView.Mappers.v5
{
	using Hyland.Unity.WorkView;

	public interface IModelMapper<TMappedModel1, TMappedModel2>
	{
		TMappedModel2 GetMappedModel(TMappedModel1 original);

		TMappedModel1 GetMappedModel(TMappedModel2 original);

		TMappedModel1 GetMappedModel(Object original, Object parent = null);
	}
}