// ------------------------------------------------------------------------------------------------
//  <copyright>
//    Copyright (c) CareSource, 2020-2022.  All rights reserved.
// 
//    FXIAuthentication
//    HttpStatusExceptionManager.cs
//  </copyright>
//  ------------------------------------------------------------------------------------------------

namespace FXIAuthentication.Core.Http;

using System.Diagnostics;
using System.Net;

public class HttpStatusExceptionManager : IHttpStatusExceptionManager
{
	private readonly ILogger<HttpStatusExceptionManager> _logger;

	public HttpStatusExceptionManager(
		ILogger<HttpStatusExceptionManager> logger) => _logger = logger;

	public async Task DetermineResponse(
		HttpContext context,
		Exception exception)
	{
		context.Response.Clear();

		if (exception is NotImplementedException)
		{
			await SetResponse(
				context,
				HttpStatusCode.NotImplemented,
				exception.Message);
		}
		else if (exception is ArgumentException)
		{
			await SetResponse(
				context,
				HttpStatusCode.BadRequest,
				exception.Message);
		}
		else if (exception is UnauthorizedAccessException)
		{
			await SetResponse(
				context,
				HttpStatusCode.Unauthorized,
				exception.Message);
		}
		else
		{
			StackTrace st = new(
				exception,
				true);
			StackFrame frame = st.GetFrame(0);

			await SetResponse(
				context,
				HttpStatusCode.InternalServerError,
				$"{exception.Message}, Line: {frame.GetFileLineNumber()}, Method: {frame.GetMethod()}");
		}
	}

	private async Task SetResponse(
		HttpContext context,
		HttpStatusCode code,
		string message)
	{
		_logger.LogError(
			$"Exception occured during processing. Status Code [${code.ToString()}], Description: {message}");

		context.Response.StatusCode = (int)code;
		context.Response.ContentType = MediaTypes.PlainText;
		await context.Response.WriteAsync(message);
	}
}