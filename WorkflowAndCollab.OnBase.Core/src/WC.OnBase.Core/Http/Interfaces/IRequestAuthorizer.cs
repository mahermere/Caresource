using System.Web.Http.Controllers;

namespace CareSource.WC.OnBase.Core.Http.Interfaces
{
    public interface IRequestAuthorizer
    {
        bool Authorize(HttpActionContext actionContext);
    }
}
