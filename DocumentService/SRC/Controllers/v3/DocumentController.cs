// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   WC.Services.Document
//   DocumentController.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.Document.Controllers.v3
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.Web.Http;
	using CareSource.WC.Entities.Documents;
	using CareSource.WC.Entities.Requests.Base;
	using Microsoft.Extensions.Logging;
	using CareSource.WC.OnBase.Core.ExtensionMethods;
	using CareSource.WC.OnBase.Core.Http.Filters;
	using CareSource.WC.Services.Document.Managers.v3;
	using CareSource.WC.Services.Document.Models.v3;
	using Microsoft.Web.Http;

	/// <summary>
	///    Version 3 of the Document Controller
	/// </summary>
	[OnBaseAuthorizeFilter]
	[ApiVersion("3", Deprecated = true)]
	[RoutePrefix("api/v{version:apiVersion}")]
	public partial class DocumentController : ApiController
	{
		/// <inheritdoc />
		/// <summary>
		///    Initializes a new instance of the
		///    <see cref="T:CareSource.WC.Services.Document.Controllers.v1.DocumentController" /> class.
		/// </summary>
		/// <param name="documentManager"></param>
		/// <param name="getDocumentManager"></param>
		/// <param name="createDocumentManager"></param>
		/// <param name="keywordManager"></param>
		/// <param name="logger"></param>
		public DocumentController(
			IDocumentManager<DocumentHeader> documentManager,
			IGetDocumentManager<Document> getDocumentManager,
			ICreateDocumentManager<OnBaseDocument> createDocumentManager,
			IKeywordManager keywordManager,
			ILogger logger)
		{
			_logger = logger;
			_getDocumentManager = getDocumentManager;
			_documentManager = documentManager;
			_keywordManager = keywordManager;
			_createDocumentManager = createDocumentManager;
		}

		private readonly ILogger _logger;

		private bool ValidateModelState()
		{
			if (ModelState.IsValid)
			{
				_logger.LogInformation("Successfully validated model state!");
				return true;
			}

			_logger.LogError("Invalid model state!", new ValidationException());

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

		private bool VerifyRequest(IFilteredRequest request)
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

		private readonly IKeywordManager _keywordManager;

		/// <summary>
		///    Gets the document manager of the document controller class.
		/// </summary>
		private readonly IDocumentManager<DocumentHeader> _documentManager;

		/// <summary>
		///    Get Document Manager Document Controller option
		/// </summary>
		private readonly IGetDocumentManager<Document> _getDocumentManager;

		private readonly ICreateDocumentManager<OnBaseDocument> _createDocumentManager;
	}
}