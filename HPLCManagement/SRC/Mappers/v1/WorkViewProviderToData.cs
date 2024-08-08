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

	public class WorkViewProviderToData : BaseDataObjectModelMapper
	{
		public override void PopulateRelated(Data data)
		{
			DependentObjectList dependentObjects = GetDependentObjects();
			WorkViewLocationToData locationMapper = new WorkViewLocationToData();

			if (dependentObjects.SafeAny())
			{
				List<Data> listDependentObjects = new List<Data>();

				listDependentObjects.AddRange(
					from o in dependentObjects
					where o.Class.Name.Equals(Constants.Location.ClassName)
					select locationMapper.GetMappedModel(o.Class.GetObjectByID(o.ID)));

				data.Related = listDependentObjects;
			}
		}
	}
}