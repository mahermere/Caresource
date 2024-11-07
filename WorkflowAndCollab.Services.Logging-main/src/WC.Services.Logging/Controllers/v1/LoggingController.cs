//  ------------------------------------------------------------------------------------------------
//  <copyright>
//    Copyright (c) CareSource, 2020-2022.  All rights reserved.
// 
//    WC.Services.Logging
//    LoggingController.cs
//  </copyright>
//  ------------------------------------------------------------------------------------------------

namespace WC.Services.Logging.Controllers.v1
{
	using System.Net;
	using System.Text;
	using Microsoft.AspNetCore.Mvc;
	using WC.Services.Logging.Managers.v1;
	using WC.Services.Logging.Models.v1;

	[Route("api/v{version:apiVersion}/[controller]")]
	[ApiVersion("1")]
	[ApiController]
	public class LogController : ControllerBase
	{
		private readonly ILoggingManager _manager;

		public LogController(ILoggingManager manager)
			=> _manager = manager;

		[HttpPost]
		public IActionResult Log(
			[FromBody] Message message) =>
			Log(
				message,
				message.Event.LogLevel);

		private IActionResult Log(
			Message message,
			LogLevel logLevel)
		{
			string token = GetTokenFromHeader();
			return CompleteResult(_manager.Log(message, logLevel, token));
		}

		[HttpPost("Trace")]
		public IActionResult LogTrace([FromBody] Message message)
			=> Log(message, LogLevel.Trace);

		[HttpPost("Debug")]
		public IActionResult LogDebug([FromBody] Message message)
			=> Log(message, LogLevel.Debug);

		[HttpPost("Info")]
		public IActionResult LogInfo([FromBody] Message message)
			=> Log(message, LogLevel.Information);

		[HttpPost("Warning")]
		public IActionResult LogWarning([FromBody] Message message)
			=> Log(message, LogLevel.Warning);

		[HttpPost("Error")]
		public IActionResult LogError([FromBody] Message message)
			=> Log(message, LogLevel.Error);

		[HttpPost("Critical")]
		public IActionResult LogCritical([FromBody] Message message)
			=> Log(message, LogLevel.Critical);


		private IActionResult CompleteResult(ILoggingResponse result) =>
			result.Code switch
			{
				-1 => StatusCode(500 ,result),
				0 => Ok(result),
				4 => Unauthorized(result),
				_ => BadRequest(result)
			};

		private string GetTokenFromHeader()
		{
			string? authHeader = Request.Headers["Authorization"];

			if (authHeader != null)
			{
				string[] tokens = authHeader.Split(' ');
				byte[] bytes = Convert.FromBase64String(tokens[1]);
				string auth = Encoding.ASCII.GetString(bytes);

				if (auth.StartsWith("splunk:", StringComparison.InvariantCultureIgnoreCase))
				{
					return auth.Split(':')[1];
				}
				
				return auth;
			}

			return string.Empty;
		}
	}
}