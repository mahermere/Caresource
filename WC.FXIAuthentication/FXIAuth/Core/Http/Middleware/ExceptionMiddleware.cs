// ------------------------------------------------------------------------------------------------
//  <copyright>
//    Copyright (c) CareSource, 2020-2022.  All rights reserved.
// 
//    FXIAuthentication
//    ExceptionMiddleware.cs
//  </copyright>
//  ------------------------------------------------------------------------------------------------

namespace FXIAuthentication.Core.Http.Middleware;

using FXIAuthentication.Core.Transaction;

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