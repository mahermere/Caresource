// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   WC.Services.OnBase
//   DocumentManager.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.OnBase.Managers.v1
{
	using System.ComponentModel.DataAnnotations;
	using System.Web.Http.ModelBinding;
	using CareSource.WC.Entities.Requests.Base;
	using CareSource.WC.OnBase.Core.Diagnostics.Interfaces;
	using CareSource.WC.Services.OnBase.Adapters.v1;
	using CareSource.WC.Services.OnBase.Managers.Interfaces.v1;

	public partial class WorkViewManager : IWorkViewManager
	{
		private readonly WorkViewAdapter _wvadapter;
		private readonly ILogger _logger;

		public WorkViewManager(
			WorkViewAdapter wvadapter,
			ILogger logger)
		{
			_wvadapter = wvadapter;
			_logger = logger;
		}

		public bool ValidateRequest(
			BaseRequest request,
			ModelStateDictionary modelState)
		{
			_logger.LogInfo("Validating Model State");

			bool isValid = modelState.IsValid;

			if (!isValid)
			{
				throw new ValidationException(
					"Error Validating Request",
					null,
					modelState);
			}

			return isValid;
		}

	}
}