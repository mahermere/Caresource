// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2022. All rights reserved.
// 
//   WC.Services.Document
//   ProviderController.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.Document.Controllers.v5
{
	using System.Web.Http;
	using CareSource.WC.OnBase.Core.Http.Filters;
	using CareSource.WC.Services.Document.Managers.v5;
	using Microsoft.Extensions.Logging;
	using Microsoft.Web.Http;

	/// <summary>
	///    Version 4 of the Document Controller
	/// </summary>
	[OnBaseAuthorizeFilter]
	[ApiVersion("5")]
	[RoutePrefix("api/v{version:apiVersion}/provider")]
	public partial class ProviderController : ApiController
	{
		/// <inheritdoc />
		/// <summary>
		///    Initializes a new instance of the
		///    <see cref="T:CareSource.WC.Services.Document.Controllers.v4.DocumentController" /> class.
		/// </summary>
		/// <param name="providerManager"></param>
		/// <param name="logger"></param>
		public ProviderController(
			IProviderManager providerManager,
			ILogger logger)
		{
			_logger = logger;
			_providerManager = providerManager;
		}

		/// <summary>
		///    Success Message Document Controller option
		/// </summary>
		private const string SuccessMessage = "Success";

		private readonly ILogger _logger;
		private readonly IProviderManager _providerManager;
	}
}