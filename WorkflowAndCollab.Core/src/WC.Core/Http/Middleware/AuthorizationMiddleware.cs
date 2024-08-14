// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   Core
//   AuthorizationMiddleware.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Core.Http.Middleware
{
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;
	using Microsoft.AspNetCore.Http;
	using Microsoft.Extensions.Logging;
	using CareSource.WC.Core.Http.Interfaces;
	using System;

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
				var authorizationManager = authorizationManagers.FirstOrDefault();

				_logger.LogDebug(
					$"Attempting request '{authorizationManager.AuthType}' authorization.");

				authorizationManager.Authorize(httpContext);
			}

			// Call the next middleware delegate in the pipeline 
			await _next.Invoke(httpContext);
		}
	}
}