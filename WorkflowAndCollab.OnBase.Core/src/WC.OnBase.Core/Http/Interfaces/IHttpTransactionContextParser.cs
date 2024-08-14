using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace CareSource.WC.OnBase.Core.Http.Interfaces
{
    public interface IHttpTransactionContextParser
    {
        void ParseRequest(HttpActionContext actionContext);

        void ParseResponse(HttpActionExecutedContext actionExecutedContext);
    }
}
