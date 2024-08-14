using System.Web.Http.Filters;

namespace CareSource.WC.OnBase.Core.Http.Interfaces
{
	public interface IHttpStatusExceptionParser
	{
		void DetermineResponse(HttpActionExecutedContext context);
	}
}
