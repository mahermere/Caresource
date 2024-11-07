// ------------------------------------------------------------------------------------------------
//  <copyright>
//    Copyright (c) CareSource, 2020-2022.  All rights reserved.
// 
//    FXIAuthentication
//    AuthManager.cs
//  </copyright>
//  ------------------------------------------------------------------------------------------------

namespace FXIAuthentication.Managers.v1;

using System.Net;
using FXIAuthentication.Adapters.v1;
using FXIAuthentication.Core.Extensions;
using FXIAuthentication.Models.v1;
using Microsoft.Extensions.Caching.Memory;

public class AuthManager : IAuthManager
{
	private readonly IAuthAdapter _adapter;
	private readonly IMemoryCache _cache;
	private readonly ILogger<AuthManager> _logger;

	public AuthManager(
		IAuthAdapter adapter,
		IMemoryCache cache,
		ILogger<AuthManager> logger)
	{
		_adapter = adapter;
		_cache = cache;
		_logger = logger;
	}

	public FxiResponse GetToken(
		string userName,
		Guid correlationGuid)
	{
		_logger.LogInformation(
			$"Starting {nameof(AuthManager)}.{nameof(GetToken)}",
			new Dictionary<string, object>()
			{
				{ nameof(correlationGuid), correlationGuid },
				{ nameof(userName),userName}
			});

		if (!_cache.TryGetValue("Token", out FxiResponse tokenData))
		{
			tokenData = _adapter.GetToken(userName, correlationGuid);

			if (tokenData != null
				&& tokenData.Status.HttpStatusCode == (int)HttpStatusCode.Created)
			{
				_cache.Set(
					"Token",
					tokenData,
					DateTimeOffset.Now.AddMinutes(45.0));

				_logger.LogDebug(
					$"Added {nameof(tokenData)} to cache.",
					new Dictionary<string, object>()
					{
						{ nameof(correlationGuid), correlationGuid }, { nameof(tokenData), tokenData }
					});
			}
			else
			{
				_logger.LogDebug(
					$"Bad Request.",
					new Dictionary<string, object>()
					{
						{ nameof(correlationGuid), correlationGuid }, { nameof(tokenData), tokenData }
					});
			}
		}
		else
		{
			_logger.LogDebug(
				$"Pulled {nameof(tokenData)} from cache.",
				new Dictionary<string, object>()
				{
					{ nameof(correlationGuid), correlationGuid },
					{ nameof(tokenData),tokenData}
				});
		}

		_logger.LogInformation(
			$"Ending {nameof(AuthManager)}.{nameof(GetToken)}",
			new Dictionary<string, object>()
			{
				{ nameof(correlationGuid), correlationGuid }
			});

		return tokenData;
	}
}