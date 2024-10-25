// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2022. All rights reserved.
// 
//   WC.Services.Document
//   DocumentController.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Service.Document.Dotnet8.Controllers
{
	using System.Net;
    using Microsoft.AspNetCore.Authorization;
    //using System.Web.Http;
    using Microsoft.AspNetCore.Mvc;
    //using CareSource.WC.OnBase.Core.Http.Filters;
    using Microsoft.Extensions.Logging;
	//using Microsoft.Web.Http;
    using Swashbuckle.AspNetCore.Annotations;
    using WC.Services.Document.Dotnet8.Managers.v6;
    using WC.Services.Document.Dotnet8.Managers.v6.Interfaces;
    using WC.Services.Document.Dotnet8.Models.v6;
    using WC.Services.Document.Dotnet8.Models.v6.Responses;


    /// <summary>
    ///    Version 6 of the Document Controller
    /// </summary>
    [Authorize(Policy = "OnBaseAuthorization")]
    [SwaggerResponse(
		(int)HttpStatusCode.Unauthorized,
		"When unable to authenticate using the basic Authentication.",
		typeof(DocumentTypesCountResponse))]
	[SwaggerResponse(
        (int)HttpStatusCode.BadRequest,
		"When there is any validation error or json parsing error.",
		typeof(ValidationProblemResponse))]
	[SwaggerResponse(
        (int)HttpStatusCode.InternalServerError,
		"When there is any unhandled exception.",
		typeof(ValidationProblemResponse))]
	[Route("api/[controller]")]
    [ApiController]
    public partial class DocumentController : ControllerBase
	{
        private readonly log4net.ILog _logger;
        /// <inheritdoc />
        /// <summary>
        ///    Initializes a new instance of the
        ///    <see cref="T:CareSource.WC.Services.Document.Controllers.v6.DocumentController" /> class.
        /// </summary>
        /// <param name="documentManager"></param>
        /// <param name="createDocumentManager"></param>
        /// <param name="logger"></param>
        public DocumentController(
			IDocumentManager documentManager,
			ICreateDocumentManager<OnBaseDocument> createDocumentManager,
            log4net.ILog logger)
		{
            _logger = logger;
            _createDocumentManager = createDocumentManager;
			_documentManager = documentManager;
		}

		/// <summary>
		///    Success Message Document Controller option
		/// </summary>
		private const string SuccessMessage = "Success";

		private readonly ICreateDocumentManager<OnBaseDocument> _createDocumentManager;

		/// <summary>
		///    Gets the document manager of the document controller class.
		/// </summary>
		private readonly IDocumentManager _documentManager;

		
	}
}