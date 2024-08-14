// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   Core
//   BasicAuthManager.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Core.Http
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using CareSource.WC.Core.Extensions;
	using CareSource.WC.Core.Http.Interfaces;
	using CareSource.WC.Core.Transaction.Interfaces;
	using CareSource.WC.Entities.Transactions;
	using Microsoft.AspNetCore.Http;
	using Microsoft.Extensions.Logging;
	using Microsoft.Extensions.Primitives;

	public class BasicAuthManager : IAuthorizationManager
	{
		private readonly ILogger<BasicAuthManager> _logger;
		private readonly ITransactionContextManager _transactionContextManager;

		public BasicAuthManager(
			ITransactionContextManager transactionContextManager,
			ILogger<BasicAuthManager> logger)
		{
			_transactionContextManager = transactionContextManager;
			_logger = logger;
		}

		public string AuthType => "Basic Auth";

		public void Authorize(
			HttpContext context)
		{
			_logger.LogDebug("Received request to authorize."
			, new Dictionary<string, object>
			{
				{
					"Headers"
					, context?.Request?
						.Headers?
						.Select(h => new { Name=h.Key, Value=h.Value })
						.ToList()
				}
			});

			StringValues? authHeader = context?.Request?.Headers?["Authorization"];
			if (!authHeader.HasValue ||
			    !authHeader.Value.ToString()
				    .ToLower()
				    .StartsWith("basic"))
			{
				throw new UnauthorizedAccessException(
					"You must use Basic Auth in order to use this endpoint.");
			}

			string basicAuthParameter = authHeader.Value.ToString()
				.Replace(
					"basic ",
					"")
				.Replace(
					"Basic ",
					"");
			string decodedString = basicAuthParameter.Base64Decode();
			decodedString = decodedString.UrlDecode();

			string[] separatedString = decodedString?
				.Split(
					new[] {":"},
					StringSplitOptions.RemoveEmptyEntries);

			if (separatedString == null ||
			    separatedString.Length < 2)
			{
				throw new UnauthorizedAccessException("Invalid format for Basic Auth value.");
			}

			string username = separatedString[0];
			string password = separatedString[1];

			_logger.LogDebug("Storing Basic Auth user details in transaction context.");

			_transactionContextManager.CurrentContext.SecurityContext = new SecurityContext
			{
				AuthenticatedUserId = username,
				Password = password,
				Domain = "CareSource",
				Type = context.Request.Scheme.ToLower()
			};

			_logger.LogInformation("Successfully authorized!");
		}
	}
}