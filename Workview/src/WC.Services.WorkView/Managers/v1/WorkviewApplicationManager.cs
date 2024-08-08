// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   Workview
//   WorkviewApplicationManager.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.WorkView.Managers.v1
{
	using System.Collections.Generic;
	using System.Web.Http.ModelBinding;
	using CareSource.WC.Entities.WorkView;
	using CareSource.WC.Services.WorkView.Adapters;
	using CareSource.WC.Services.WorkView.Mappers;

	public class WorkViewApplicationManager : IWorkViewApplicationManager
	{
		private readonly IModelMapper<WorkviewObject, WorkviewObjectRequest> _modelMapper;
		private readonly IWorkviewObjectAdapter<WorkviewObject> _WorkviewObjectAdapter;

		public WorkViewApplicationManager(
			IModelMapper<WorkviewObject, WorkviewObjectRequest> modelMapper,
			IWorkviewObjectAdapter<WorkviewObject> WorkviewObjectAdapter)
		{
			_modelMapper = modelMapper;
			_WorkviewObjectAdapter = WorkviewObjectAdapter;
		}

		public WorkviewObject CreateNewObject(WorkviewObjectRequest request)
		{
			WorkviewObject mappedModel = _modelMapper.GetMappedModel(request);

			return _WorkviewObjectAdapter.CreateNewObject(mappedModel);
		}

		public IEnumerable<WorkviewObject> FindWorkviewObjects(WorkviewObjectRequest request)
		{
			WorkviewObject mappedModel = _modelMapper.GetMappedModel(request);
			return _WorkviewObjectAdapter.SearchWorkviewObjects(mappedModel);
		}

		public WorkviewObject GetWorkviewObject(
			long id,
			WorkviewObjectRequest request)
		{
			WorkviewObject mappedModel = _modelMapper.GetMappedModel(request);
			mappedModel.Id = id;

			return _WorkviewObjectAdapter.GetWorkviewObject(mappedModel);
		}

		public bool ValidateRequest(
			WorkviewObjectRequest request,
			ModelStateDictionary modelState)
		{
			return modelState.IsValid;
		}
	}
}