// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2022. All rights reserved.
// 
//   WC.Services.Document
//   DocumentController.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.Document.Controllers.v5
{
	using System.Net;
	using System.Web.Http;
	using CareSource.WC.OnBase.Core.Http.Filters;
	using CareSource.WC.Services.Document.Managers.v5;
	using CareSource.WC.Services.Document.Models.v5;
	using Microsoft.Extensions.Logging;
	using Microsoft.Web.Http;
	using Swashbuckle.Swagger.Annotations;

	/// <summary>
	///    Version 4 of the Document Controller
	/// </summary>
	[OnBaseAuthorizeFilter]
	[ApiVersion("5")]
	[RoutePrefix("api/v{version:apiVersion}")]
	[SwaggerResponse(
		HttpStatusCode.Unauthorized,
		"When unable to authenticate using the basic Authentication.",
		typeof(DocumentTypesCountResponse))]
	[SwaggerResponse(
		HttpStatusCode.BadRequest,
		"When there is any validation error or json parsing error.",
		typeof(ValidationProblemResponse))]
	[SwaggerResponse(
		HttpStatusCode.InternalServerError,
		"When there is any unhandled exception.",
		typeof(ValidationProblemResponse))]
	public partial class DocumentController : ApiController
	{
		/// <inheritdoc />
		/// <summary>
		///    Initializes a new instance of the
		///    <see cref="T:CareSource.WC.Services.Document.Controllers.v5.DocumentController" /> class.
		/// </summary>
		/// <param name="documentManager"></param>
		/// <param name="createDocumentManager"></param>
		/// <param name="logger"></param>
		public DocumentController(
			IDocumentManager documentManager,
			ICreateDocumentManager<OnBaseDocument> createDocumentManager,
			ILogger logger)
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

		private readonly ILogger _logger;
	}
}