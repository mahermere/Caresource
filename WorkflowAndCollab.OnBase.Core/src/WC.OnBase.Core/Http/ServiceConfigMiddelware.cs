using CareSource.WC.OnBase.Core.Http.Filters;
using System.Web.Http;

namespace CareSource.WC.OnBase.Core.Http
{
	public static class ServiceConfigMiddelware
	{
		public static void Load(HttpConfiguration config)
		{
			config.Filters.Add(new GlobalExceptionFilter());
            config.Filters.Add(new GlobalTransactionContextFilter());
        }
	}
}