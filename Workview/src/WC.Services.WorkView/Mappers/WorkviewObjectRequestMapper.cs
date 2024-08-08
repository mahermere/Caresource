// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   Workview
//   WorkviewObjectRequestMapper.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.WorkView.Mappers
{
	using System.Linq;
	using CareSource.WC.Entities.WorkView;

	public class WorkviewObjectRequestMapper : IModelMapper<WorkviewObject, WorkviewObjectRequest>
	{
		public WorkviewObjectRequest GetMappedModel(WorkviewObject original)
		{
			if (original == null)
			{
				return null;
			}

			return new WorkviewObjectRequest
			{
				ApplicationName = original.ApplicationName,
				ClassName = original.ClassName,
				Attributes = original.Attributes?
					.Select(
						a => new WorkviewAttributeRequest
						{
							Name = a.Name,
							Value = a.Value
						})
					.ToList()
			};
		}

		public WorkviewObject GetMappedModel(WorkviewObjectRequest original)
		{
			if (original == null)
			{
				return null;
			}

			return new WorkviewObject
			{
				ApplicationName = original.ApplicationName,
				ClassName = original.ClassName,
				Attributes = original.Attributes?
					.Select(
						a => new WorkviewAttribute
						{
							Name = a.Name,
							Value = a.Value
						})
					.ToList()
			};
		}
	}
}