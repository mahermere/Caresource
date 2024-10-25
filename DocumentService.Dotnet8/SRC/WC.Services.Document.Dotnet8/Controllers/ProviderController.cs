// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2022. All rights reserved.
// 
//   WC.Services.Document
//   ProviderController.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Service.Document.Dotnet8.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    //using System.Web.Http;
    //using CareSource.WC.OnBase.Core.Http.Filters;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using WC.Services.Document.Dotnet8.Managers.v6;
    using WC.Services.Document.Dotnet8.Managers.v6.Interfaces;


    //using Microsoft.Web.Http;

    /// <summary>
    ///    Version 6 of the Document Controller
    /// </summary>
    [Authorize(Policy = "OnBaseAuthorization")]
	[Route("api/[controller]")]
    [ApiController]
    public partial class ProviderController : ControllerBase
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
            log4net.ILog logger)
		{
			_logger = logger;
			_providerManager = providerManager;
		}

		/// <summary>
		///    Success Message Document Controller option
		/// </summary>
		private const string SuccessMessage = "Success";

        private readonly log4net.ILog _logger;
        private readonly IProviderManager _providerManager;
	}
}