// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   Workview
//   WorkviewApplicationManager.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.WorkView.Managers.v5
{
	using System.Collections.Generic;
	using System.Web.Http.ModelBinding;
	using CareSource.WC.Services.WorkView.Adapters.v5;
	using CareSource.WC.Services.WorkView.Mappers.v5;
	using CareSource.WC.Services.WorkView.Models.v5;
	using Microsoft.Extensions.Logging;
	using CareSource.WC.OnBase.Core.ExtensionMethods;

	public class CreateManager : ICreateManager
	{
		private readonly ICreateAdapter _createAdapter;
		private ILogger _logger;
		private readonly IModelMapper<WorkViewObject, WorkViewBaseObject> _modelMapper;

		public CreateManager(
			IModelMapper<WorkViewObject, WorkViewBaseObject> modelMapper,
			ICreateAdapter createAdapter,
			ILogger logger)
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
			request.TrimAllStrings();

			(bool isValid, Dictionary<string, string[]> errors) =
				_createAdapter.ValidateRequest(
				workviewApplicationName,
				request);

			if (isValid)
			{
				_logger.LogInformation("Model State is Valid.");
			}
			else
			{
				_logger.LogError(
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