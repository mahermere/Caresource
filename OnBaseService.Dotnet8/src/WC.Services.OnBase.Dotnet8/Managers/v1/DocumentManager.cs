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

    public partial class DocumentManager : IDocumentManager
	{
		private readonly IDocumentAdapter _adapter;
		private readonly log4net.ILog _logger;

		public DocumentManager(
			IDocumentAdapter adapter,
			log4net.ILog logger)
		{
			_adapter = adapter;
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