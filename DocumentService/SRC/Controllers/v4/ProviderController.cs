// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   WC.Services.Document
//   ProviderController.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.Document.Controllers.v4
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.Web.Http;
	using CareSource.WC.Entities.Requests.Base;
	using Microsoft.Extensions.Logging;
	using CareSource.WC.OnBase.Core.ExtensionMethods;
	using CareSource.WC.OnBase.Core.Http.Filters;
	using CareSource.WC.Services.Document.Managers.v4;
	using Microsoft.Web.Http;
	using Document = Models.v4.Document;

	/// <summary>
	///    Version 4 of the Document Controller
	/// </summary>
	[OnBaseAuthorizeFilter]
	[ApiVersion("4", Deprecated = true)]
	[RoutePrefix("api/v{version:apiVersion}/provider")]
	[Obsolete("Please use V5 routes")]
	public partial class ProviderController : ApiController
	{
		/// <summary>
		///    Success Message Document Controller option
		/// </summary>
		private const string SuccessMessage = "Success";

		private readonly ILogger _logger;
		private readonly IProviderManager _providerManager;
		private readonly IClaimsManager<Document> _claimsManager;

		/// <inheritdoc />
		/// <summary>
		///    Initializes a new instance of the
		///    <see cref="T:CareSource.WC.Services.Document.Controllers.v4.DocumentController" /> class.
		/// </summary>
		/// <param name="providerManager"></param>
		/// <param name="claimsManager"></param>
		/// <param name="logger"></param>
		public ProviderController(
			IProviderManager providerManager,
			IClaimsManager<Document> claimsManager,
			ILogger logger)
		{
			_logger = logger;
			_claimsManager = claimsManager;
			_providerManager = providerManager;
		}

		private bool ValidateModelState()
		{
			if (ModelState.IsValid)
			{
				_logger.LogInformation("Successfully validated model state!");
				return true;
			}

			_logger.LogError(
				"Invalid model state!",
				new ValidationException());

			return false;
		}

		private bool VerifyRequest(BaseRequest request)
		{
			if (request == null)
			{
				throw new NullReferenceException("Request Data is is Null");
			}

			if (request.CorrelationGuid.Equals(Guid.Empty))
			{
				request.CorrelationGuid = Guid.NewGuid();
			}

			return ValidateModelState();
		}
	}
}