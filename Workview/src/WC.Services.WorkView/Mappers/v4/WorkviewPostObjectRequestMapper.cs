// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   Workview
//   WorkviewPostObjectRequestMapper.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.WorkView.Mappers.v4
{
	using System.Collections.Generic;
	using System.Linq;
	using CareSource.WC.Entities.Workview.v2;

	public class WorkViewPostObjectRequestMapper
		: IModelMapper<
			IEnumerable<WorkviewObject>,
			WorkviewObjectBatchRequest>
	{
		public WorkviewObjectBatchRequest GetMappedModel(IEnumerable<WorkviewObject> original)
		{
			if (original == null)
			{
				return null;
			}

			return new WorkviewObjectBatchRequest
			{
				WorkviewObjects = original.Select(
					o => new WorkviewObjectPostRequest
					{
						ApplicationName = o.ApplicationName,
						ClassName = o.ClassName,
						Attributes = o.Attributes?
							.Select(
								a => new WorkviewAttributeRequest
								{
									Name = a.Name,
									Value = a.Value
								})
					})
			};
		}

		public IEnumerable<WorkviewObject> GetMappedModel(WorkviewObjectBatchRequest original)
		{
			return original?.WorkviewObjects.Select(
				wo => new WorkviewObject
				{
					ApplicationName = wo.ApplicationName,
					ClassName = wo.ClassName,

					Attributes = wo.Attributes.Select(
						a => new WorkviewAttribute
						{
							Name = a.Name,
							Value = a.Value
						})
				});
		}
	}
}