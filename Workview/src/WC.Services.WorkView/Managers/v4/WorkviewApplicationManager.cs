// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   Workview
//   WorkviewApplicationManager.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.WorkView.Managers.v4
{
	using System;
	using System.Collections.Generic;
	using System.Web.Http.ModelBinding;
	using CareSource.WC.Entities.Workview.v2;
	using CareSource.WC.Services.WorkView.Adapters.v4;
	using CareSource.WC.Services.WorkView.Mappers.v4;
	using Microsoft.Extensions.Logging;

	public class WorkViewApplicationManager : IWorkViewApplicationManager
	{
		private readonly IModelMapper<WorkviewObject, WorkviewObjectGetRequest> _modelGetMapper;

		private readonly IModelMapper<IEnumerable<WorkviewObject>, WorkviewObjectBatchRequest>
			_modelPostMapper;

		private readonly IWorkViewObjectAdapter<WorkviewObject> _workviewObjectAdapter;
		private ILogger _logger;

		public WorkViewApplicationManager(
			IModelMapper<WorkviewObject, WorkviewObjectGetRequest> modelGetMapper,
			IModelMapper<IEnumerable<WorkviewObject>, WorkviewObjectBatchRequest> modelPostMapper,
			IWorkViewObjectAdapter<WorkviewObject> workviewObjectAdapter,
			ILogger logger)
		{
			_modelGetMapper = modelGetMapper;
			_modelPostMapper = modelPostMapper;
			_workviewObjectAdapter = workviewObjectAdapter;
			_logger = logger;
		}

		public bool ValidateRequest(WorkviewObjectPostRequest request, ModelStateDictionary modelState)
			=> modelState.IsValid;

		public IEnumerable<WorkviewObject> CreateNewObjects(WorkviewObjectBatchRequest request)
		{
			try
			{
				IEnumerable<WorkviewObject> mappedModel = _modelPostMapper.GetMappedModel(request);

				return _workviewObjectAdapter.CreateNewObjects(mappedModel)
					.Result;
			}
			catch (Exception ex)
			{
				string message = ex.InnerException?.Message ?? ex.Message;
				_logger.LogError(message, ex);

				if (ex is KeyNotFoundException ||
				    ex is ArgumentException ||
				    ex.InnerException is KeyNotFoundException ||
				    ex.InnerException is ArgumentException)
				{
					throw new ArgumentException(message);
				}

				throw;
			}
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

		public WorkviewObject UpdateWorkviewObject(WorkviewObjectPostRequest request)
		{
			return _workviewObjectAdapter.UpdateObject(request);
		}

		public WorkviewObject CreateNewObject(WorkviewObjectPostRequest request)
		{
			return _workviewObjectAdapter.CreateObject(request);
		}

		public bool ValidateRequest(
			WorkviewObjectGetRequest request,
			ModelStateDictionary modelState)
			=> modelState.IsValid;

		public bool ValidateRequest(
			WorkviewObjectBatchRequest request,
			ModelStateDictionary modelState)
			=> modelState.IsValid;
	}
}