// ------------------------------------------------------------------------------------------------
//  <copyright>
//    Copyright (c) CareSource, 2020-2022.  All rights reserved.
// 
//    FXIAuthentication
//    TransactionContextMiddleware.cs
//  </copyright>
//  ------------------------------------------------------------------------------------------------

namespace FXIAuthentication.Core.Http.Middleware;

using FXIAuthentication.Core.Extensions;
using FXIAuthentication.Core.Helpers;
using FXIAuthentication.Core.Transaction;
using FXIAuthentication.Core.Transaction.Models;
using Microsoft.Extensions.Primitives;

public class TransactionContextMiddleware
{
	private readonly RequestDelegate _next;

	private IJsonSerializerHelper _jsonSerializerHelper;
	private ILogger<TransactionContextMiddleware> _logger;

	public TransactionContextMiddleware(
		RequestDelegate next) => _next = next;

	public async Task Invoke(
		HttpContext httpContext,
		ITransactionContextManager transactionContextManager,
		IEventContextManager eventContextManager,
		IJsonSerializerHelper jsonSerializerHelper,
		ILogger<TransactionContextMiddleware> logger)
	{
		_jsonSerializerHelper = jsonSerializerHelper;
		_logger = logger;

		try
		{
			TransactionContext httpTransactionContext = GetTransactionContext(httpContext);

			TransactionContext newTransactionContext =
				transactionContextManager.InitializeContext(httpTransactionContext);

			if (httpTransactionContext == null)
			{
				newTransactionContext.EventContext = null;
			}

			eventContextManager.SetEventContext(newTransactionContext, httpContext);

			transactionContextManager.CurrentContext = newTransactionContext;

			// Call the next middleware delegate in the pipeline 
			await _next.Invoke(httpContext);
		}
		finally
		{
			transactionContextManager.FinalizeContext(transactionContextManager.CurrentContext);
		}
	}

	private TransactionContext GetTransactionContext(
		HttpContext httpContext)
	{
		_logger.LogDebug(
			"Attempting to pull source from 'X-Transaction-Context-Content-Type' header.");
		string tcContentType = "application/json";
		StringValues? tcContentTypeHeader =
			httpContext.Request?.Headers?["X-Transaction-Context-Content-Type"];
		if (tcContentTypeHeader.HasValue &&
		    !tcContentTypeHeader.Value.ToString()
			    .IsNullOrWhiteSpace())
		{
			tcContentType = tcContentTypeHeader.Value.ToString();
		}

		_logger.LogDebug("Attempting to pull source from 'X-Transaction-Context' header.");
		StringValues? tcHeader = httpContext.Request?.Headers?["X-Transaction-Context"];
		if (tcHeader.HasValue &&
		    !tcHeader.Value.ToString()
			    .IsNullOrWhiteSpace())
		{
			TransactionContext transactionContext = null;
			if (tcContentType == MediaTypes.ApplicationJson ||
			    tcContentType.IsNullOrWhiteSpace())
			{
				try
				{
					string decodedString = tcHeader.Value.ToString()
						.Base64Decode();
					transactionContext =
						_jsonSerializerHelper.FromJson<TransactionContext>(decodedString);
				}
				catch (Exception ex)
				{
					throw new ArgumentException(
						"Failed to parse JSON TransactionContext for " +
						$"'X-Transaction-Context' header. Error: {ex.Message}");
				}
			}
			else
			{
				throw new ArgumentException(
					$"This service does not support the value '{tcContentType}'" +
					" for the 'X-Transaction-Context-Content-Type' header.");
			}

			return transactionContext;
		}

		return null;
	}
}