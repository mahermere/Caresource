// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   Core
//   ExceptionMiddleware.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Core.Http.Middleware
{
	using System;
	using System.Threading.Tasks;
	using CareSource.WC.Core.Http.Interfaces;
	using CareSource.WC.Core.Transaction.Interfaces;
	using Microsoft.AspNetCore.Http;

	public class ExceptionMiddleware
	{
		private readonly IHttpStatusExceptionManager _httpStatusExceptionManager;
		private readonly RequestDelegate _next;

		public ExceptionMiddleware(
			RequestDelegate next,
			IHttpStatusExceptionManager httpStatusExceptionManager)
		{
			_next = next;
			_httpStatusExceptionManager = httpStatusExceptionManager;
		}

		public async Task Invoke(
			HttpContext httpContext,
			ITransactionContextManager transactionContextManager)
		{
			try
			{
				// Call the next middleware delegate in the pipeline 
				await _next.Invoke(httpContext);
			}
			catch (Exception ex)
			{
				transactionContextManager.PopulateTransactionException(ex);
				await _httpStatusExceptionManager.DetermineResponse(
					httpContext,
					ex);
			}
		}
	}
}