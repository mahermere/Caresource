using CareSource.WC.OnBase.Core.Http.Interfaces;
using System;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace CareSource.WC.OnBase.Core.Http.Filters
{
    public class GlobalTransactionContextFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var httpTransactionContextParser = (IHttpTransactionContextParser)actionContext.Request.GetDependencyScope()
                .GetService(typeof(IHttpTransactionContextParser));

            if (httpTransactionContextParser == null)
                throw new Exception($"Could not find type '{typeof(IHttpTransactionContextParser).Name}' as a registered dependency.");

            httpTransactionContextParser.ParseRequest(actionContext);
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            var httpTransactionContextParser = (IHttpTransactionContextParser)actionExecutedContext.Request.GetDependencyScope()
                .GetService(typeof(IHttpTransactionContextParser));

            if (httpTransactionContextParser == null)
                throw new Exception($"Could not find type '{typeof(IHttpTransactionContextParser).Name}' as a registered dependency.");

            httpTransactionContextParser.ParseResponse(actionExecutedContext);
        }
    }
}
