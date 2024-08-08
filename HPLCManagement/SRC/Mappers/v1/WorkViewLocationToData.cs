// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2023. All rights reserved.
// 
//   WC.Services.HplcManagement
//   WorkViewLocationToData.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace HplcManagement.Mappers.v1
{
	using System.Collections.Generic;
	using System.Linq;
	using HplcManagement.Models.v1;
	using Hyland.Unity.WorkView;

	public class WorkViewLocationToData : BaseDataObjectModelMapper
	{
		public override void PopulateRelated(Data data)
		{
			DependentObjectList dependentObjects = GetDependentObjects();

			WorkViewPhoneToData phoneMapper = new WorkViewPhoneToData();

			if (dependentObjects.SafeAny())
			{
				List<Data> listDependentObjects = new List<Data>();

				listDependentObjects.AddRange(
					from o in dependentObjects
					where o.Class.Name.Equals(Constants.Phone.ClassName)
					select phoneMapper.GetMappedModel(o.Class.GetObjectByID(o.ID)));

				data.Related = listDependentObjects;
			}
		}
	}
}