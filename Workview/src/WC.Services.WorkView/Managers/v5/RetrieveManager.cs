// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2022. All rights reserved.
// 
//   Workview
//   RetrieveManager.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.WorkView.Managers.v5
{
	using CareSource.WC.Services.WorkView.Adapters.v5;
	using CareSource.WC.Services.WorkView.Mappers.v5;
	using CareSource.WC.Services.WorkView.Models.v5;
	using Microsoft.Extensions.Logging;
	using CareSource.WC.OnBase.Core.ExtensionMethods;

	public class RetrieveManager : IRetrieveManager
	{
		private readonly IModelMapper<WorkViewObject, WorkViewBaseObject> _modelMapper;
		private readonly IRetrieveAdapter _retrieveAdapter;
		private readonly ILogger _logger;

		public RetrieveManager(
			IModelMapper<WorkViewObject, WorkViewBaseObject> modelMapper,
			IRetrieveAdapter retrieveAdapter,
			ILogger logger)
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
			_logger.LogInformation(
				$"{nameof(RetrieveManager)}.{nameof(GetWorkviewObject)} call, " +
				$"to get{workViewApplicationName}.{className} Object Id: {objectId}.");

			return _retrieveAdapter.GetWorkviewObject(
				workViewApplicationName,
				className,
				objectId);
		}
	}
}