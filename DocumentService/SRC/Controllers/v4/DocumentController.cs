// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   WC.Services.Document
//   DocumentController.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.Document.Controllers.v4
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.Web.Http;
	using CareSource.WC.Entities.Documents;
	using CareSource.WC.Entities.Requests.Base;
	using Microsoft.Extensions.Logging;
	using CareSource.WC.OnBase.Core.ExtensionMethods;
	using CareSource.WC.OnBase.Core.Http.Filters;
	using CareSource.WC.Services.Document.Managers.v4;
	using Microsoft.Web.Http;

	/// <summary>
	///    Version 4 of the Document Controller
	/// </summary>
	[OnBaseAuthorizeFilter]
	[ApiVersion("4", Deprecated = true)]
	[RoutePrefix("api/v{version:apiVersion}")]
	[Obsolete("Please us V5 Routes")]
	public partial class DocumentController : ApiController
	{
		/// <summary>
		///    Success Message Document Controller option
		/// </summary>
		private const string SuccessMessage = "Success";

		/// <summary>
		///    Gets the document manager of the document controller class.
		/// </summary>
		private readonly IDocumentManager<DocumentHeader> _documentManager;

		private readonly ILogger _logger;
		private readonly IGetDocumentManager<Models.v4.Document> _getDocumentManager;

		/// <inheritdoc />
		/// <summary>
		///    Initializes a new instance of the
		///    <see cref="T:CareSource.WC.Services.Document.Controllers.v4.DocumentController" /> class.
		/// </summary>
		/// <param name="documentManager"></param>
		/// <param name="getDocumentManager"></param>
		/// <param name="logger"></param>
		public DocumentController(
			IDocumentManager<DocumentHeader> documentManager,
			IGetDocumentManager<Models.v4.Document> getDocumentManager,
			ILogger logger)
		{
			_logger = logger;

			_getDocumentManager = getDocumentManager;
			_documentManager = documentManager;
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