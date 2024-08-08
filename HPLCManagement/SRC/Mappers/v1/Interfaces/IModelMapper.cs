// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2021. All rights reserved.
// 
//   WC.Services.Hplc
//   IModelMapper.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace HplcManagement.Mappers.v1.Interfaces
{
	using HplcManagement.Models.v1;
	using Hyland.Unity.WorkView;

	public interface IModelMapper<TMappedModel1, TMappedModel2>
	{
		TMappedModel2 GetMappedModel(TMappedModel1 original);

		TMappedModel1 GetMappedModel(
			TMappedModel2 original,
			bool includeChildren = true);

		void PopulateRelated(TMappedModel1 original);
	}
}