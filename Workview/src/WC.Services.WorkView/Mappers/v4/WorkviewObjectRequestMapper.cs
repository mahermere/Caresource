// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   Workview
//   WorkviewObjectRequestMapper.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.WorkView.Mappers.v4
{
	using System.Linq;
	using CareSource.WC.Entities.Workview.v2;

	public class WorkviewObjectRequestMapper
		: IModelMapper<
			WorkviewObject,
			WorkviewObjectGetRequest>
	{
		public WorkviewObjectGetRequest GetMappedModel(WorkviewObject original)
		{
			if (original == null)
			{
				return null;
			}

			return new WorkviewObjectGetRequest
			{
				ApplicationName = original.ApplicationName,
				ClassName = original.ClassName,
				FilterName = original.FilterName,
				Filters = original.Filters?
					.Select(
						a => new WorkviewFilterRequest
						{
							Name = a.Name,
							Value = a.Value
						})
					.ToList(),
				Attributes = original.Attributes.Select(a => a.Name)
			};
		}

		public WorkviewObject GetMappedModel(WorkviewObjectGetRequest original)
		{
			if (original == null)
			{
				return null;
			}

			return new WorkviewObject
			{
				ApplicationName = original.ApplicationName,
				ClassName = original.ClassName,
				FilterName = original.FilterName,
				Filters = original.Filters?
					.Select(
						a => new WorkviewFilter
						{
							Name = a.Name,
							Value = a.Value
						})
					.ToList(),
				Attributes = original.Attributes?
					.Select(
						a => new WorkviewAttribute
						{
							Name = a,
							Value = string.Empty
						})
					.ToList()
			};
		}
	}
}