// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2022. All rights reserved.
// 
//   Workview
//   RetrieveAdapter.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.WorkView.Adapters.v5
{
	using System;
	using System.Collections.Generic;
	using CareSource.WC.OnBase.Core.Connection.Interfaces;
	using CareSource.WC.Services.WorkView.Mappers.v5;
	using CareSource.WC.Services.WorkView.Models.v5;
	using Hyland.Unity.WorkView;
	using Application = Hyland.Unity.Application;
	using Object = Hyland.Unity.WorkView.Object;
	using Microsoft.Extensions.Logging;
	using CareSource.WC.OnBase.Core.ExtensionMethods;

	public class RetrieveAdapter : IRetrieveAdapter
	{
		private readonly IApplicationConnectionAdapter<Application> _applicationConnectionAdapter;
		private readonly IModelMapper<WorkViewObject, WorkViewBaseObject> _modelMapper;
		private readonly ILogger _logger;


		/// <summary>
		/// Initializes a new instance of the <see cref="RetrieveAdapter"/> class.
		/// </summary>
		/// <param name="modelMapper">The model mapper.</param>
		/// <param name="applicationConnectionAdapter">The application connection adapter.</param>
		/// <param name="logger">The logger.</param>
		public RetrieveAdapter(
			IModelMapper<WorkViewObject, WorkViewBaseObject> modelMapper,
			IApplicationConnectionAdapter<Application> applicationConnectionAdapter,
			ILogger logger)
		{
			_modelMapper = modelMapper;
			_applicationConnectionAdapter = applicationConnectionAdapter;
			_logger = logger;
		}

		public WorkViewObject GetWorkviewObject(
				string workViewApplicationName,
				string className,
				long objectId)
		{
			_logger.LogInformation(
				$"{nameof(RetrieveAdapter)}.{nameof(GetWorkviewObject)} call, " +
				$"to get{workViewApplicationName}.{className} Object Id: {objectId}.");
			Hyland.Unity.WorkView.Application app =
				_applicationConnectionAdapter.Application.WorkView.Applications
					.Find(workViewApplicationName);

			if (app == null)
			{
				throw new ArgumentOutOfRangeException(nameof(workViewApplicationName));
			}

			Class wvClass = app.Classes.Find(className);

			if (wvClass == null)
			{
				throw new ArgumentOutOfRangeException(nameof(className));
			}

			Object obj = wvClass.GetObjectByID(objectId);

			if (obj == null)
			{
				throw new KeyNotFoundException("No item found for the provided Id");
			}

			return _modelMapper.GetMappedModel(obj);
		}

		public IEnumerable<WorkViewObject> SearchWorkviewObjects(
			object workviewObject)
			=> throw new NotImplementedException();

		public (bool, Dictionary<string, string[]>) ValidateRequest(
			string workviewApplicationName,
			CreateRequest request)
			=> throw new NotImplementedException();
	}
}