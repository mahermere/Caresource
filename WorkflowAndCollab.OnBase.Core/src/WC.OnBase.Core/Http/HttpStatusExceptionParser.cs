using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;
using CareSource.WC.OnBase.Core.Diagnostics.Interfaces;
using CareSource.WC.OnBase.Core.Http.Interfaces;
using CareSource.WC.OnBase.Core.Transaction.Interfaces;

namespace CareSource.WC.OnBase.Core.Http
{
	public class HttpStatusExceptionParser : IHttpStatusExceptionParser
	{
        private readonly ITransactionContextManager _transactionContextManager;
        private readonly ILogger _logger;

        public HttpStatusExceptionParser(ITransactionContextManager transactionContextManager
            , ILogger logger)
        {
            _transactionContextManager = transactionContextManager;
            _logger = logger;
        }

        public void DetermineResponse(HttpActionExecutedContext context)
        {
            var ex = context.Exception;

            if (ex is NotImplementedException)
                SetResponse(context, HttpStatusCode.NotImplemented, ex.Message, ex);
            else if (ex is ArgumentException)
                SetResponse(context, HttpStatusCode.BadRequest, ex.Message, ex);
            else if (ex is UnauthorizedAccessException)
                SetResponse(context, HttpStatusCode.Unauthorized, ex.Message, ex);
            else
            {
                var st = new StackTrace(ex, true);
                var frame = st.GetFrame(0);

                SetResponse(context, HttpStatusCode.InternalServerError
                    , $"{ex.Message}, Line: {frame.GetFileLineNumber()}, Method: {frame.GetMethod()}"
                    , ex);
            }
        }

        private void SetResponse(HttpActionExecutedContext context, HttpStatusCode code, string message, Exception ex)
        {
            context.Response = context.Request.CreateResponse(message);
            context.Response.StatusCode = code;

            _transactionContextManager.PopulateTransactionException(ex);

            _logger.LogError(message, ex
                , new Dictionary<string, object>
                {
                    { "StatusCode", code }
                });
        }
    }
}