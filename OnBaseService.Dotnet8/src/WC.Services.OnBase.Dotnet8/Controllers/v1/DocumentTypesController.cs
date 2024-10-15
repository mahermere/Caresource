// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   WC.Services.OnBase
//   DocumentController.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.OnBase.Dotnet8.Controllers.v1
{
	//using CareSource.WC.OnBase.Core.Diagnostics.Interfaces;
	//using CareSource.WC.OnBase.Core.Http.Filters;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using WC.Services.OnBase.Dotnet8.Managers.Interfaces.v1;

    /// <summary>
    ///    Version 1 of the OnBase Controller
    /// </summary>
    [Authorize(Policy = "OnBaseAuthorization")]
    [Route("api/documenttypes")]
	public partial class DocumentTypesController : OnBaseControllerBase
	{
		private readonly IDocumentManager _documentManager;
		private log4net.ILog _logger;

		/// <summary>
		/// </summary>
		/// <param name="documentManager"></param>
		/// <param name="logger"></param>
		public DocumentTypesController(IDocumentManager documentManager,log4net.ILog logger): base(logger)
		{
            _logger = logger;
            _documentManager = documentManager;
        }
	}
}