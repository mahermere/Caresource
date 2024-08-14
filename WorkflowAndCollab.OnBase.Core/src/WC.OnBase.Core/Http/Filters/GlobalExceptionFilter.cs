using System;
using System.Net.Http;
using System.Web.Http.Filters;
using CareSource.WC.OnBase.Core.Http.Interfaces;

namespace CareSource.WC.OnBase.Core.Http.Filters
{
	public class GlobalExceptionFilter : ExceptionFilterAttribute
	{
		public override void OnException(HttpActionExecutedContext context)
		{
            var httpStatusExceptionParser = (IHttpStatusExceptionParser)context.Request.GetDependencyScope()
                .GetService(typeof(IHttpStatusExceptionParser));

            if (httpStatusExceptionParser == null)
                throw new Exception($"Could not find type '{typeof(IHttpStatusExceptionParser).Name}' as a registered dependency.");

            httpStatusExceptionParser.DetermineResponse(context);
		}
	}
}