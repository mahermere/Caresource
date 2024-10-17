// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   Workview
//   WorkviewApplicationManager.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.WorkView.Dotnet8.Managers.v5
{
	using System.Collections.Generic;
	using WC.Services.WorkView.Dotnet8.Adapters.v5;
	using WC.Services.WorkView.Dotnet8.Mappers.v5;
	using WC.Services.WorkView.Dotnet8.Models.v5;
	using Microsoft.Extensions.Logging;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using WC.Services.WorkView.Dotnet8.Mappers.v5.Interfaces;

    //using CareSource.WC.OnBase.Core.ExtensionMethods;

    public class CreateManager : ICreateManager
	{
		private readonly ICreateAdapter _createAdapter;
		private log4net.ILog _logger;
		private readonly IModelMapper<WorkViewObject, WorkViewBaseObject> _modelMapper;

		public CreateManager(
			IModelMapper<WorkViewObject, WorkViewBaseObject> modelMapper,
			ICreateAdapter createAdapter,
			log4net.ILog logger)
		{
			_modelMapper = modelMapper;
			_createAdapter = createAdapter;
			_logger = logger;
		}

		public bool ValidateRequest(
			string workviewApplicationName,
			CreateRequest request,
			ModelStateDictionary modelState)
		{
			//request.TrimAllStrings();

			(bool isValid, Dictionary<string, string[]> errors) =
				_createAdapter.ValidateRequest(
				workviewApplicationName,
				request);

			if (isValid)
			{
				_logger.Info("Model State is Valid.");
			}
			else
			{
				_logger.Error(
					"Error Validating Model State.");

				foreach (KeyValuePair<string, string[]> error in errors)
				{
					modelState.AddModelError(
						error.Key,
						string.Join(
							",",
							error.Value));
				}
			}

			return modelState.IsValid;
		}

		public IEnumerable<WorkViewObject> CreateNewObject(
			string workviewApplicationName,
			CreateRequest request) =>
			_createAdapter.CreateObjects(request.Data);
	}
}