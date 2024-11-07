// ------------------------------------------------------------------------------------------------
//  <copyright>
//    Copyright (c) CareSource, 2020-2022.  All rights reserved.
// 
//    FXIAuthentication
//    AuthorizationMiddleware.cs
//  </copyright>
//  ------------------------------------------------------------------------------------------------

namespace FXIAuthentication.Core.Http.Middleware;

public class AuthorizationMiddleware
{
	private readonly ILogger<AuthorizationMiddleware> _logger;
	private readonly RequestDelegate _next;

	public AuthorizationMiddleware(
		RequestDelegate next,
		ILogger<AuthorizationMiddleware> logger)
	{
		_next = next;
		_logger = logger;
	}

	public async Task Invoke(
		HttpContext httpContext,
		IEnumerable<IAuthorizationManager> authorizationManagers)
	{
		if ((authorizationManagers?.Count() ?? 0) == 0)
		{
			_logger.LogDebug("No request authorization required.");
		}
		else if (authorizationManagers.Count() > 1)
		{
			throw new Exception("Can only have one authorization manager configured at a time.");
		}
		else
		{
			IAuthorizationManager? authorizationManager = authorizationManagers.FirstOrDefault();

			_logger.LogDebug(
				$"Attempting request '{authorizationManager.AuthType}' authorization.");

			authorizationManager.Authorize(httpContext);
		}

		// Call the next middleware delegate in the pipeline 
		await _next.Invoke(httpContext);
	}
}