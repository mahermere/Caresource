// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2022. All rights reserved.
// 
//   WC.Services.Document
//   MemberController.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Service.Document.Dotnet8.Controllers
{
	//using System.Web.Http;
	//using CareSource.WC.OnBase.Core.Http.Filters;
	using WC.Services.Document.Dotnet8.Managers;
	using Microsoft.Extensions.Logging;
    using Microsoft.AspNetCore.Mvc;
    using WC.Services.Document.Dotnet8.Managers.v6;
    using WC.Services.Document.Dotnet8.Managers.v6.Interfaces;
    using Microsoft.AspNetCore.Authorization;

    /// <summary>
    ///    Version 6 of the Document Controller
    /// </summary>
    [Authorize(Policy = "OnBaseAuthorization")]
	[Route("api/[Controller]")]
    [ApiController]
    public partial class MemberController : ControllerBase
	{
		/// <inheritdoc />
		/// <summary>
		///    Initializes a new instance of the
		///    <see cref="T:CareSource.WC.Services.Document.Controllers.v4.DocumentController" /> class.
		/// </summary>
		/// <param name="memberManager"></param>
		/// <param name="logger"></param>
		public MemberController(
			IMemberManager memberManager,
            log4net.ILog logger)
		{
			_logger = logger;

			_memberManager = memberManager;
		}

		/// <summary>
		///    Success Message Document Controller option
		/// </summary>
		private const string SuccessMessage = "Success";

        private readonly log4net.ILog _logger;
        private readonly IMemberManager _memberManager;
	}
}