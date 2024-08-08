// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   Workview
//   WorkviewApplicationManager.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.WorkView.Managers.v2
{
	using System.Collections.Generic;
	using System.Web.Http.ModelBinding;
	using CareSource.WC.Entities.Workview.v2;
	using CareSource.WC.Services.WorkView.Adapters.v2;
	using CareSource.WC.Services.WorkView.Mappers.v2;

	public class WorkViewApplicationManager : IWorkViewApplicationManager
	{
		private readonly IModelMapper<WorkviewObject, WorkviewObjectGetRequest> _modelGetMapper;

		private readonly IModelMapper<IEnumerable<WorkviewObject>, WorkviewObjectBatchRequest>
			_modelPostMapper;

		private readonly IWorkviewObjectAdapter<WorkviewObject> _workviewObjectAdapter;

		public WorkViewApplicationManager(
			IModelMapper<WorkviewObject, WorkviewObjectGetRequest> modelGetMapper,
			IModelMapper<IEnumerable<WorkviewObject>, WorkviewObjectBatchRequest> modelPostMapper,
			IWorkviewObjectAdapter<WorkviewObject> workviewObjectAdapter)
		{
			_modelGetMapper = modelGetMapper;
			_modelPostMapper = modelPostMapper;
			_workviewObjectAdapter = workviewObjectAdapter;
		}

		public IEnumerable<WorkviewObject> CreateNewObjects(WorkviewObjectBatchRequest request)
		{
			IEnumerable<WorkviewObject> mappedModel = _modelPostMapper.GetMappedModel(request);

			return _workviewObjectAdapter.CreateNewObjects(mappedModel)
				.Result;
		}

		public IEnumerable<WorkviewObject> FindWorkviewObjects(WorkviewObjectGetRequest request)
		{
			WorkviewObject mappedModel = _modelGetMapper.GetMappedModel(request);
			return _workviewObjectAdapter.SearchWorkviewObjects(mappedModel);
		}

		public WorkviewObject GetWorkviewObject(
			long id,
			WorkviewObjectGetRequest request)
		{
			WorkviewObject mappedModel = _modelGetMapper.GetMappedModel(request);
			mappedModel.Id = id;

			return _workviewObjectAdapter.GetWorkviewObject(mappedModel);
		}

		public bool ValidateRequest(
			WorkviewObjectGetRequest request,
			ModelStateDictionary modelState)
		{
			return modelState.IsValid;
		}


		public bool ValidateRequest(
			WorkviewObjectBatchRequest request,
			ModelStateDictionary modelState)
		{
			return modelState.IsValid;
		}
	}
}