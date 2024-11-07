// ------------------------------------------------------------------------------------------------
//  <copyright>
//    Copyright (c) CareSource, 2020-2022.  All rights reserved.
// 
//    FXIAuthentication
//    FXIController.cs
//  </copyright>
//  ------------------------------------------------------------------------------------------------

namespace FXIAuthentication.Controllers.v1;

using FXIAuthentication.Managers.v1;
using FXIAuthentication.Models.v1;
using Microsoft.AspNetCore.Mvc;
using FXIAuthentication.Core.Extensions;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion("1")]
public class FxiController : ControllerBase
{
	private readonly ILogger<FxiController> _logger;
	private readonly IAuthManager _manager;

	public FxiController(
		ILogger<FxiController> logger,
		IAuthManager manager)
	{
		_logger = logger;
		_manager = manager;
	}

	[HttpGet]
	public IActionResult GetFxiAuthentication(
		[FromQuery] string userName)
	{
		Guid correlationGuid = Guid.NewGuid();

		_logger.LogInformation(
			$"Starting {nameof(FxiController)}.{nameof(GetFxiAuthentication)}",
			new Dictionary<string, object>()
			{
				{ nameof(correlationGuid), correlationGuid },
				{ nameof(userName),userName}
			});

		FxiResponse result = _manager.GetToken(userName, correlationGuid);

		_logger.LogInformation(
			$"Ending {nameof(GetFxiAuthentication)}",
			new Dictionary<string, object>()
			{
				{ nameof(correlationGuid), correlationGuid }
			});

		return StatusCode(result.Status.HttpStatusCode, result);
	}
}