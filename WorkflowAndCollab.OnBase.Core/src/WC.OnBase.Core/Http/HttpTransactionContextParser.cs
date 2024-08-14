//  ------------------------------------------------------------------------------------------------
//  <copyright>
//    Copyright (c) CareSource, 2020-2022.  All rights reserved.
// 
//    OnBase.Core
//    HttpTransactionContextParser.cs
//  </copyright>
//  ------------------------------------------------------------------------------------------------

namespace CareSource.WC.OnBase.Core.Http
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Web.Http.Controllers;
	using System.Web.Http.Filters;
	using CareSource.WC.Entities.Transactions;
	using CareSource.WC.OnBase.Core.Diagnostics.Interfaces;
	using CareSource.WC.OnBase.Core.ExtensionMethods;
	using CareSource.WC.OnBase.Core.Helpers.Interfaces;
	using CareSource.WC.OnBase.Core.Http.Interfaces;
	using CareSource.WC.OnBase.Core.Services;
	using CareSource.WC.OnBase.Core.Transaction.Interfaces;

	public class HttpTransactionContextParser : IHttpTransactionContextParser
	{
		private readonly IEventContextManager _eventContextManager;
		private readonly IJsonSerializerHelper _jsonSerializerHelper;
		private readonly ILogger _logger;
		private readonly ITransactionContextManager _transactionContextManager;
		private readonly IXmlSerializerHelper _xmlSerializerHelper;

		public HttpTransactionContextParser(ITransactionContextManager transactionContextManager,
			IEventContextManager eventContextManager,
			IXmlSerializerHelper xmlSerializerHelper,
			IJsonSerializerHelper jsonSerializerHelper,
			ILogger logger)
		{
			_transactionContextManager = transactionContextManager;
			_eventContextManager = eventContextManager;
			_xmlSerializerHelper = xmlSerializerHelper;
			_jsonSerializerHelper = jsonSerializerHelper;
			_logger = logger;
		}

		public void ParseRequest(HttpActionContext actionContext)
		{
			TransactionContext httpTransactionContext = GetTransactionContext(actionContext);

			TransactionContext newTransactionContext =
				_transactionContextManager.InitializeContext(httpTransactionContext);

			if (httpTransactionContext == null)
			{
				newTransactionContext.EventContext = null;
			}

			_eventContextManager.SetEventContext(newTransactionContext, actionContext);

			_transactionContextManager.CurrentContext = newTransactionContext;
		}

		public void ParseResponse(HttpActionExecutedContext actionExecutedContext)
		{
			_transactionContextManager.FinalizeContext(_transactionContextManager.CurrentContext);
		}

		private TransactionContext GetTransactionContext(HttpActionContext actionContext)
		{
			_logger.LogDebug(
				"Attempting to pull source from 'X-Transaction-Context-Content-Type' header.");

			string tcContentType = "application/json";
			IEnumerable<string> tcContentTypeHeader = new List<string>();
			if (actionContext.Request?.Headers.TryGetValues("X-Transaction-Context-Content-Type",
					out tcContentTypeHeader) ?? false)
			{
				tcContentType = tcContentTypeHeader.FirstOrDefault();
			}

			_logger.LogDebug("Attempting to pull source from 'X-Transaction-Context' header.");

			IEnumerable<string> tcHeaders = new List<string>();
			actionContext.Request?.Headers.TryGetValues("X-Transaction-Context", out tcHeaders);
			string tcHeader = tcHeaders?.FirstOrDefault();
			if (!tcHeader.IsNullOrWhiteSpace())
			{
				TransactionContext transactionContext = null;
				if (tcContentType == MediaTypes.ApplicationXml)
				{
					try
					{
						string decodedString = tcHeader.Base64Decode();
						transactionContext = _xmlSerializerHelper.FromXml<TransactionContext>(decodedString);
					}
					catch (Exception ex)
					{
						throw new ArgumentException(
							"Failed to parse XML TransactionContext for " +
							$"'X-Transaction-Context' header. Error: {ex.Message}");
					}
				}
				else if (tcContentType == MediaTypes.ApplicationJson || tcContentType.IsNullOrWhiteSpace())
				{
					try
					{
						string decodedString = tcHeader.Base64Decode();
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