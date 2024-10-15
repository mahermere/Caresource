// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   WC.Services.OnBase
//   DocumentManager.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.OnBase.Dotnet8.Managers.v1
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
	using CareSource.WC.Entities.Requests.Base;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using WC.Services.OnBase.Dotnet8.Adapters.Interfaces.v1;
    using WC.Services.OnBase.Dotnet8.Adapters.v1;
    using WC.Services.OnBase.Dotnet8.Managers.Interfaces.v1;
    using WC.Services.OnBase.Dotnet8.Models.v1;

    public partial class WorkViewManager : IWorkViewManager
	{
		private readonly IWorkViewAdapter _wvadapter;
		private readonly log4net.ILog _logger;

		public WorkViewManager(
			IWorkViewAdapter wvadapter,
			log4net.ILog logger)
		{
			_wvadapter = wvadapter;
			_logger = logger;
		}

        public bool ValidateRequest(
			BaseRequest request,
			ModelStateDictionary modelState)
		{
			_logger.Info("Validating Model State");

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