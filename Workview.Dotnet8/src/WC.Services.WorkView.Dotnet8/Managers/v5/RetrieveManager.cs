// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2022. All rights reserved.
// 
//   Workview
//   RetrieveManager.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.WorkView.Dotnet8.Managers.v5
{
	using WC.Services.WorkView.Dotnet8.Adapters.v5;
	using WC.Services.WorkView.Dotnet8.Mappers.v5;
	using WC.Services.WorkView.Dotnet8.Models.v5;
	using Microsoft.Extensions.Logging;
    using WC.Services.WorkView.Dotnet8.Mappers.v5.Interfaces;

    //using CareSource.WC.OnBase.Core.ExtensionMethods;

    public class RetrieveManager : IRetrieveManager
	{
		private readonly IModelMapper<WorkViewObject, WorkViewBaseObject> _modelMapper;
		private readonly IRetrieveAdapter _retrieveAdapter;
		private readonly log4net.ILog _logger;

		public RetrieveManager(
			IModelMapper<WorkViewObject, WorkViewBaseObject> modelMapper,
			IRetrieveAdapter retrieveAdapter,
			log4net.ILog logger)
		{
			_modelMapper = modelMapper;
			_retrieveAdapter = retrieveAdapter;
			_logger = logger;
		}

		public WorkViewObject GetWorkviewObject(
			string workViewApplicationName,
			string className,
			long objectId)
		{
			_logger.Info(
				$"{nameof(RetrieveManager)}.{nameof(GetWorkviewObject)} call, " +
				$"to get{workViewApplicationName}.{className} Object Id: {objectId}.");

			return _retrieveAdapter.GetWorkviewObject(
				workViewApplicationName,
				className,
				objectId);
		}
	}
}