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

	public partial class DocumentManager : IDocumentManager
	{
		private readonly DocumentAdapter _adapter;
		private readonly ILogger _logger;

		public DocumentManager(
			DocumentAdapter adapter,
			ILogger logger)
		{
			_adapter = adapter;
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