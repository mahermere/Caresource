// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   WC.Services.OnBase
//   DocumentController.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.OnBase.Controllers.v1
{
	using System.Web.Http;
	using CareSource.WC.OnBase.Core.Diagnostics.Interfaces;
	using CareSource.WC.OnBase.Core.Http.Filters;
	using CareSource.WC.Services.OnBase.Managers.Interfaces.v1;
	using Microsoft.Web.Http;

	/// <summary>
	///    Version 1 of the OnBase Controller
	/// </summary>
	[OnBaseAuthorizeFilter]
	[ApiVersion("1")]
	[RoutePrefix("api/v{version:apiVersion}/documenttypes")]
	public partial class DocumentTypesController : OnBaseControllerBase
	{
		private readonly IDocumentManager _documentManager;

		/// <summary>
		///    Initializes a new instance of the
		///    <see cref="T:CareSource.WC.Services.OnBase.Controllers.v1.OnBaseController" /> class.
		/// </summary>
		/// <param name="documentManager"></param>
		/// <param name="logger"></param>
		public DocumentTypesController(
			IDocumentManager documentManager,
			ILogger logger): base(logger)
			=> _documentManager = documentManager;
	}
}