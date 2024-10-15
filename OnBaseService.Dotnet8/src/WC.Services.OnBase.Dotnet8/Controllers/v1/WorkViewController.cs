// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   WC.Services.OnBase
//   WorkViewController.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WC.Services.OnBase.Dotnet8.Managers.Interfaces.v1;

namespace WC.Services.OnBase.Dotnet8.Controllers.v1
{
	

	/// <summary>
	///    Version 1 of the OnBase Controller
	/// </summary>
	[Authorize(Policy = "OnBaseAuthorization")]
	[Route("api/workview/applications")]
	public partial class WorkViewController : OnBaseControllerBase
	{
		private readonly IWorkViewManager _workviewManager;
		private log4net.ILog _logger;
		/// <summary>
		///    Initializes a new instance of the
		///    <see cref="T:CareSource.WC.Services.OnBase.Controllers.OnBaseControllerBase" /> class.
		/// </summary>
		/// <param name="workviewManger"></param>
		/// <param name="logger"></param>
		public WorkViewController(IWorkViewManager workviewManger, log4net.ILog logger) : base(logger)
		{ 
            _logger = logger;
			_workviewManager = workviewManger; 
		}
	}
}
