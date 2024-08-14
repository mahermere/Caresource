using System;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using CareSource.WC.OnBase.Core.Http.Interfaces;

namespace CareSource.WC.OnBase.Core.Http.Filters
{
	public class OnBaseAuthorizeFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var requestAuthorizer = (IRequestAuthorizer)actionContext.Request.GetDependencyScope()
                .GetService(typeof(IRequestAuthorizer));

            if (requestAuthorizer == null)
                throw new Exception($"Could not find type '{typeof(IRequestAuthorizer).Name}' as a registered dependency.");

            if (!requestAuthorizer.Authorize(actionContext))
                throw new UnauthorizedAccessException($"Unable to Authorize request.");
        }
	}
}