// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2023. All rights reserved.
// 
//   WC.Services.HplcManagement
//   WorkViewDataClassRequest.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace HplcManagement.Mappers.v1
{
	using System.Collections.Generic;
	using System.Linq;
	using HplcManagement.Models.v1;
	using Hyland.Unity.WorkView;

	public class WorkViewProductToData : BaseDataObjectModelMapper
	{
		public override Data GetMappedModel(
			Object original,
			bool includeChildren = true)
		{
			MainObject = original;
			Data wvo = base.GetMappedModel(GetRelatedObject(Constants.Products.Product));

			PopulateRelated(wvo);
			return wvo;
			
		}

		public override void PopulateRelated(Data data)
		{
			WorkViewStateToData stateMapper = new WorkViewStateToData();
			data.Related =
				new[] { stateMapper.GetMappedModel(GetRelatedObject(Constants.Products.RelationshipToState)) };

		}
	}
}
