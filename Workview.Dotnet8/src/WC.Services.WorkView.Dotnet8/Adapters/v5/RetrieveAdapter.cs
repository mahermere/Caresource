// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2022. All rights reserved.
// 
//   Workview
//   RetrieveAdapter.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.WorkView.Dotnet8.Adapters.v5
{
	using System;
	using System.Collections.Generic;
	//using CareSource.WC.OnBase.Core.Connection.Interfaces;
	using WC.Services.WorkView.Dotnet8.Mappers.v5;
	using WC.Services.WorkView.Dotnet8.Models.v5;
	using Hyland.Unity.WorkView;
	using Application = Hyland.Unity.Application;
	using Object = Hyland.Unity.WorkView.Object;
	using Microsoft.Extensions.Logging;
    using WC.Services.WorkView.Dotnet8.Mappers.v5.Interfaces;
    using WC.Services.WorkView.Dotnet8.Repository;

    //using CareSource.WC.OnBase.Core.ExtensionMethods;

    public class RetrieveAdapter : IRetrieveAdapter
	{
		private readonly IRepository _repo;
		private readonly IModelMapper<WorkViewObject, WorkViewBaseObject> _modelMapper;
		private readonly log4net.ILog _logger;


		/// <summary>
		/// Initializes a new instance of the <see cref="RetrieveAdapter"/> class.
		/// </summary>
		/// <param name="modelMapper">The model mapper.</param>
		/// <param name="repo">The Repo containing application object.</param>
		/// <param name="logger">The logger.</param>
		public RetrieveAdapter(
			IModelMapper<WorkViewObject, WorkViewBaseObject> modelMapper,
			IRepository repo,
			log4net.ILog logger)
		{
			_modelMapper = modelMapper;
			_repo = repo;
			_logger = logger;
		}

		public WorkViewObject GetWorkviewObject(
				string workViewApplicationName,
				string className,
				long objectId)
		{
			_logger.Info(
				$"{nameof(RetrieveAdapter)}.{nameof(GetWorkviewObject)} call, " +
				$"to get{workViewApplicationName}.{className} Object Id: {objectId}.");
			Hyland.Unity.WorkView.Application app =
				_repo.Application.WorkView.Applications
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