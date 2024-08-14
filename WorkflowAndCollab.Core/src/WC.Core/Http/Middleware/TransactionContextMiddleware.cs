// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   Core
//   TransactionContextMiddleware.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Core.Http.Middleware
{
	using System;
	using System.Threading.Tasks;
	using CareSource.WC.Core.Extensions;
	using CareSource.WC.Core.Helpers.Interfaces;
	using CareSource.WC.Core.Transaction.Interfaces;
	using CareSource.WC.Entities.Transactions;
	using Microsoft.AspNetCore.Http;
	using Microsoft.Extensions.Logging;
	using Microsoft.Extensions.Primitives;

	public class TransactionContextMiddleware
	{
		private readonly RequestDelegate _next;

		private IJsonSerializerHelper _jsonSerializerHelper;
		private ILogger<TransactionContextMiddleware> _logger;
		private IXmlSerializerHelper _xmlSerializerHelper;

		public TransactionContextMiddleware(
			RequestDelegate next) => _next = next;

		public async Task Invoke(
			HttpContext httpContext,
			ITransactionContextManager transactionContextManager,
			IEventContextManager eventContextManager,
			IJsonSerializerHelper jsonSerializerHelper,
			IXmlSerializerHelper xmlSerializerHelper,
			ILogger<TransactionContextMiddleware> logger)
		{
			_xmlSerializerHelper = xmlSerializerHelper;
			_jsonSerializerHelper = jsonSerializerHelper;
			_logger = logger;

			try
			{
				var httpTransactionContext = GetTransactionContext(httpContext);

				var newTransactionContext = transactionContextManager.InitializeContext(httpTransactionContext);

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
				if (tcContentType == MediaTypes.ApplicationXml)
				{
					try
					{
						string decodedString = tcHeader.Value.ToString()
							.Base64Decode();
						transactionContext =
							_xmlSerializerHelper.FromXml<TransactionContext>(decodedString);
					}
					catch (Exception ex)
					{
						throw new ArgumentException(
							"Failed to parse XML TransactionContext for " +
							$"'X-Transaction-Context' header. Error: {ex.Message}");
					}
				}
				else if (tcContentType == MediaTypes.ApplicationJson ||
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
}