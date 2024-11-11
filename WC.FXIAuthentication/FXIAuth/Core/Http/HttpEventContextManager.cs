// ------------------------------------------------------------------------------------------------
//  <copyright>
//    Copyright (c) CareSource, 2020-2022.  All rights reserved.
// 
//    FXIAuthentication
//    HttpEventContextManager.cs
//  </copyright>
//  ------------------------------------------------------------------------------------------------

namespace FXIAuthentication.Core.Http;

using System.Text.RegularExpressions;
using FXIAuthentication.Core.Extensions;
using FXIAuthentication.Core.Helpers;
using FXIAuthentication.Core.Transaction;
using FXIAuthentication.Core.Transaction.Models;
using Microsoft.Extensions.Primitives;

public class HttpEventContextManager : IEventContextManager
{
	private readonly IAssemblyHelper _assemblyHelper;
	private readonly ILogger<HttpEventContextManager> _logger;

	public HttpEventContextManager(
		IAssemblyHelper assemblyHelper,
		ILogger<HttpEventContextManager> logger)
	{
		_assemblyHelper = assemblyHelper;
		_logger = logger;
	}

	public void SetEventContext(
		TransactionContext context,
		params object[] eventArgs)
	{
		HttpContext httpContext = eventArgs[0] as HttpContext;
		if (httpContext == null)
		{
			throw new Exception(
				"HttpContext is required to set EventContext on TransactionContext.");
		}

		string source = context.EventContext?.Source;
		_logger.LogDebug("Attempting to pull source from 'X-Source' header.");

		StringValues? sourceHeader = httpContext.Request?.Headers?["X-Source"];
		if (sourceHeader.HasValue &&
		    !sourceHeader.Value.ToString()
			    .IsNullOrWhiteSpace())
		{
			source = sourceHeader.Value.ToString();
		}

		string correlationGuid = context.EventContext?.CorrelationGuid;
		_logger.LogDebug(
			"Attempting to pull correlation guid from 'X-CorrelationGuid' header.");

		StringValues? correlationGuidHeader =
			httpContext.Request?.Headers?["X-CorrelationGuid"];
		if (correlationGuidHeader.HasValue &&
		    !correlationGuidHeader.Value.ToString()
			    .IsNullOrWhiteSpace())
		{
			correlationGuid = correlationGuidHeader.Value.ToString();
		}

		string version = "1";
		Match versionMatch = Regex.Match(
			httpContext.Request?.Path.Value ?? string.Empty,
			".*\\/v([0-9]+).*");
		if (versionMatch.Success)
		{
			version = versionMatch.Groups[1]
				.Value;
		}

		context.EventContext = new EventContext
		{
			CorrelationGuid = correlationGuid ??
			                  Guid.NewGuid()
				                  .ToString(),
			ParentGuid = context.EventContext?.ParentGuid,
			CurrentGuid = context.EventContext?.CurrentGuid
			              ?? Guid.NewGuid().ToString(),
			Destination = "OnBase",
			Source = source ?? httpContext.Connection.RemoteIpAddress.ToString(),
			ApplicationName = _assemblyHelper.GetEntryAssemblyName()
				?
				.Split(
					",")?[0],
			Event = $"{httpContext.Request?.Method} {httpContext.Request?.PathBase}",
			Version = version
		};
	}
}