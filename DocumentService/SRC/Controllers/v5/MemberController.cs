// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2022. All rights reserved.
// 
//   WC.Services.Document
//   MemberController.cs
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
	///    Version 5 of the Document Controller
	/// </summary>
	[OnBaseAuthorizeFilter]
	[ApiVersion("5")]
	[RoutePrefix("api/v{version:apiVersion}/member/{memberId}")]
	public partial class MemberController : ApiController
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
			ILogger logger)
		{
			_logger = logger;

			_memberManager = memberManager;
		}

		/// <summary>
		///    Success Message Document Controller option
		/// </summary>
		private const string SuccessMessage = "Success";

		private readonly ILogger _logger;
		private readonly IMemberManager _memberManager;
	}
}