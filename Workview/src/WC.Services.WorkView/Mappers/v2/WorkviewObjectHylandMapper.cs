// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   Workview
//   WorkviewObjectHylandMapper.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.WorkView.Mappers.v2
{
	using System;
	using System.Linq;
	using CareSource.WC.Entities.Workview.v2;
	using Hyland.Unity.WorkView;
	using Object = Hyland.Unity.WorkView.Object;

	public class WorkviewObjectHylandMapper
		: IModelMapper<WorkviewObject, System.Tuple<Application, Object>>
	{
		public System.Tuple<Application, Object> GetMappedModel(WorkviewObject original)
		{
			throw new NotImplementedException();
		}

		public WorkviewObject GetMappedModel(System.Tuple<Application, Object> original)
		{
			if (original?.Item1 == null &&
			    original?.Item2 == null)
			{
				return null;
			}

			return new WorkviewObject
			{
				ApplicationId = original.Item1.ID,
				ApplicationName = original.Item1.Name,
				ClassId = original.Item2.Class.ID,
				ClassName = original.Item2.Class.Name,
				Id = original.Item2.ID,
				Name = original.Item2.Name,
				CreatedBy = original.Item2.CreatedBy?.RealName,
				CreatedDate = original.Item2.CreatedDate,
				RevisionBy = original.Item2.RevisionBy?.RealName,
				RevisionDate = original.Item2.RevisionDate,
				Attributes = original.Item2.AttributeValues
					.Select(
						a => new WorkviewAttribute
						{
							Name = a.Name,
							Value = a.ToString()
						})
					.ToList()
			};
		}
	}
}